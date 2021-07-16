using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace LivingNPCs.Jobs.Gatherer
{
	public abstract class GatheringJob : Job
	{
		public int WaitingTime;

		public virtual int IsChest(Point point, int _)
		{
			Tile tile = Framing.GetTileSafely(point);
			return tile.active() && (tile.type == TileID.Containers || tile.type == TileID.Containers2) ? 1 : -1;
		}
	}
}