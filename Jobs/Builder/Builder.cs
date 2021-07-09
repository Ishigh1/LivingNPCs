using LivingNPCs.HouseStructure;
using Terraria;

namespace LivingNPCs.Jobs.Builder
{
	public abstract class Builder : Job
	{
		public float Efficiency;
		public Item Hammer;
		public House House;
	}
}