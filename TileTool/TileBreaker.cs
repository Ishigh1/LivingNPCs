using Terraria;
using Terraria.ModLoader;

namespace LivingNPCs.TileTool
{
	public class TileBreaker : TileAction
	{
		public bool IsWall;

		public TileBreaker(int x, int y, Tool tool, bool isWall = false)
		{
			IsWall = isWall;
			Initialize(x, y, tool);
		}

		public TileBreaker(int x, int y, ToolSet toolSet)
		{
			int tileType = Framing.GetTileSafely(x, y).type;
			if (Main.tileHammer[tileType])
				Initialize(x, y, toolSet.Hammer);
			else if (Main.tileAxe[tileType])
				Initialize(x, y, toolSet.Axe);
			else
				Initialize(x, y, toolSet.PickAxe);
		}

		public override bool UseItem()
		{
			if (!base.UseItem())
				return false;

			int power = Tool.GetPower();
			int damage = (int) (power * Tool.Proficiency);
			int tileId = HitTile.HitObject(X, Y, 1);
			TileLoader.MineDamage(power, ref damage);

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