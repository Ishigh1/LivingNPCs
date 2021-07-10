using LivingNPCs.HouseStructure.HouseParts.TileInfo;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace LivingNPCs.TileTool
{
	public class TileBuilder : TileAction
	{
		public float Progress;
		public TileInfo TileInfo;

		public TileBuilder(int x, int y, ToolSet toolSet, TileInfo tileInfo)
		{
			TileInfo = tileInfo;
			Initialize(x, y, toolSet.Building);
		}

		public override bool UseItem()
		{
			if (!base.UseItem())
				return false;

			Progress += Tool.GetPower() * Tool.Proficiency;
			if (Progress >= 100)
			{
				TileInfo.Place(X, Y, null);
				return true;
			}

			return false;
		}

		public virtual void PlayPlacementSound(int x, int y, int tileId)
		{
			switch (tileId)
			{
				case TileID.MagicalIceBlock:
					Main.PlaySound(SoundID.Item30, x * 16, y * 16);
					break;
				case TileID.MinecartTrack:
					Main.PlaySound(SoundID.Item52, x * 16, y * 16);
					break;
				case TileID.CopperCoinPile:
				case TileID.SilverCoinPile:
				case TileID.GoldCoinPile:
				case TileID.PlatinumCoinPile:
					Main.PlaySound(SoundID.Coins, x * 16, y * 16);
					break;
				default:
					Main.PlaySound(SoundID.Dig, x * 16, y * 16);
					break;
			}

			if (tileId == TileID.Demonite || tileId == TileID.DemoniteBrick)
				for (int k = 0; k < 3; k++)
					Dust.NewDust(new Vector2(x * 16, y * 16), 16, 16, 14 /*DustID.Demonite*/);
		}
	}
}