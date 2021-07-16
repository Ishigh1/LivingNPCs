using System;
using Terraria;

namespace LivingNPCs.TileTool
{
	public class Tool
	{
		public Item Item;
		public float Proficiency;
		public ToolType ToolType;

		public Tool(int type, float proficiency, ToolType toolType)
		{
			Item = new Item();
			Item.SetDefaults(type);
			Proficiency = proficiency;
			ToolType = toolType;
		}

		public Tool(Item item, float proficiency, ToolType toolType)
		{
			Item = item;
			Proficiency = proficiency;
			ToolType = toolType;
		}

		public int GetPower()
		{
			switch (ToolType)
			{
				case ToolType.Axe:
					return Item.axe;
				case ToolType.Hammer:
					return Item.hammer;
				case ToolType.Pickaxe:
					return Item.pick;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}