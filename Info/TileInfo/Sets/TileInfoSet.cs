using Terraria;

namespace LivingNPCs.Info.TileInfo.Sets
{
	public abstract class TileInfoSet
	{
		public TileInfo Candle;
		public TileInfo Door;
		public TileInfo LeftChair;
		public TileInfo RightChair;
		public TileInfo Tile;

		public TileInfo Wall;

		public TileInfo WorkBench;

		public static void ChairFacingRight(int x, int y)
		{
			Framing.GetTileSafely(x, y).frameX += 18;
			Framing.GetTileSafely(x, y - 1).frameX += 18;
		}
	}
}