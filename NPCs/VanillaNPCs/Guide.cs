using LivingNPCs.Jobs.Builder.HouseBuilder;
using LivingNPCs.Jobs.Crafter;
using LivingNPCs.Jobs.Gatherer.WoodCutter;
using LivingNPCs.Jobs.HouseCleaner;
using LivingNPCs.TileTool;
using LivingNPCs.Village.OrderSystem.Order;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace LivingNPCs.NPCs.VanillaNPCs
{
	public class Guide : Villager
	{
		public override void SetDefaults(NPC npc)
		{
			base.SetDefaults(npc);

			EasierNPC.ToolSet =
				new ToolSet(ItemID.CopperAxe, ItemID.CopperHammer, ItemID.CopperPickaxe,
					1f); //Should be at 0.5f for game, 1f for debug
			AddJob(new HouseBuilder());
			AddJob(new HouseCleaner());
			AddJob(new WoodCutter());
			AddJob(new Crafter());
			SetJobToActive<HouseBuilder>();
			
			EasierNPC.OrderCollection.AddOrder(new HouseOrder());
		}

		public override void DrawWeapon(SpriteBatch spriteBatch, Color drawColor)
		{
			ActiveJob?.TileAction?.DrawSwing(EasierNPC.NPC, spriteBatch, drawColor);
		}
	}
}