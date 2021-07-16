using System.Collections.Generic;
using System.Linq;
using LivingNPCs.Info.ItemInfo;
using Terraria;
using Terraria.ID;

namespace LivingNPCs.Village.OrderSystem.Order
{
	public class CraftingOrder : DependantOrder
	{
		public Recipe Recipe;
		public ItemOrder BaseOrder;

		public CraftingOrder(Recipe recipe, ItemOrder baseOrder)
		{
			Recipe = recipe;
			BaseOrder = baseOrder;
		}

		public override void OnCompleted()
		{
			if (BaseOrder.Completed)//Refund components
			{
				foreach (Order otherOrder in OtherOrders)
				{
					if (otherOrder.Completed)
					{
						ItemInfo itemInfo = ((ItemOrder) otherOrder).ItemInfo;
						BaseOrder.Requester.AddItemToInventory(itemInfo.ItemId, itemInfo.Stack);
					}
					else
					{
						otherOrder.Completed = true;
					}
				}
			}
			else
			{
				BaseOrder.Completed = true;
				int excess = Recipe.createItem.stack - BaseOrder.ItemInfo.Stack;
				if (excess > 0)
					BaseOrder.Requester.AddItemToInventory(Recipe.createItem.type, excess);
			}
		}

		public override bool IsValid()
		{
			return !BaseOrder.CheckValidity();
		}

		public override List<Order> GenerateOtherOrders()
		{
			return Recipe.requiredItem.TakeWhile(item => item.type != ItemID.None)
				.Select(item => new ItemInfo(item.type, item.stack))
				.Select(itemInfo => new ItemOrder(BaseOrder.Requester, itemInfo)).Cast<Order>().ToList();
		}

		public override List<Order> Refresh()
		{
			return null;
		}
	}
}