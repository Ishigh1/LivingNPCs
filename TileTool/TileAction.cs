using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace LivingNPCs.TileTool
{
	public abstract class TileAction
	{
		public int Animation;
		public int Delay;
		public HitTile HitTile;
		public Tool Tool;
		public int X;
		public int Y;

		protected void Initialize(int x, int y, Tool tool)
		{
			HitTile = new HitTile();
			Tool = tool;
			Delay = (int) (Tool.Item.useTime / Tool.Proficiency);
			X = x;
			Y = y;
		}

		public virtual bool UseItem()
		{
			if (--Delay > 0)
				return false;

			Delay = (int) (Tool.Item.useTime / Tool.Proficiency);
			return true;
		}

		public void DrawSwing(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D itemTexture = Main.itemTexture[Tool.Item.type];

			(Vector2 weaponStart, float rotation) = npc.GetSwingStats(150,
				100 - Animation, npc.spriteDirection,
				Tool.Item.width, Tool.Item.height);

			Vector2 weaponPosition = weaponStart + (weaponStart - npc.Center);
			Vector2 origin = itemTexture.Size() * new Vector2(npc.spriteDirection != 1 ? 1 : 0, 1f);

			SpriteEffects spriteEffects =
				npc.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			if (npc.direction == 1)
				weaponPosition.X += npc.width / 2f;
			else
				weaponPosition.X -= npc.width / 2f;

			if (Animation < 65 && Animation > 0.5f)
				weaponPosition.Y += npc.height / 3f;

			spriteBatch.Draw(itemTexture,
				new Vector2((int) (weaponPosition.X - Main.screenPosition.X),
					(int) (weaponPosition.Y - Main.screenPosition.Y)), null, npc.GetAlpha(drawColor), rotation, origin,
				npc.scale, spriteEffects, 0f);

			int frameShift = Main.npcFrameCount[npc.type] - NPCID.Sets.AttackFrameCount[npc.type];
			int frame = Animation < 35 ? frameShift :
				Animation < 50 ? frameShift + 1 :
				Animation < 65 ? frameShift + 2 :
				frameShift + 3;
			npc.frame.Y = frame * Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type];
			Animation = (Animation + 100 / Tool.Item.useAnimation) % 100;
		}
	}
}