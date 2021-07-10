using System.Collections.Generic;
using LivingNPCs.Jobs;
using LivingNPCs.TileTool;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace LivingNPCs.NPCs
{
	public abstract class Villager : JobCollection
	{
		public EasierNPC NPC;

		public Dictionary<int, int> Inventory => NPC.Inventory;
		public int Type => NPC.Type;

		public virtual void SetDefaults(NPC npc)
		{
			NPC = new EasierNPC(npc);
		}

		public virtual bool AI()
		{
			UpdateTime(Time.Now());
			return ActiveJob.AI(NPC);
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
	}
}