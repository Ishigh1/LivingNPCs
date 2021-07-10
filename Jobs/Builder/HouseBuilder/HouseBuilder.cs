using System;
using LivingNPCs.HouseStructure;
using LivingNPCs.HouseStructure.HouseParts.TileInfo;
using LivingNPCs.NPCs;
using LivingNPCs.TileTool;
using Microsoft.Xna.Framework;
using Terraria;

namespace LivingNPCs.Jobs.Builder.HouseBuilder
{
	public class HouseBuilder : Builder
	{
		public HouseBuilderState HouseBuilderState;
		public TileInfo TileInfo;

		public HouseBuilder()
		{
			HouseBuilderState = HouseBuilderState.LookingForNewHouseEmplacement;
		}

		public override bool AI(EasierNPC npc)
		{
			switch (HouseBuilderState)
			{
				case HouseBuilderState.LookingForNewHouseEmplacement:
					(Point point, int size, int direction) = FindNearbyTile(npc, 50, FindOpenField);
					if (point == Point.Zero)
						return true;
					House = new House(point, direction, size % 100);
					HouseBuilderState = HouseBuilderState.WaitingForCleanSpot;
					return false;
				case HouseBuilderState.WaitingForCleanSpot:
					if (House.IsBuildable())
					{
						HouseBuilderState = HouseBuilderState.SearchingNextTile;
						goto case HouseBuilderState.SearchingNextTile;
					}

					return true;
				case HouseBuilderState.SearchingNextTile:
					(Point location, TileInfo tileInfo) = House.GetNextPointToBuild();
					if (location == Point.Zero)
					{
						HouseBuilderState = HouseBuilderState.Finished;
						goto case HouseBuilderState.Finished;
					}
					else
					{
						npc.SetObjective(location, 5);
						TileInfo = tileInfo;
						HouseBuilderState = HouseBuilderState.GoingToNextTile;
						goto case HouseBuilderState.GoingToNextTile;
					}
				case HouseBuilderState.GoingToNextTile:
					if (npc.ReachedObjective() && !npc.OnObjective() && npc.Stop())
					{
						HouseBuilderState = HouseBuilderState.Building;
						TileAction = new TileBuilder(npc.Objective.location.X, npc.Objective.location.Y, npc.ToolSet,
							TileInfo);
						goto case HouseBuilderState.Building;
					}

					npc.Walk();
					return false;
				case HouseBuilderState.Building:
					if (TileAction.UseItem())
					{
						TileAction = null;
						HouseBuilderState = HouseBuilderState.SearchingNextTile;
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
				if (WorldGen.SolidOrSlopedTile(location.X + length * direction, location.Y))
				{
					length--;
					break;
				}

				if (WorldGen.SolidOrSlopedTile(location.X + length * direction, location.Y + 1))
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
	}
}