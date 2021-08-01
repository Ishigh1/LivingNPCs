using System;
using Terraria;

namespace LivingNPCs.Info.TileInfo
{
	public class TileInfo
	{
		public Action<int, int> Extra;
		public bool IsWall;
		public ItemInfo.ItemInfo ItemInfo;
		public int Style;
		public int TileId;

		public TileInfo(int itemId, int tileId, bool isWall = false, int style = 0, Action<int, int> extra = null)
		{
			ItemInfo = new ItemInfo.ItemInfo(itemId);
			TileId = tileId;
			IsWall = isWall;
			Style = style;
			Extra = extra;
		}

		public void Place(int x, int y)
		{
			if (IsWall)
				WorldGen.PlaceWall(x, y, TileId);
			else
				WorldGen.PlaceTile(x, y, TileId, forced: true, style: Style);
			Extra?.Invoke(x, y);
		}
	}
}