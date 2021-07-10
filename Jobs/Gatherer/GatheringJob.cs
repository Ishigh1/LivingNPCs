using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace LivingNPCs.Jobs.Gatherer
{
	public abstract class GatheringJob : Job
	{
		public (Point location, Chest chest) HomeChest;
		public int WaitingTime;

		public void SetHomeChest(Point chestLocation)
		{
			HomeChest.location = chestLocation;
			int chestId = Chest.FindChestByGuessing(chestLocation.X - 1, chestLocation.Y - 1);
			HomeChest.chest = Main.chest[chestId];
		}

		public virtual int IsChest(Point point, int _)
		{
			Tile tile = Framing.GetTileSafely(point);
			return tile.active() && (tile.type == TileID.Containers || tile.type == TileID.Containers2) ? 1 : -1;
		}
	}
}