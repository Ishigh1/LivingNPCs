using LivingNPCs.HouseStructure.HouseParts.TileInfo;
using LivingNPCs.HouseStructure.HouseParts.TileInfo.Sets;
using Microsoft.Xna.Framework;

namespace LivingNPCs.HouseStructure.HouseParts.Ceiling
{
	public class Ceiling : HousePart
	{
		public Ceiling(TileInfoSet tileInfoSet, Point leftEnd, Point rightEnd)
		{
			FirstEnd = new Point(leftEnd.X + 1, leftEnd.Y);
			SecondEnd = new Point(rightEnd.X - 1, leftEnd.Y);
			for (int x = FirstEnd.X; x <= SecondEnd.X; x++) Blocks.Add((new Point(x, FirstEnd.Y), tileInfoSet.Tile));
		}
	}
}