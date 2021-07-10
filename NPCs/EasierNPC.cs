using System;
using System.Collections.Generic;
using System.Linq;
using LivingNPCs.TileTool;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace LivingNPCs.NPCs
{
	public class EasierNPC
	{
		public const int TileToSquaredPixel = 16 * 16;
		public Dictionary<int, int> Inventory;

		public NPC NPC;
		public (Point location, int reach) Objective;
		
		public ToolSet ToolSet;

		public EasierNPC(NPC npc)
		{
			NPC = npc;
			ClearObjective();
			Inventory = new Dictionary<int, int>();
		}

		public int LeftX => (int) (NPC.position.X / 16);
		public int UpperY => (int) (NPC.position.Y / 16);
		public int RightX => (int) Math.Ceiling((NPC.position.X + NPC.width) / 16);
		public int LowerY => (int) Math.Ceiling((NPC.position.Y + NPC.height) / 16);
		public int Type => NPC.type;

		public bool Stop()
		{
			if (NPC.velocity.Y == 0)
			{
				NPC.velocity.X = 0;
				return true;
			}
			else
			{
				return false;
			}
		}

		public void SetObjective(Point point, int reach = 0)
		{
			Objective = (point, reach);
			GoingToObjective();
		}

		public bool GoingToObjective()
		{
			if (Objective.location.X < LeftX - Objective.reach - 1)
			{
				NPC.direction = -1;
				return true;
			}
			else if (Objective.location.X > RightX + Objective.reach)
			{
				NPC.direction = 1;
				return true;
			}
			else
			{
				return false;
			}
		}

		public void Walk(float maxSpeed = 1.5f)
		{
			if (NPC.direction == -1)
			{
				if (NPC.velocity.X <= -(maxSpeed - 0.1f))
					NPC.velocity.X = -maxSpeed;
				else
					NPC.velocity.X -= 0.1f;

				if (Framing.GetTileSafely(LeftX - 1, UpperY).type == TileID.ClosedDoor)
					WorldGen.OpenDoor(LeftX - 1, UpperY, NPC.direction);
				if (Framing.GetTileSafely(RightX + 1, UpperY).type == TileID.OpenDoor)
					WorldGen.CloseDoor(RightX + 1, UpperY);

				if (Collision.SolidTiles(LeftX - 1, LeftX - 1, UpperY, LowerY - 2))
					Jump();
				else
					Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed,
						ref NPC.gfxOffY);
			}
			else
			{
				if (NPC.velocity.X >= maxSpeed - 0.1f)
					NPC.velocity.X = maxSpeed;
				else
					NPC.velocity.X += 0.1f;

				if (Framing.GetTileSafely(RightX + 1, UpperY).type == TileID.ClosedDoor)
					WorldGen.OpenDoor(RightX + 1, UpperY, NPC.direction);
				if (Framing.GetTileSafely(LeftX - 1, UpperY).type == TileID.OpenDoor)
					WorldGen.CloseDoor(LeftX - 1, UpperY);
				
				if (Collision.SolidTiles(RightX + 1, RightX + 1, UpperY, LowerY - 2))
					Jump();
				else
					Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed,
						ref NPC.gfxOffY);
			}
		}

		public void Jump()
		{
			if (NPC.velocity.Y == 0)
				NPC.velocity.Y = -5;
		}

		public void ClearObjective()
		{
			Objective = (Point.Zero, -1);
		}

		public bool ReachedObjective()
		{
			return !GoingToObjective();
		}

		public bool OnObjective()
		{
			return LeftX <= Objective.location.X && Objective.location.X <= RightX &&
			       UpperY <= Objective.location.Y && Objective.location.Y <= LowerY;
		}

		public bool NoObjective()
		{
			return Objective.location == Point.Zero;
		}

		public (bool pickedUpItems, bool remainingItems) GatherItems(int range)
		{
			int squaredRange = range * range * TileToSquaredPixel;
			//I square it here to call LengthSquared and avoid Length's square root

			bool pickedUpItems = false;
			bool remainingItems = false;

			foreach (Item item in Main.item.Where(item => item.active))
			{
				Vector2 vector = NPC.Center - item.Center;
				float squaredDistance = vector.LengthSquared();
				if (squaredDistance < 1 * TileToSquaredPixel)
				{
					item.active = false;
					Inventory.TryGetValue(item.type, out int amount);
					Inventory[item.type] = amount + item.stack;
					pickedUpItems = true;
				}
				else if (squaredDistance < squaredRange)
				{
					item.velocity = vector / (float) Math.Sqrt(squaredDistance) * 3;
					item.beingGrabbed = true;
					remainingItems = true;
				}
			}

			return (pickedUpItems, remainingItems);
		}

		public void DumpAllInChest(Chest chest, Dictionary<int, int> ItemsToKeep = null)
		{
			foreach (Item item in chest.item)
			{
				int amount = 0;
				int type = 0;
				if (item.type == ItemID.None)
					foreach (KeyValuePair<int, int> pair in Inventory)
					{
						if (pair.Value != 0 && !(ItemsToKeep?.ContainsKey(pair.Key) is true &&
						                         ItemsToKeep[pair.Key] >= pair.Value))
						{
							type = pair.Key;
							amount = pair.Value;
							break;
						}
					}
				else if (Inventory.TryGetValue(item.type, out amount))
					type = item.type;

				if (amount == 0)
					continue;

				int savedAmount = 0;
				if (ItemsToKeep?.TryGetValue(item.type, out savedAmount) is true && savedAmount >= amount)
					continue;

				if (item.type == ItemID.None)
					item.SetDefaults(type);
				int amountGiven = Math.Min(amount - savedAmount, item.maxStack - item.stack);
				item.stack += amountGiven;
				Inventory[type] -= amountGiven;
			}
		}
	}
}