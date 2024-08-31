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
		public Consumable(ModItem modItem, object CategoryOrCustomColor, Func<int> currentCount, Func<int> totalCount, LocalizedText extraTooltip) {
			DisplayText = modItem.DisplayName;
			HoverText = modItem.Tooltip;
			CurrentCount = currentCount;
			TotalCount = totalCount;
			SetTexture(modItem.Texture);
			ID = modItem.Type;

			if (extraTooltip != null) HoverText = Munchies.ConcatenateNewline.WithFormatArgs(HoverText, extraTooltip); 

			if (CategoryOrCustomColor is Color color) CustomTextColor = color;
			else if (CategoryOrCustomColor is string categoryName) Type = GetModdedType(categoryName);
		}

		public Consumable(int vanillaItemId, ConsumableType type, Func<int> currentCount, Func<int> totalCount, LocalizedText extraTooltip) {
			DisplayText = Lang.GetItemName(vanillaItemId);
			HoverText = Language.GetText($"ItemTooltip.{ItemID.Search.GetName(vanillaItemId)}");
			CurrentCount = currentCount;
			TotalCount = totalCount;
			SetTexture(vanillaItemId);
			Type = type;
			ID = vanillaItemId;

			if (extraTooltip != null) HoverText = Munchies.ConcatenateNewline.WithFormatArgs(HoverText, extraTooltip);
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
		#endregion
	}
}
