using System;
using LivingNPCs.NPCs;
using LivingNPCs.TileTool;
using LivingNPCs.Village.OrderSystem.Order;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace LivingNPCs.Jobs.Gatherer.WoodCutter
{
	public class WoodCutter : GatheringJob
	{
		public WoodCuttingState WoodCuttingState;

		public WoodCutter()
		{
			WoodCuttingState = WoodCuttingState.Finished;
		}

		public override bool AI(EasierNPC easierNPC)
		{
			switch (WoodCuttingState)
			{
				case WoodCuttingState.LookingForWood:
					Point treeLocation = FindNearbyTile(easierNPC, 50, IsTree).location;
					if (treeLocation != Point.Zero)
					{
						easierNPC.SetObjective(treeLocation, 2);
						WoodCuttingState = WoodCuttingState.GoingToWood;
						goto case WoodCuttingState.GoingToWood;
					}

					return true;
				case WoodCuttingState.GoingToWood:
					if (easierNPC.ReachedObjective() && easierNPC.Stop())
					{
						WoodCuttingState = WoodCuttingState.CuttingWood;
						TileAction = new TileBreaker(easierNPC.Objective.location.X, easierNPC.Objective.location.Y,
							easierNPC.ToolSet);
						goto case WoodCuttingState.CuttingWood;
					}

					easierNPC.Walk();
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
					(bool pickedUpItems, bool remainingItems) = easierNPC.GatherItems(10);
					if (!remainingItems)
					{
						if (--WaitingTime == 0)
						{
							if (easierNPC.Inventory.TryGetValue(ItemID.Acorn, out int acornAmount) && acornAmount > 0)
							{
								WorldGen.PlaceTile(easierNPC.Objective.location.X, easierNPC.Objective.location.Y,
									TileID.Saplings);
								easierNPC.Inventory[ItemID.Acorn]--;
							}

							CheckInventoryFill();
						}
					}
					else if (pickedUpItems)
					{
						WaitingTime = 30;
					}

					return false;
				case WoodCuttingState.Finished:
					return true;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void CheckInventoryFill()
		{
			WoodCuttingState = ((ItemOrder) CurrentOrder)?.CheckValidity() is false
				? WoodCuttingState.Finished
				: WoodCuttingState.LookingForWood;
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

		public override Order NewOrder(EasierNPC easierNPC)
		{
			ItemOrder itemOrder =
				easierNPC.OrderCollection.GetOrder<ItemOrder>(order => order.ItemInfo.ItemId == ItemID.Wood);
			if (itemOrder != null) WoodCuttingState = WoodCuttingState.LookingForWood;

			return itemOrder;
		}
	}
}