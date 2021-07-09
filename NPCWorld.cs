using System.Linq;
using LivingNPCs.NPCs;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace LivingNPCs
{
	public class NPCWorld : ModWorld
	{
		public override void Load(TagCompound tagCompound)
		{
			foreach (VillagerNPC villagerNPC in Main.npc.Where(npc => npc.active)
				.Select(npc => npc.GetGlobalNPC<VillagerNPC>())
				.Where(villagerNPC => villagerNPC != null)) villagerNPC.Load(tagCompound);
		}

		public override TagCompound Save()
		{
			TagCompound tagCompound = new TagCompound();
			foreach (VillagerNPC villagerNPC in Main.npc.Where(npc => npc.active)
				.Select(npc => npc.GetGlobalNPC<VillagerNPC>()).Where(villagerNPC => villagerNPC != null))
				villagerNPC.Save(tagCompound);
			return tagCompound;
		}
	}
}