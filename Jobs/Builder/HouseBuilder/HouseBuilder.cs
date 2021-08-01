using System;
using LivingNPCs.NPCs;
using LivingNPCs.TileTool;
using LivingNPCs.Village.HouseStructure;
using LivingNPCs.Village.OrderSystem.Order;
using Microsoft.Xna.Framework;
using Terraria;

namespace LivingNPCs.Jobs.Builder.HouseBuilder
{
	public class HouseBuilder : Builder
	{
		public HouseBuilderState HouseBuilderState;

		public HouseBuilder()
		{
			HouseBuilderState = HouseBuilderState.LookingForNewHouseEmplacement;
		}

		public override bool AI(EasierNPC easierNPC)
		{
			switch (HouseBuilderState)
			{
				case HouseBuilderState.LookingForNewHouseEmplacement:
					(Point point, int score, int direction) = FindNearbyTile(easierNPC, 50, FindOpenField);
					if (point != Point.Zero)
					{
						House house = new House(easierNPC, point, direction, score % 100);
						easierNPC.Village.Home = house;
						CurrentOrder.Completed = true;
						HouseBuilderState = HouseBuilderState.Finished;
					}

					return true;
				case HouseBuilderState.GoingToNextTile:
					if (easierNPC.ReachedObjective() && !easierNPC.OnObjective() && easierNPC.Stop())
					{
						HouseBuilderState = HouseBuilderState.Building;
						TileAction = new TileBuilder(easierNPC.Objective.location.X, easierNPC.Objective.location.Y,
							easierNPC.ToolSet,
							((BuildingOrder) CurrentOrder).TileInfo);
						goto case HouseBuilderState.Building;
					}

					easierNPC.Walk();
					return false;
				case HouseBuilderState.Building:
					if (TileAction.UseItem())
					{
						TileAction = null;
						HouseBuilderState = HouseBuilderState.Finished;
						goto case HouseBuilderState.Finished;
					}

					return false;
				case HouseBuilderState.Finished:
					return true;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public virtual int FindOpenField(Point location, int direction)
		{
			int length;
			int solidGround = 0;
			for (length = 0; length < 15; length++)
			{
				if (WorldGen.SolidTile(location.X + length * direction, location.Y))
				{
					length--;
					break;
				}

				if (WorldGen.SolidTile(location.X + length * direction, location.Y + 1))
				{
					solidGround++;
				}
				else if (solidGround > 7)
				{
					length--;
					break;
				}
			}

			if (solidGround == length) //Perfect floor
				return length * 10001;
			else
				return length + solidGround * 100;
		}

		public override Order NewOrder(EasierNPC easierNPC)
		{
			if (HouseBuilderState == HouseBuilderState.LookingForNewHouseEmplacement)
			{
				return easierNPC.Village.GetOrder<HouseOrder>();
			}
			else
			{
				BuildingOrder buildingOrder = easierNPC.Village.GetOrder<BuildingOrder>();
				if (buildingOrder != null)
				{
					CachedObjective = (buildingOrder.Location, 5);
					HouseBuilderState = HouseBuilderState.GoingToNextTile;
				}

				return buildingOrder;
			}
		}
	}
}