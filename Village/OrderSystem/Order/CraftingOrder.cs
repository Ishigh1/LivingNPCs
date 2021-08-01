using System.Collections.Generic;
using System.Linq;
using LivingNPCs.Info.ItemInfo;
using Terraria;
using Terraria.ID;

namespace LivingNPCs.Village.OrderSystem.Order
{
	public class CraftingOrder : DependantOrder
	{
		public ItemOrder BaseOrder;
		public Recipe Recipe;
		public int TileId;
		public Village Village;

		public CraftingOrder(Recipe recipe, ItemOrder baseOrder, Village village, int tileId = -1)
		{
			Recipe = recipe;
			BaseOrder = baseOrder;
			Village = village;
			TileId = tileId;
		}

		public override void OnCompleted()
		{
			if (BaseOrder.Completed) //Refund components
			{
				foreach (Order otherOrder in OtherOrders)
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
			else
			{
				BaseOrder.Requester.AddItemToInventory(Recipe.createItem.type, Recipe.createItem.stack);
				BaseOrder.Completed = true;
			}
#if DEBUG
			base.OnCompleted();
#endif
		}

		public override bool IsValid()
		{
			return BaseOrder.CheckValidity();
		}

		public override bool IsAvailable()
		{
			return base.IsAvailable() && Village.ContainsTile(TileId);
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

#if DEBUG
		public override string ToString()
		{
			return base.ToString() + "recipe: " + ItemID.GetUniqueKey(Recipe.createItem.type);
		}
#endif
	}
}