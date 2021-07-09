using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace LivingNPCs.NPCs
{
	public class VillagerNPC : GlobalNPC
	{
		public Villager Villager;
		public override bool InstancePerEntity => true;

		public override void SetDefaults(NPC npc)
		{
			Villager = ((LivingNPCs) mod).VillagerFactory.GetVillagerFromNpc(npc);
			Villager?.SetDefaults(npc);
		}

		public override bool PreAI(NPC npc)
		{
			return Villager == null || Villager.AI();
		}

		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			Villager?.DrawWeapon(spriteBatch, drawColor);
			return true;
		}

		public void Save(TagCompound tagCompound)
		{
			Villager?.Save(tagCompound);
		}

		public void Load(TagCompound tagCompound)
		{
			Villager?.Load(tagCompound);
		}
	}
}