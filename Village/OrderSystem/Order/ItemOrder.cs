using LivingNPCs.Info.ItemInfo;
using LivingNPCs.Info.TileInfo;
using LivingNPCs.NPCs;

namespace LivingNPCs.Village.OrderSystem.Order
{
	public class ItemOrder : Order
	{
		public EasierNPC Requester;
		public ItemInfo ItemInfo;

		public ItemOrder(EasierNPC requester, ItemInfo itemInfo)
		{
			Requester = requester;
			ItemInfo = itemInfo;
		}

		public override bool IsValid()
		{
			if(Requester.Inventory.TryGetValue(ItemInfo.ItemId, out int amount) && amount >= ItemInfo.Stack)
			{
				Requester.Inventory[ItemInfo.ItemId] = amount - ItemInfo.Stack;
				return false;
			}

			return true;
		}

		public override bool IsAvailable()
		{
			return true;
		}
	}
}