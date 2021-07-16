using System;
using System.Collections.Generic;
using System.Linq;
using LivingNPCs.Jobs;
using LivingNPCs.Village.OrderSystem.Order;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace LivingNPCs.NPCs
{
	public abstract class Villager : JobCollection
	{
		public Dictionary<int, int> Inventory => EasierNPC.Inventory;
		public int Type => EasierNPC.Type;

		public virtual void SetDefaults(NPC npc)
		{
			EasierNPC = new EasierNPC(npc);
		}

		public virtual bool AI()
		{
			UpdateTime(Time.Now());
			if (ActiveJob?.CurrentOrder is DependantOrder dependantOrder)
			{
				List<Order> orders = dependantOrder.Refresh();
				if (orders != null)
					foreach (Order order in orders)
						EasierNPC.OrderCollection.AddOrder(order);
			}

			if (!(ActiveJob?.CurrentOrder?.CheckValidity() is true) || !ActiveJob.CurrentOrder.IsAvailable())
				FindNewJob();
			
			EasierNPC.GatherItems(3);
			return ActiveJob?.AI(EasierNPC) ?? true;
		}

		public virtual void UpdateTime(Time now)
		{
		}

		public virtual void DrawWeapon(SpriteBatch spriteBatch, Color drawColor)
		{
		}

		public void Save(TagCompound tagCompound)
		{
			TagCompound inventory = new TagCompound();
			foreach (KeyValuePair<int, int> item in Inventory)
			{
				int stack = item.Value;
				if (item.Value != 0)
				{
					string itemName = ItemID.GetUniqueKey(item.Key);
					inventory.Add(itemName, stack);
				}
			}

			if (inventory.Count != 0)
				tagCompound.Add(Type.ToString(), inventory);
		}

		public void Load(TagCompound tagCompound)
		{
			string npcType = Type.ToString();
			if (!tagCompound.ContainsKey(npcType))
				return;
			TagCompound inventory = tagCompound.GetCompound(npcType);
			if (inventory != null)
				foreach (KeyValuePair<string, object> item in inventory)
				{
					int stack = (int) item.Value;
					int itemId = ItemID.TypeFromUniqueKey(item.Key);
					Inventory.Add(itemId, stack);
				}
		}

		public void FindNewJob()
		{
			ActiveJob = null;
			foreach (KeyValuePair<Type, Job> keyValuePair in Jobs)
			{
				Job job = keyValuePair.Value;
				if (job.CurrentOrder?.CheckValidity() is true && job.CurrentOrder.IsAvailable())
				{
					SetJobToActive(keyValuePair.Key);
					return;
				}
			}

			foreach (KeyValuePair<Type, Job> keyValuePair in Jobs)
			{
				Job job = keyValuePair.Value;
				if (!(job.CurrentOrder?.Completed is false))
				{
					job.CurrentOrder = job.NewOrder(EasierNPC);
					if (job.CurrentOrder?.CheckValidity() is true && job.CurrentOrder.IsAvailable())
					{
						SetJobToActive(keyValuePair.Key);
						return;
					}
				}
			}
		}
	}
}