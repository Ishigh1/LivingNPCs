using Terraria;
using Terraria.ModLoader;

namespace LivingNPCs.TileTool
{
	public class TileBreaker : TileAction
	{
		public bool IsWall;

		public TileBreaker(int x, int y, Item tool, int power, float efficiency, bool isWall = false)
		{
			IsWall = isWall;
			Initialize(x, y, tool, power, efficiency);
		}

		public TileBreaker(int x, int y, Item hammer, Item axe, Item pickaxe, float efficiency)
		{
			int tileType = Framing.GetTileSafely(x, y).type;
			if (Main.tileHammer[tileType])
				Initialize(x, y, hammer, hammer.hammer, efficiency);
			else if (Main.tileAxe[tileType])
				Initialize(x, y, axe, axe.axe, efficiency);
			else
				Initialize(x, y, pickaxe, pickaxe.pick, efficiency);
		}

		public override bool UseItem()
		{
			if (--Delay > 0)
				return false;

			Delay = UseTime;

			int damage = Power;
			int tileId = HitTile.HitObject(X, Y, 1);
			TileLoader.MineDamage(Power, ref damage);

			bool tileDestroyed = false;
			if (HitTile.AddDamage(tileId, damage) >= 100)
			{
				HitTile.Clear(tileId);
				if (IsWall)
					WorldGen.KillWall(X, Y);
				else
					WorldGen.KillTile(X, Y);
				tileDestroyed = true;
			}
			else
			{
				if (IsWall)
					WorldGen.KillWall(X, Y, true);
				else
					WorldGen.KillTile(X, Y, true);
			}

			if (damage != 0) HitTile.Prune();
			return tileDestroyed;
		}
	}
}