using LivingNPCs.Jobs.Builder.HouseBuilder;
using LivingNPCs.Jobs.Gatherer.WoodCutter;
using LivingNPCs.Jobs.HouseCleaner;
using LivingNPCs.TileTool;
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
			
			NPC.ToolSet = new ToolSet(ItemID.CopperAxe, ItemID.CopperHammer, ItemID.CopperPickaxe, 1f);//Should be at 0.5f for game, 1f for debug
			AddJob(new WoodCutter());
			AddJob(new HouseBuilder());
			AddJob(new HouseCleaner());
			SetJobToActive<WoodCutter>();
		}

		public override bool AI()
		{
			bool returnValue = base.AI();
			switch (ActiveJob)
			{
				case WoodCutter woodCutter:
				{
					if (woodCutter.WoodCuttingState != WoodCuttingState.GatheringWood &&
					    NPC.Inventory.TryGetValue(ItemID.Wood, out int amount) && amount >= 11 &&
					    GetJob<HouseBuilder>().HouseBuilderState != HouseBuilderState.Finished)
						SetJobToActive<HouseBuilder>();
					break;
				}
				case HouseBuilder houseBuilder:
				{
					if (houseBuilder.HouseBuilderState == HouseBuilderState.Finished)
					{
						SetJobToActive<WoodCutter>();
					}
					else if (houseBuilder.HouseBuilderState == HouseBuilderState.WaitingForCleanSpot)
					{
						SetJobToActive<HouseCleaner>();
						GetJob<HouseCleaner>().House = houseBuilder.House;
					}

					break;
				}
				case HouseCleaner houseCleaner:
				{
					if (houseCleaner.HouseCleanerState == HouseCleanerState.Finished)
						SetJobToActive<HouseBuilder>();
					break;
				}
			}

			return returnValue;
		}

		public override void DrawWeapon(SpriteBatch spriteBatch, Color drawColor)
		{
			ActiveJob.TileAction?.DrawSwing(NPC.NPC, spriteBatch, drawColor);
		}
	}
}