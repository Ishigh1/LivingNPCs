using System.Collections.Generic;
using System.Linq;

namespace LivingNPCs.Village.OrderSystem.Order
{
	public abstract class DependantOrder : Order
	{
		private List<Order> _otherOrders;
		public List<Order> OtherOrders => _otherOrders ?? (_otherOrders = GenerateOtherOrders());

		public abstract List<Order> GenerateOtherOrders();

		public override bool IsAvailable()
		{
			return OtherOrders.All(otherOrder => !otherOrder.CheckValidity());
		}

		public abstract List<Order> Refresh();

#if DEBUG
		public override string ToString()
		{
			string returnValue = base.ToString() + "dependent: [";
			returnValue = OtherOrders.Aggregate(returnValue, (current, otherOrder) => current + (otherOrder + ","));
			return returnValue + "]";
		}
#endif
	}
}