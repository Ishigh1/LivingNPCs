using LivingNPCs.NPCs;
using Terraria.ModLoader;

namespace LivingNPCs
{
	public class LivingNPCs : Mod
	{
		public VillagerFactory VillagerFactory;

		public override void Load()
		{
			VillagerFactory = new VillagerFactory();
		}
	}
}