using System.Collections.Generic;
using LivingNPCs.Info.TileInfo;
using LivingNPCs.NPCs;
using Microsoft.Xna.Framework;
using Terraria;
#if DEBUG
using Terraria.ID;

#endif

namespace LivingNPCs.Village.OrderSystem.Order
{
	public class BuildingOrder : DependantOrder
	{
		public EasierNPC Builder;
		public Point Location;
		public TileInfo TileInfo;

		public BuildingOrder(EasierNPC builder, Point location, TileInfo tileInfo)
		{
			Builder = builder;
			Location = location;
			TileInfo = tileInfo;
		}

		public override void OnCompleted()
		{
			Builder.Village.AddTile(TileInfo.TileId);
#if DEBUG
			base.OnCompleted();
#endif
		}

		public override List<Order> GenerateOtherOrders()
		{
			List<Order> orders = new List<Order>
			{
				new ItemOrder(Builder, TileInfo.ItemInfo)
			};
			if (!TileInfo.IsWall)
				orders.Add(new CleaningOrder(Location, TileInfo));
			return orders;
		}

		public override List<Order> Refresh()
		{
			if (TileInfo.IsWall)
				return null;
			CleaningOrder cleaningOrder = new CleaningOrder(Location, TileInfo);
			if (cleaningOrder.CheckValidity() != OtherOrders[0].CheckValidity())
			{
				OtherOrders[0] = cleaningOrder;
				return new List<Order> {cleaningOrder};
			}
			else
			{
				return null;
			}
		}

		public override bool IsValid()
		{
			Tile tile = Framing.GetTileSafely(Location);
			if (TileInfo.IsWall)
				return tile.wall != TileInfo.TileId;
			else
				return !tile.active() || tile.type != TileInfo.TileId;
		}

#if DEBUG
		public override string ToString()
		{
			return base.ToString() + "at x:" + Location.X + ", y:" + Location.Y + ", tile: " +
			       ItemID.GetUniqueKey(TileInfo.ItemInfo.ItemId);
		}
#endif
	}
}