using System;
using System.Collections.Generic;
using LivingNPCs.Village.OrderSystem.Order;

namespace LivingNPCs.Village.OrderSystem
{
	public class OrderCollection
	{
		public Dictionary<Type, List<Order.Order>> Collection;

		public OrderCollection()
		{
			Collection = new Dictionary<Type, List<Order.Order>>();
		}

		public void AddOrder(Order.Order order)
		{
#if DEBUG
			LivingNPCs.Writer.WriteLine("new order : " + order);
#endif
			if (!order.CheckValidity()) return;
			Type orderType = order.GetType();
			if (!Collection.TryGetValue(orderType, out List<Order.Order> orders))
			{
				orders = new List<Order.Order>();
				Collection[orderType] = orders;
			}

			orders.Add(order);
			if (order is DependantOrder dependentOrder)
			{
				List<Order.Order> otherOrders = dependentOrder.OtherOrders;
				if (otherOrders != null)
					foreach (Order.Order otherOrder in otherOrders)
						AddOrder(otherOrder);
			}
		}

		public TOrder GetOrder<TOrder>(Func<TOrder, bool> condition = null, int index = 0)
			where TOrder : Order.Order
		{
			if (Collection.TryGetValue(typeof(TOrder), out List<Order.Order> orders))
			{
				if (condition != null)
					for (; index < orders.Count; index++)
						if (condition((TOrder) orders[index]))
							break;

				if (index >= orders.Count)
					return null;

				Order.Order order = orders[index];
				orders.RemoveAt(index);
				if (!order.CheckValidity())
					return GetOrder(condition, index + 1);
				else
					return (TOrder) order;
			}

			return null;
		}
	}
}