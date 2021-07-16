using System;
using System.Collections.Generic;
using LivingNPCs.NPCs;
using LivingNPCs.Village.OrderSystem.Order;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace LivingNPCs.Jobs.Crafter
{
	public class Crafter : Job
	{
		public Dictionary<int, Recipe> AllowedRecipes;
		public CrafterState CrafterState;

		public Crafter()
		{
			AllowedRecipes = new Dictionary<int, Recipe>
			{
				{ItemID.Torch, Main.recipe[0]},
				{ItemID.WoodWall, Main.recipe[441]},
				{ItemID.WoodenDoor, Main.recipe[475]},
				{ItemID.WoodenChair, Main.recipe[476]},
				{ItemID.WorkBench, Main.recipe[480]},
			};
			CrafterState = CrafterState.Finished;
		}

		public override bool AI(EasierNPC easierNPC)
		{
			CraftingOrder craftingOrder = (CraftingOrder) CurrentOrder;
			switch (CrafterState)
			{
				case CrafterState.FindingStation:
					(Point point, int _, int _) = FindNearbyTile(easierNPC, 5,
						(location, _) => Framing.GetTileSafely(location).type == craftingOrder.Recipe.requiredTile[0]
							? 100
							: -1);
					if (point != Point.Zero)
					{
						easierNPC.SetObjective(point);
						CrafterState = CrafterState.GoingToStation;
						goto case CrafterState.GoingToStation;
					}

					return true;
				case CrafterState.GoingToStation:
					if (easierNPC.ReachedObjective())
					{
						CrafterState = CrafterState.Crafting;
						goto case CrafterState.Crafting;
					}

					easierNPC.Walk();

					return false;
				case CrafterState.Crafting:
					craftingOrder.Completed = true;
					CrafterState = CrafterState.Finished;
					return false;
				case CrafterState.Finished:
					return true;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public override Order NewOrder(EasierNPC easierNPC)
		{
			ItemOrder itemOrder =
				easierNPC.OrderCollection.GetOrder<ItemOrder>(
					order => AllowedRecipes.ContainsKey(order.ItemInfo.ItemId));
			if (itemOrder is null)
				return null;

			Recipe recipe = AllowedRecipes[itemOrder.ItemInfo.ItemId];
			CrafterState = recipe.requiredTile[0] == -1 ? CrafterState.Crafting : CrafterState.FindingStation;
			CraftingOrder craftingOrder = new CraftingOrder(recipe, itemOrder);
			foreach (Order otherOrder in craftingOrder.OtherOrders) easierNPC.OrderCollection.AddOrder(otherOrder);

			return craftingOrder;
		}
	}
}