using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Munchies.Models.Enums;
using Munchies.Utilities;
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
		public string Difficulty;
		public Func<bool> Available;

		public int ID;
		public bool UsingMissingTexture;
		public LocalizedText DifficultyText = LocalizedText.Empty;

		#region Init
		public Consumable(ModItem modItem, object CategoryOrCustomColor, Func<int> currentCount, Func<int> totalCount, string difficulty, Func<bool> available = null, LocalizedText extraTooltip = null) {
			DisplayText = modItem.DisplayName;
			HoverText = modItem.Tooltip;
			CurrentCount = currentCount;
			TotalCount = totalCount;
			SetTexture(modItem.Texture);
			ID = modItem.Type;
			Difficulty = difficulty;
			Available = available;

			if (extraTooltip != null) HoverText = Munchies.ConcatenateNewline.WithFormatArgs(HoverText, extraTooltip);

			if (CategoryOrCustomColor is Color color) CustomTextColor = color;
			else if (CategoryOrCustomColor is string categoryName) Type = GetModdedType(categoryName);

			if (Type != null) HoverText = Munchies.ConcatenateNewline.WithFormatArgs(HoverText, EnumUtil.GetEnumText(Type.Value));

			SetDifficultyText(difficulty);
		}

		public Consumable(int vanillaItemId, ConsumableType type, Func<int> currentCount, Func<int> totalCount, string difficulty, Func<bool> available = null, LocalizedText extraTooltip = null) {
			DisplayText = Lang.GetItemName(vanillaItemId);
			HoverText = Language.GetText($"ItemTooltip.{ItemID.Search.GetName(vanillaItemId)}");
			CurrentCount = currentCount;
			TotalCount = totalCount;
			SetTexture(vanillaItemId);
			Type = type;
			ID = vanillaItemId;
			Difficulty = difficulty;
			Available = available;

			if (extraTooltip != null) HoverText = Munchies.ConcatenateNewline.WithFormatArgs(HoverText, extraTooltip);

			HoverText = Munchies.ConcatenateNewline.WithFormatArgs(HoverText, EnumUtil.GetEnumText(Type.Value));

			SetDifficultyText(difficulty);
		}

		//Helper since we have access to Difficulty enum
		public Consumable(int vanillaItemId, ConsumableType type, Func<int> currentCount, Func<int> totalCount, Difficulty difficulty, Func<bool> available = null, LocalizedText extraTooltip = null) :
			this(vanillaItemId, type, currentCount, totalCount, difficulty.ToString(), available, extraTooltip) { }
		#endregion

		#region Public helpers
		public bool HasBeenConsumed => CurrentCount() == TotalCount();
		public bool IsMultiUse => TotalCount() > 1;
		#endregion

		#region Private helpers
		private void SetDifficultyText(string difficulty) {
			if (difficulty != Enums.Difficulty.classic.ToString()) {
				var diff = Munchies.GetDifficulty(difficulty);
				DifficultyText = diff != null ? EnumUtil.GetEnumText(diff.Value) : LocalizedText.Empty;
			}
		}

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
				return ConsumableType.player;
			}
		}
		#endregion
	}
}
