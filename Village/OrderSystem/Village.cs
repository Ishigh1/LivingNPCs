using System.Collections.Generic;
using LivingNPCs.Village.HouseStructure;
using Microsoft.Xna.Framework;

namespace LivingNPCs.Village.OrderSystem
{
	public class Village : OrderCollection
	{
		public List<int> Tiles;
		public House Home;

		public Village()
		{
			Tiles = new List<int>();
		}

		public void AddTile(int tileId)
		{
			if (!Tiles.Contains(tileId))
				Tiles.Add(tileId);
		}

		public bool ContainsTile(int tileId)
		{
			return tileId == -1 || Tiles.Contains(tileId);
		}
	}
}