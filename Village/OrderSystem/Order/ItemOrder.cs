using LivingNPCs.Info.ItemInfo;
using LivingNPCs.NPCs;
#if DEBUG
using Terraria.ID;

#endif

namespace LivingNPCs.Village.OrderSystem.Order
{
	public class ItemOrder : Order
	{
		public ItemInfo ItemInfo;
		public EasierNPC Requester;

		public ItemOrder(EasierNPC requester, ItemInfo itemInfo)
		{
			Requester = requester;
			ItemInfo = itemInfo;
		}

		public override bool IsValid()
		{
			return !Requester.AddItemToInventory(ItemInfo.ItemId, -ItemInfo.Stack);
		}

		public override bool IsAvailable()
		{
			return true;
		}

#if DEBUG
		public override string ToString()
		{
			return base.ToString() + "item: " + ItemID.GetUniqueKey(ItemInfo.ItemId);
		}
#endif
	}
}