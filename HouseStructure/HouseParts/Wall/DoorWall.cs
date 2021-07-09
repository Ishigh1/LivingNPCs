using LivingNPCs.HouseStructure.HouseParts.TileInfo;
using LivingNPCs.HouseStructure.HouseParts.TileInfo.Sets;
using Microsoft.Xna.Framework;

namespace LivingNPCs.HouseStructure.HouseParts.Wall
{
	public class DoorWall : HousePart
	{
		public DoorWall(TileInfoSet tileInfoSet, Point location, int size)
		{
			FirstEnd = new Point(location.X, location.Y - size);
			SecondEnd = new Point(location.X, location.Y - 1);

			for (int y = FirstEnd.Y; y <= SecondEnd.Y - 3; y++)
				Blocks.Add((new Point(location.X, y), tileInfoSet.Tile));
			Blocks.Add((new Point(SecondEnd.X, SecondEnd.Y - 2), tileInfoSet.Door));
		}
	}
}