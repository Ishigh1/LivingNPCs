using Terraria;

namespace LivingNPCs.HouseStructure.HouseParts.TileInfo.Sets
{
	public abstract class TileInfoSet
	{
		public TileInfo Tile;
		public TileInfo Door;
		
		public TileInfo WorkBench;
		public TileInfo LeftChair;
		public TileInfo RightChair;
		public TileInfo Candle;
		
		public TileInfo Wall;
		public static void ChairFacingRight(int x, int y)
		{
			Framing.GetTileSafely(x, y).frameX += 18;
			Framing.GetTileSafely(x, y - 1).frameX += 18;
		}
	}
}