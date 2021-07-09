using System.Collections.Generic;
using LivingNPCs.HouseStructure.HouseParts;
using LivingNPCs.HouseStructure.HouseParts.Background;
using LivingNPCs.HouseStructure.HouseParts.Ceiling;
using LivingNPCs.HouseStructure.HouseParts.Floor;
using LivingNPCs.HouseStructure.HouseParts.Furnitures;
using LivingNPCs.HouseStructure.HouseParts.TileInfo;
using LivingNPCs.HouseStructure.HouseParts.TileInfo.Sets;
using LivingNPCs.HouseStructure.HouseParts.Wall;
using Microsoft.Xna.Framework;
using Terraria;

namespace LivingNPCs.HouseStructure
{
	public class House
	{
		public List<HousePart> HouseParts;
		public List<Point> TilesToClean;

		public House(Point location, int direction, int size)
		{
			TileInfoSet tileInfoSet = new WoodenTileInfo();
			HousePart flatFloor = new FlatFloor(tileInfoSet, location, direction, size);

			HousePart leftWall, rightWall;
			int wallType = Main.rand.Next(3);
			if (wallType <= 1)
				leftWall = new DoorWall(tileInfoSet, flatFloor.FirstEnd, 6);
			else
				leftWall = new Wall(tileInfoSet, flatFloor.FirstEnd, 6);
			if (wallType >= 1)
				rightWall = new DoorWall(tileInfoSet, flatFloor.SecondEnd, 6);
			else
				rightWall = new Wall(tileInfoSet, flatFloor.SecondEnd, 6);

			HousePart ceiling = new Ceiling(tileInfoSet, leftWall.FirstEnd, rightWall.FirstEnd);
			HousePart furnitures = new Furnitures(tileInfoSet, flatFloor);
			HousePart walls = new RectangleBackground(tileInfoSet, leftWall.FirstEnd, flatFloor.SecondEnd);

			HouseParts = new List<HousePart>
			{
				flatFloor, furnitures, leftWall, rightWall, ceiling, walls
			};
			
			TilesToClean = new List<Point>();
			foreach (HousePart solidHousePart in HouseParts)
			foreach ((Point point, TileInfo _) in solidHousePart.Blocks)
				if (Framing.GetTileSafely(point).active())
					TilesToClean.Add(point);
		}

		public bool IsBuildable()
		{
			return TilesToClean.Count == 0;
		}

		public (Point location, TileInfo tileInfo) GetNextPointToBuild()
		{
			foreach (HousePart solidHousePart in HouseParts)
			{
				(Point location, TileInfo tileInfo) nextPoint = solidHousePart.GetNextBlock();
				if (nextPoint.location != Point.Zero) return nextPoint;
			}

			return (Point.Zero, null);
		}

		public Point GetNextPointToClean()
		{
			if (TilesToClean.Count == 0)
				return Point.Zero;
			Point location = TilesToClean[0];
			TilesToClean.RemoveAt(0);
			return !Framing.GetTileSafely(location).active() ? GetNextPointToClean() : location;
		}
	}
}