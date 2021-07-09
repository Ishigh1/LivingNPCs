using LivingNPCs.Jobs.Builder.HouseBuilder;
using LivingNPCs.Jobs.Gatherer.WoodCutter;
using LivingNPCs.Jobs.HouseCleaner;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace LivingNPCs.NPCs.VanillaNPCs
{
	public class Guide : Villager
	{
		public HouseBuilder HouseBuilderJob;
		public HouseCleaner HouseCleanerJob;
		public WoodCutter WoodCutterJob;

		public override void SetDefaults(NPC npc)
		{
			WoodCutterJob = new WoodCutter(Axe, 1f); //Should be at 0.5f for game, 1f for debug
			HouseBuilderJob = new HouseBuilder(Hammer, 1f);
			HouseCleanerJob = new HouseCleaner(Hammer, Axe, PickAxe, 1f);
			Job = WoodCutterJob;
			base.SetDefaults(npc);
		}

		public override bool AI()
		{
			bool returnValue = base.AI();
			switch (Job)
			{
				case WoodCutter woodCutter:
				{
					if (woodCutter.WoodCuttingState != WoodCuttingState.GatheringWood &&
					    NPC.Inventory.TryGetValue(ItemID.Wood, out int amount) && amount >= 11 &&
					    HouseBuilderJob.HouseBuilderState != HouseBuilderState.Finished)
						Job = HouseBuilderJob;
					break;
				}
				case HouseBuilder houseBuilder:
				{
					if (houseBuilder.HouseBuilderState == HouseBuilderState.Finished)
					{
						Job = WoodCutterJob;
					}
					else if (houseBuilder.HouseBuilderState == HouseBuilderState.WaitingForCleanSpot)
					{
						Job = HouseCleanerJob;
						HouseCleanerJob.House = houseBuilder.House;
					}

					break;
				}
				case HouseCleaner houseCleaner:
				{
					if (houseCleaner.HouseCleanerState == HouseCleanerState.Finished)
						Job = HouseBuilderJob;
					break;
				}
			}

			return returnValue;
		}

		public override void DrawWeapon(SpriteBatch spriteBatch, Color drawColor)
		{
			Job.TileAction?.DrawSwing(NPC.NPC, spriteBatch, drawColor);
		}
	}
}