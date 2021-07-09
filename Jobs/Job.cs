using System;
using LivingNPCs.NPCs;
using LivingNPCs.TileTool;
using Microsoft.Xna.Framework;
using Terraria;

namespace LivingNPCs.Jobs
{
	public abstract class Job
	{
		public TileAction TileAction;
		public abstract bool AI(EasierNPC npc);

		public (Point location, int value, int direction) FindNearbyTile(EasierNPC npc, int reach,
			Func<Point, int, int> tileCondition)
		{
			Point left = new Point(npc.LeftX, npc.LowerY);
			Point right = new Point(npc.RightX, npc.LowerY);
			int bestValue = 0;
			Point bestLocation = Point.Zero;
			int bestDirection = 0;

			for (int x = npc.LeftX + 1; x < npc.RightX; x++)
			{
				int y = npc.LowerY;
				while (!WorldGen.SolidTile(Framing.GetTileSafely(x, y)))
					y++;

				while (WorldGen.SolidTile(Framing.GetTileSafely(x, y)))
					y--;
				Point point = new Point(x, y);

				int direction = x - npc.LeftX < (npc.RightX - npc.LeftX) / 2 ? -1 : 1;
				int value = tileCondition(point, direction);
				if (value > bestValue)
				{
					bestValue = value;
					bestLocation = point;
					bestDirection = direction;
				}
			}

			for (int i = 0; i < reach; i++)
			{
				left.X--;
				left.Y += npc.NPC.height / 16;

				while (!WorldGen.SolidTile(Framing.GetTileSafely(left)))
					left.Y++;

				while (WorldGen.SolidTile(Framing.GetTileSafely(left)))
					left.Y--;


				right.X++;
				right.Y += npc.NPC.height / 16;

				while (!WorldGen.SolidTile(Framing.GetTileSafely(right)))
					right.Y++;

				while (WorldGen.SolidTile(Framing.GetTileSafely(right)))
					right.Y--;

				int value = tileCondition(left, -1);
				if (value > bestValue)
				{
					bestValue = value;
					bestLocation = new Point(left.X, left.Y);
					bestDirection = -1;
				}

				value = tileCondition(right, 1);
				if (value > bestValue)
				{
					bestValue = value;
					bestLocation = new Point(right.X, right.Y);
					bestDirection = 1;
				}
			}

			return (bestLocation, bestValue, bestDirection);
		}
	}
}