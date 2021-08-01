using LivingNPCs.NPCs;
using Terraria.ModLoader;
#if DEBUG
using System.IO;

#endif

namespace LivingNPCs
{
	public class LivingNPCs : Mod
	{
#if DEBUG
		public static StreamWriter Writer;
#endif
		public VillagerFactory VillagerFactory;

		public override void Load()
		{
			VillagerFactory = new VillagerFactory();
#if DEBUG
			Writer = new StreamWriter(@"D:\debug.txt", true);
#endif
		}
	}
}