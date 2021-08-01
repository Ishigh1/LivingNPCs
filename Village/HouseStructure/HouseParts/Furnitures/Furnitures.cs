using LivingNPCs.Info.TileInfo.Sets;
using Microsoft.Xna.Framework;
using Terraria;

namespace LivingNPCs.Village.HouseStructure.HouseParts.Furnitures
{
	public class Furnitures : HousePart
	{
		public Furnitures(TileInfoSet tileInfoSet, HousePart floor)
		{
			Point craftingTable =
				new Point((floor.FirstEnd.X + floor.SecondEnd.X) / 2, floor.FirstEnd.Y - 1);
			Blocks.Add((craftingTable, tileInfoSet.WorkBench));
			if (Main.rand.Next(2) == 0)
				Blocks.Add((new Point(craftingTable.X - 1, craftingTable.Y), tileInfoSet.RightChair));
			//Blocks.Add((new Point(craftingTable.X + 2, craftingTable.Y), tileInfoSet.Candle));
			else
				Blocks.Add((new Point(craftingTable.X + 2, craftingTable.Y), tileInfoSet.LeftChair));
			//Blocks.Add((new Point(craftingTable.X - 1, craftingTable.Y), tileInfoSet.Candle));
		}
	}
}