using System;
using System.Collections.Generic;
using LivingNPCs.NPCs;
using LivingNPCs.TileTool;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace LivingNPCs.Jobs.Gatherer.WoodCutter
{
	public class WoodCutter : GatheringJob
	{
		public Dictionary<int, int> InventoryToSave;
		public WoodCuttingState WoodCuttingState;

		public WoodCutter()
		{
			WoodCuttingState = WoodCuttingState.LookingForWood;
			InventoryToSave = new Dictionary<int, int>
			{
				{ItemID.Acorn, 5}
			};
		}

		public override bool AI(EasierNPC npc)
		{
			switch (WoodCuttingState)
			{
				case WoodCuttingState.LookingForChest:
					Point chestLocation = FindNearbyTile(npc, 50, IsChest).location;
					if (chestLocation != Point.Zero)
					{
						SetHomeChest(chestLocation);
						WoodCuttingState = WoodCuttingState.LookingForWood;
						goto case WoodCuttingState.LookingForWood;
					}

					return true;
				case WoodCuttingState.LookingForWood:
					Point treeLocation = FindNearbyTile(npc, 10, IsTree).location;
					if (treeLocation != Point.Zero)
					{
						npc.SetObjective(treeLocation, 2);
						WoodCuttingState = WoodCuttingState.GoingToWood;
						goto case WoodCuttingState.GoingToWood;
					}

					return true;
				case WoodCuttingState.GoingToWood:
					if (npc.ReachedObjective() && npc.Stop())
					{
						WoodCuttingState = WoodCuttingState.CuttingWood;
						TileAction = new TileBreaker(npc.Objective.location.X, npc.Objective.location.Y, npc.ToolSet);
						goto case WoodCuttingState.CuttingWood;
					}

					npc.Walk();
					return false;
				case WoodCuttingState.CuttingWood:
					if (TileAction.UseItem())
					{
						TileAction = null;
						WoodCuttingState = WoodCuttingState.GatheringWood;
						WaitingTime = 30;
					}

					return false;
				case WoodCuttingState.GatheringWood:
					(bool pickedUpItems, bool remainingItems) = npc.GatherItems(10);
					if (!remainingItems)
					{
						if (--WaitingTime == 0)
						{
							if (npc.Inventory.TryGetValue(ItemID.Acorn, out int acornAmount) && acornAmount > 0)
							{
								WorldGen.PlaceTile(npc.Objective.location.X, npc.Objective.location.Y, TileID.Saplings);
								npc.Inventory[ItemID.Acorn]--;
							}

							CheckInventoryFill(npc);
						}
					}
					else if (pickedUpItems)
					{
						WaitingTime = 30;
					}

					return false;
				case WoodCuttingState.GoingToChest:
					if (npc.ReachedObjective() && npc.Stop())
					{
						npc.DumpAllInChest(HomeChest.chest, InventoryToSave);
						WoodCuttingState = WoodCuttingState.LookingForWood;
						goto case WoodCuttingState.LookingForWood;
					}

					npc.Walk();
					return false;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void CheckInventoryFill(EasierNPC npc)
		{
			int itemCount = 50;
			foreach (KeyValuePair<int, int> i in npc.Inventory) itemCount -= i.Value;
			itemCount = 0; //Debug code
			if (itemCount >= 0)
			{
				WoodCuttingState = WoodCuttingState.LookingForWood;
			}
			else
			{
				npc.SetObjective(HomeChest.location, 2);
				WoodCuttingState = WoodCuttingState.GoingToChest;
			}
		}

		public virtual int IsTree(Point point, int _)
		{
			Tile tile = Framing.GetTileSafely(point);
			int value = -1;
			int y = point.Y;

			while (tile.active() && tile.type == TileID.Trees)
			{
				tile = Framing.GetTileSafely(point.X, --y);
				value++;
			}

			return value;
		}
	}
}