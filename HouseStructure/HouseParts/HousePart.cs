using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LivingNPCs.HouseStructure.HouseParts
{
	public abstract class HousePart
	{
		public List<(Point location, TileInfo.TileInfo tileInfo)> Blocks;
		public Point FirstEnd; //TopMost or LeftMost end of the structure

		public int NextBlock;
		public Point SecondEnd; //BottomMost or RightMost end of the structure

		protected HousePart()
		{
			Blocks = new List<(Point location, TileInfo.TileInfo tileInfo)>();
		}

		public (Point location, TileInfo.TileInfo tileInfo) GetNextBlock()
		{
			return NextBlock == Blocks.Count ? (Point.Zero, null) : Blocks[NextBlock++];
		}
	}
}