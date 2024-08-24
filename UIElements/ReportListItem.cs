using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Munchies.Models;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Munchies.UIElements {
	public class ReportListItem(Consumable consumable) : UIElement() {
		public readonly Consumable Consumable = consumable;

		// Maximum width of any of the images
		private readonly float imageAssetMaxWidth = 36;
		private readonly float spacing = 10;
		public static Asset<Texture2D> CheckMarkTexture;

		UIPanel panel;
		UIText text;

		public override void OnInitialize() {
			panel = new() {
				Width = StyleDimension.Fill,
				Height = StyleDimension.Fill
			};
			panel.SetPadding(0);
			Append(panel);

			UIImage itemImage = new(Consumable.Texture);
			itemImage.Left.Set(spacing, 0);
			itemImage.Width.Set(Consumable.AssetDimensions.X, 0f);
			itemImage.Height.Set(Consumable.AssetDimensions.Y, 0f);
			itemImage.SetPadding(0);
			itemImage.VAlign = 0.5f;
			panel.Append(itemImage);

			UIImage checkMarkImage = new(CheckMarkTexture);
			checkMarkImage.Left.Set(-spacing - 19, 1f);
			checkMarkImage.Width.Set(19f, 0f);
			checkMarkImage.Height.Set(20f, 0f);
			checkMarkImage.SetPadding(0);
			checkMarkImage.VAlign = 0.5f;
			checkMarkImage.Color = Color.SpringGreen;
			if (Consumable.HasBeenConsumed()) {
				panel.Append(checkMarkImage);
			}

			text = new(text: Consumable.DisplayText) {
				TextColor = Consumable.CustomTextColor ?? DisplayTextColor,
				ShadowColor = Color.Black,
				IsWrapped = false,
				WrappedTextBottomPadding = 0f,
				HAlign = 0f,
				VAlign = 0.5f,
				DynamicallyScaleDownToWidth = true,
			};
			text.Left.Set(imageAssetMaxWidth + (spacing * 2), 0f);
			text.SetPadding(0);

			panel.Append(text);

			// If text is too long to fit, scale it down --> DynamicallyScaleDownToWidth doesn't appear to do anything on its own, at least how I have everything setup
			CalculatedStyle textDimensions = text.GetInnerDimensions();
			float restOfPanelWidths = (checkMarkImage.Width.Pixels + spacing + 19) + imageAssetMaxWidth + (spacing * 2);
			float maxTextWidth = panel.GetDimensions().Width - restOfPanelWidths;
			if (textDimensions.Width > maxTextWidth) {
				float newScale = maxTextWidth / textDimensions.Width;
				text.SetText(text: Consumable.DisplayText, textScale: newScale, large: false);
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);
			if (panel?.IsMouseHovering ?? false) {
				//Main.hoverItemName = HoverText();
				UICommon.TooltipMouseText(text: HoverText()); // Adds box behind hover text
			}
		}

		private Color DisplayTextColor => Consumable.Type switch {
			ConsumableType.player_normal => Color.White,
			ConsumableType.player_expert => Main.expertMode ? Color.Orange : Color.Gray,
			ConsumableType.world => new Color(r: 242, g: 111, b: 238),
			_ => throw new System.NotImplementedException(),
		};

		private string HoverText() {
			return Consumable.UsingMissingTexture ? "[MISSING TEXTURE] " + Consumable.HoverText : Consumable.HoverText;
		}
	}
}
