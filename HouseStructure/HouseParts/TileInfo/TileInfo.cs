using System;
using System.Collections.Generic;
using Terraria;

namespace LivingNPCs.HouseStructure.HouseParts.TileInfo
{
	public class TileInfo
	{
		public bool IsWall;
		public int ItemId;
		public int Style;
		public int TileId;
		public Action<int, int> Extra;

		public TileInfo(int itemId, int tileId, bool isWall = false, int style = 0, Action<int, int> extra = null)
		{
			ItemId = itemId;
			TileId = tileId;
			IsWall = isWall;
			Style = style;
			Extra = extra;
		}

		public bool Place(int x, int y, Dictionary<int, int> inventory)
		{
			if (IsWall)
				WorldGen.PlaceWall(x, y, TileId);
			else
				WorldGen.PlaceTile(x, y, TileId, style: Style);
			Extra?.Invoke(x, y);
			return true;
		}
	}
}