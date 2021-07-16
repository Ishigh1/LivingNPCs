using Terraria.ID;

namespace LivingNPCs.Info.TileInfo.Sets
{
	public class WoodenTileInfo : TileInfoSet
	{
		public WoodenTileInfo()
		{
			Tile = new TileInfo(ItemID.Wood, TileID.WoodBlock);
			Door = new TileInfo(ItemID.WoodenDoor, TileID.ClosedDoor, style: 0);

			WorkBench = new TileInfo(ItemID.WorkBench, TileID.WorkBenches, style: 0);
			LeftChair = new TileInfo(ItemID.WoodenChair, TileID.Chairs, style: 0);
			RightChair = new TileInfo(ItemID.WoodenChair, TileID.Chairs, style: 0, extra: ChairFacingRight);
			Candle = new TileInfo(ItemID.Torch, TileID.Torches, style: 0);

			Wall = new TileInfo(ItemID.WoodWall, WallID.Wood, true);
		}
	}
}