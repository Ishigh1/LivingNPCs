using System.Collections.Generic;
using LivingNPCs.Info.TileInfo;
using LivingNPCs.Info.TileInfo.Sets;
using LivingNPCs.NPCs;
using LivingNPCs.Village.HouseStructure.HouseParts;
using LivingNPCs.Village.HouseStructure.HouseParts.Background;
using LivingNPCs.Village.HouseStructure.HouseParts.Ceiling;
using LivingNPCs.Village.HouseStructure.HouseParts.Floor;
using LivingNPCs.Village.HouseStructure.HouseParts.Furnitures;
using LivingNPCs.Village.HouseStructure.HouseParts.Wall;
using LivingNPCs.Village.OrderSystem;
using LivingNPCs.Village.OrderSystem.Order;
using Microsoft.Xna.Framework;
using Terraria;

namespace LivingNPCs.Village.HouseStructure
{
	public class House
	{
		public List<HousePart> HouseParts;

		public House(EasierNPC builder, Point location, int direction, int size)
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

			foreach (HousePart housePart in HouseParts)
			foreach ((Point point, TileInfo tileInfo) in housePart.Blocks)
				builder.OrderCollection.AddOrder(new BuildingOrder(builder, point, tileInfo));
		}
	}
}