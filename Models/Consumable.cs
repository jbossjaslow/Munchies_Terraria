using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Munchies.Models {
	public class Consumable {
		public LocalizedText DisplayText;
		public LocalizedText HoverText;
		public Asset<Texture2D> Texture;
		public ConsumableType? Type;
		public Color? CustomTextColor;
		public Func<int> CurrentCount; // How many of the item have been consumed; for bools (aka single use consumables), this is either 0 or 1
		public Func<int> TotalCount; // Total number of items that can be consumed; for bools, this is 1

		public int ID;
		public bool UsingMissingTexture;

		#region Init
		public Consumable(ModItem modItem, object CategoryOrCustomColor, Func<int> currentCount, Func<int> totalCount) {
			DisplayText = modItem.DisplayName;
			HoverText = modItem.Tooltip;
			CurrentCount = currentCount;
			TotalCount = totalCount;
			SetTexture(modItem.Texture);
			ID = modItem.Type;

			if (CategoryOrCustomColor is Color color) CustomTextColor = color;
			else if (CategoryOrCustomColor is string categoryName) Type = GetModdedType(categoryName);
		}

		public Consumable(int vanillaItemId, ConsumableType type, Func<int> currentCount, Func<int> totalCount) {
			DisplayText = Lang.GetItemName(vanillaItemId);
			HoverText = Language.GetText($"ItemTooltip.{ItemID.Search.GetName(vanillaItemId)}");
			CurrentCount = currentCount;
			TotalCount = totalCount;
			SetTexture(vanillaItemId);
			Type = type;
			ID = vanillaItemId;
		}

		public Consumable(int vanillaItemId) {
			DisplayText = Lang.GetItemName(vanillaItemId);
			HoverText = Language.GetText($"ItemTooltip.{ItemID.Search.GetName(vanillaItemId)}");
			CurrentCount = () => VanillaItemCurrentCount(vanillaItemId);
			TotalCount = () => VanillaItemTotalCount(vanillaItemId);
			SetTexture(vanillaItemId);
			Type = GetVanillaType(vanillaItemId);
			ID = vanillaItemId;
		}
		#endregion

		#region Public helpers
		public bool HasBeenConsumed => CurrentCount() == TotalCount();
		public bool IsMultiUse => TotalCount() > 1;
		#endregion

		#region Private helpers
		private void SetTexture(string texturePath) {
			if (ModContent.HasAsset(texturePath)) {
				Texture = ModContent.Request<Texture2D>(texturePath);
				UsingMissingTexture = false;
			} else {
				Texture = ModContent.Request<Texture2D>("Terraria/Images/UI/UI_quickicon1");
				UsingMissingTexture = true;
			}
		}

		private void SetTexture(int vanillaItemId) {
			Texture = ModContent.Request<Texture2D>($"Terraria/Images/Item_{vanillaItemId}");
			UsingMissingTexture = false;
		}

		private static ConsumableType GetModdedType(string type) {
			if (Enum.TryParse(type, out ConsumableType consumableType)) {
				return consumableType;
			} else {
				return ConsumableType.player_normal;
			}
		}

		private static ConsumableType GetVanillaType(int id) {
			return id switch {
				// multi-use
				ItemID.LifeCrystal => ConsumableType.multiUse,
				ItemID.LifeFruit => ConsumableType.multiUse,
				ItemID.ManaCrystal => ConsumableType.multiUse,

				// normal
				ItemID.ArtisanLoaf => ConsumableType.player_normal,
				ItemID.TorchGodsFavor => ConsumableType.player_normal,
				ItemID.AegisCrystal => ConsumableType.player_normal,
				ItemID.AegisFruit => ConsumableType.player_normal,
				ItemID.ArcaneCrystal => ConsumableType.player_normal,
				ItemID.Ambrosia => ConsumableType.player_normal,
				ItemID.GummyWorm => ConsumableType.player_normal,
				ItemID.GalaxyPearl => ConsumableType.player_normal,

				// expert
				ItemID.DemonHeart => ConsumableType.player_expert,
				ItemID.MinecartPowerup => ConsumableType.player_expert,

				// world
				ItemID.CombatBook => ConsumableType.world,
				ItemID.CombatBookVolumeTwo => ConsumableType.world,
				ItemID.PeddlersSatchel => ConsumableType.world,
				_ => throw new NotImplementedException(),
			};
		}

		private static int VanillaItemCurrentCount(int id) {
			return id switch {
				// multi-use
				ItemID.LifeCrystal => Main.LocalPlayer.ConsumedLifeCrystals,
				ItemID.LifeFruit => Main.LocalPlayer.ConsumedLifeFruit,
				ItemID.ManaCrystal => Main.LocalPlayer.ConsumedManaCrystals,

				// normal
				ItemID.ArtisanLoaf => Main.LocalPlayer.ateArtisanBread.ToInt(),
				ItemID.TorchGodsFavor => Main.LocalPlayer.unlockedBiomeTorches.ToInt(),
				ItemID.AegisCrystal => Main.LocalPlayer.usedAegisCrystal.ToInt(),
				ItemID.AegisFruit => Main.LocalPlayer.usedAegisFruit.ToInt(),
				ItemID.ArcaneCrystal => Main.LocalPlayer.usedArcaneCrystal.ToInt(),
				ItemID.Ambrosia => Main.LocalPlayer.usedAmbrosia.ToInt(),
				ItemID.GummyWorm => Main.LocalPlayer.usedGummyWorm.ToInt(),
				ItemID.GalaxyPearl => Main.LocalPlayer.usedGalaxyPearl.ToInt(),

				// expert
				ItemID.DemonHeart => Main.LocalPlayer.CanDemonHeartAccessoryBeShown().ToInt(),
				ItemID.MinecartPowerup => Main.LocalPlayer.unlockedSuperCart.ToInt(),

				// world
				ItemID.CombatBook => NPC.combatBookWasUsed.ToInt(),
				ItemID.CombatBookVolumeTwo => NPC.combatBookVolumeTwoWasUsed.ToInt(),
				ItemID.PeddlersSatchel => NPC.peddlersSatchelWasUsed.ToInt(),
				_ => throw new NotImplementedException(),
			};
		}

		private static int VanillaItemTotalCount(int id) {
			return id switch {
				// multi-use
				ItemID.LifeCrystal => 15,
				ItemID.LifeFruit => 20,
				ItemID.ManaCrystal => 9,

				// normal
				ItemID.ArtisanLoaf => 1,
				ItemID.TorchGodsFavor => 1,
				ItemID.AegisCrystal => 1,
				ItemID.AegisFruit => 1,
				ItemID.ArcaneCrystal => 1,
				ItemID.Ambrosia => 1,
				ItemID.GummyWorm => 1,
				ItemID.GalaxyPearl => 1,

				// expert
				ItemID.DemonHeart => 1, // 3335
				ItemID.MinecartPowerup => 1,

				// world
				ItemID.CombatBook => 1,
				ItemID.CombatBookVolumeTwo => 1,
				ItemID.PeddlersSatchel => 1,
				_ => throw new NotImplementedException(),
			};
		}
		#endregion
	}
}
