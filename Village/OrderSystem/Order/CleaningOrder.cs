using LivingNPCs.Info.TileInfo;
using Microsoft.Xna.Framework;
using Terraria;

namespace LivingNPCs.Village.OrderSystem.Order
{
	public class CleaningOrder : Order
	{
		public Point Location;
		public TileInfo TileInfo;

		public CleaningOrder(Point location, TileInfo tileInfo)
		{
			Location = location;
			TileInfo = tileInfo;
		}

		public override bool IsValid()
		{
			Tile tile = Framing.GetTileSafely(Location);
			if (TileInfo.IsWall)
				return tile.wall != 0 && tile.wall != TileInfo.TileId;
			else
				return tile.active() && tile.type != TileInfo.TileId;
		}

		public override bool IsAvailable()
		{
			return true;
		}
	}
}