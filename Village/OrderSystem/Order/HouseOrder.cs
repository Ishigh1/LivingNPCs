namespace LivingNPCs.Village.OrderSystem.Order
{
	public class HouseOrder : Order
	{
		public override bool IsValid()
		{
			return true;
		}

		public override bool IsAvailable()
		{
			return true;
		}
	}
}