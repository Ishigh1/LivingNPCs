using LivingNPCs.Info.TileInfo.Sets;
using Microsoft.Xna.Framework;

namespace LivingNPCs.Village.HouseStructure.HouseParts.Background
{
	public class RectangleBackground : HousePart
	{
		public RectangleBackground(TileInfoSet tileInfoSet, Point topLeft, Point bottomRight)
		{
			for (int x = topLeft.X + 1; x < bottomRight.X; x++)
			for (int y = topLeft.Y + 1; y < bottomRight.Y; y++)
				Blocks.Add((new Point(x, y), tileInfoSet.Wall));
		}
	}
}