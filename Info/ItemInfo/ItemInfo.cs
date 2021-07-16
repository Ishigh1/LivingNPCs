using Terraria.ID;

namespace LivingNPCs.Info.ItemInfo
{
	public class ItemInfo
	{
		public int ItemId;
		public int Stack;

		public ItemInfo(int itemId, int stack = 1)
		{
			ItemId = itemId;
			Stack = stack;
		}

		public override string ToString()
		{
			return ItemID.GetUniqueKey(ItemId) + "*" + Stack;
		}
	}
}