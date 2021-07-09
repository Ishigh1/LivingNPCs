using LivingNPCs.NPCs.VanillaNPCs;
using Terraria;
using Terraria.ID;

namespace LivingNPCs.NPCs
{
	public class VillagerFactory
	{
		public virtual Villager GetVillagerFromNpc(NPC npc)
		{
			switch (npc.type)
			{
				case NPCID.Guide:
					return new Guide();
			}

			return null;
		}
	}
}