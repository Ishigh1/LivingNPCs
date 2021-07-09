using System;
using LivingNPCs.HouseStructure;
using LivingNPCs.NPCs;
using LivingNPCs.TileTool;
using Microsoft.Xna.Framework;
using Terraria;

namespace LivingNPCs.Jobs.HouseCleaner
{
	public class HouseCleaner : Job
	{
		public Item Axe;
		public float Efficiency;
		public Item Hammer;
		public House House;
		public HouseCleanerState HouseCleanerState;
		public Item Pickaxe;

		public HouseCleaner(Item hammer, Item axe, Item pickaxe, float efficiency)
		{
			HouseCleanerState = HouseCleanerState.SearchingNextTile;

			Hammer = hammer;
			Axe = axe;
			Pickaxe = pickaxe;
			Efficiency = efficiency;
		}

		public override bool AI(EasierNPC npc)
		{
			switch (HouseCleanerState)
			{
				case HouseCleanerState.SearchingNextTile:
					Point location = House.GetNextPointToClean();
					if (location == Point.Zero)
					{
						HouseCleanerState = HouseCleanerState.Finished;
						goto case HouseCleanerState.Finished;
					}
					else
					{
						npc.SetObjective(location, 2);
						HouseCleanerState = HouseCleanerState.GoingToNextTile;
						goto case HouseCleanerState.GoingToNextTile;
					}
				case HouseCleanerState.GoingToNextTile:
					if (npc.ReachedObjective() && npc.Stop())
					{
						HouseCleanerState = HouseCleanerState.Destroying;
						TileAction = new TileBreaker(npc.Objective.location.X, npc.Objective.location.Y,
							Hammer, Axe, Pickaxe, Efficiency);
						goto case HouseCleanerState.Destroying;
					}

					npc.Walk();
					return false;
				case HouseCleanerState.Destroying:
					if (TileAction.UseItem())
					{
						TileAction = null;
						HouseCleanerState = HouseCleanerState.SearchingNextTile;
					}

					return false;
				case HouseCleanerState.Finished:
					return true;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}