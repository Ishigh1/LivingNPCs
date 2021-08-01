using System;
using LivingNPCs.NPCs;
using LivingNPCs.TileTool;
using LivingNPCs.Village.OrderSystem.Order;

namespace LivingNPCs.Jobs.HouseCleaner
{
	public class HouseCleaner : Job
	{
		public HouseCleanerState HouseCleanerState;

		public HouseCleaner()
		{
			HouseCleanerState = HouseCleanerState.Finished;
		}

		public override bool AI(EasierNPC easierNPC)
		{
			switch (HouseCleanerState)
			{
				case HouseCleanerState.GoingToNextTile:
					if (easierNPC.ReachedObjective() && easierNPC.Stop())
					{
						HouseCleanerState = HouseCleanerState.Destroying;
						TileAction = new TileBreaker(easierNPC.Objective.location.X, easierNPC.Objective.location.Y,
							easierNPC.ToolSet);
						goto case HouseCleanerState.Destroying;
					}

					easierNPC.Walk();
					return false;
				case HouseCleanerState.Destroying:
					if (TileAction.UseItem())
					{
						TileAction = null;
						HouseCleanerState = HouseCleanerState.Finished;
					}

					return false;
				case HouseCleanerState.Finished:
					return true;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public override Order NewOrder(EasierNPC easierNPC)
		{
			CleaningOrder cleaningOrder = easierNPC.Village.GetOrder<CleaningOrder>();
			if (cleaningOrder != null)
			{
				CachedObjective = (cleaningOrder.Location, 5);
				HouseCleanerState = HouseCleanerState.GoingToNextTile;
			}

			return cleaningOrder;
		}
	}
}