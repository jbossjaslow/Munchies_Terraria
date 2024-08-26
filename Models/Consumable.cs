using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Munchies.Models {
	public class Consumable {
		public string DisplayText;
		//public string TexturePath;
		public Asset<Texture2D> Texture;
		//public (float X, float Y) AssetDimensions;
		public string HoverText;
		public ConsumableType? Type;
		public Color? CustomTextColor;
		public Func<int> CurrentCount; // How many of the item have been consumed; for bools (aka single use consumables), this is either 0 or 1
		public Func<int> TotalCount; // Total number of items that can be consumed; for bools, this is 1

		public bool UsingMissingTexture;

		#region Init
		public Consumable(ModItem modItem, object CategoryOrCustomColor, Func<int> currentCount, Func<int> totalCount) {
			DisplayText = modItem.DisplayName.ToString();
			HoverText = modItem.Tooltip.ToString();
			CurrentCount = currentCount;
			TotalCount = totalCount;
			SetTextureAndDimensions(modItem.Texture);

			if (CategoryOrCustomColor is Color color) CustomTextColor = color;
			else if (CategoryOrCustomColor is string categoryName) Type = GetType(categoryName);
		}

		public Consumable(int vanillaItemId, ConsumableType type, Func<int> currentCount, Func<int> totalCount) {
			DisplayText = Lang.GetItemName(vanillaItemId).Value;
			HoverText = Language.GetText($"ItemTooltip.{ItemID.Search.GetName(vanillaItemId)}").Value;
			CurrentCount = currentCount;
			TotalCount = totalCount;
			SetTextureAndDimensions(vanillaItemId);
			Type = type;
		}
		#endregion

		#region Public helpers
		public bool HasBeenConsumed => CurrentCount() == TotalCount();
		public bool IsMultiUse => TotalCount() > 1;
		#endregion

		#region Private helpers
		private void SetTextureAndDimensions(string texturePath) {
			if (ModContent.HasAsset(texturePath)) {
				Texture = ModContent.Request<Texture2D>(texturePath);
				//AssetDimensions = (X: float.Parse(Texture.Width().ToString()), Y: float.Parse(Texture.Height().ToString()));
				UsingMissingTexture = false;
			} else {
				//AssetDimensions = (6, 16);
				Texture = ModContent.Request<Texture2D>("Terraria/Images/UI/UI_quickicon1");
				UsingMissingTexture = true;
			}
		}

		private void SetTextureAndDimensions(int vanillaItemId) {
			Texture = ModContent.Request<Texture2D>($"Terraria/Images/Item_{vanillaItemId}");
			//AssetDimensions = (X: float.Parse(Texture.Width().ToString()), Y: float.Parse(Texture.Height().ToString()));
			UsingMissingTexture = false;
		}

		private static ConsumableType GetType(string type) {
			if (Enum.TryParse(type, out ConsumableType consumableType)) {
				return consumableType;
			} else {
				return ConsumableType.player_normal;
			}
		}
		#endregion
	}
}
