using Terraria;
using Terraria.ID;

namespace LivingNPCs.TileTool
{
	public class ToolSet
	{
		public Tool Axe;
		public Tool Hammer;
		public Tool Building;
		public Tool PickAxe;

		public ToolSet(int axeType, int hammerType, int pickaxeType, float baseProficiency)
		{
			Axe = new Tool(axeType, baseProficiency, ToolType.Axe);
			Hammer = new Tool(hammerType, baseProficiency, ToolType.Hammer);
			Building = new Tool(Hammer.Item, baseProficiency, ToolType.Hammer);
			PickAxe = new Tool(pickaxeType, baseProficiency, ToolType.Pickaxe);
		}
	}
}