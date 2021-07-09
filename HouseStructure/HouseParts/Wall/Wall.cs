using LivingNPCs.HouseStructure.HouseParts.TileInfo;
using LivingNPCs.HouseStructure.HouseParts.TileInfo.Sets;
using Microsoft.Xna.Framework;

namespace LivingNPCs.HouseStructure.HouseParts.Wall
{
	public class Wall : HousePart
	{
		public Wall(TileInfoSet tileInfoSet, Point location, int size)
		{
			FirstEnd = new Point(location.X, location.Y - size);
			SecondEnd = new Point(location.X, location.Y - 1);

			for (int y = FirstEnd.Y; y <= SecondEnd.Y; y++)
				Blocks.Add((new Point(location.X, y), tileInfoSet.Tile));
		}
	}
}