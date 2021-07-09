using LivingNPCs.HouseStructure.HouseParts.TileInfo;
using LivingNPCs.HouseStructure.HouseParts.TileInfo.Sets;
using Microsoft.Xna.Framework;

namespace LivingNPCs.HouseStructure.HouseParts.Floor
{
	public class FlatFloor : HousePart
	{
		public FlatFloor(TileInfoSet tileInfoSet, Point location, int direction, int size)
		{
			if (direction == 1)
			{
				FirstEnd = new Point(location.X, location.Y);
				SecondEnd = new Point(location.X + size, location.Y);
			}
			else
			{
				FirstEnd = new Point(location.X - size, location.Y);
				SecondEnd = new Point(location.X, location.Y);
			}

			for (int x = FirstEnd.X; x <= SecondEnd.X; x++)
				Blocks.Add((new Point(x, location.Y), tileInfoSet.Tile));
		}
	}
}