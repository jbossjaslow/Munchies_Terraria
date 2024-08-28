﻿using Microsoft.Xna.Framework;
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

		private float CheckMarkOrRatioTextWidth = 0;

		UIPanel panel;
		UIText text;
		UIImage itemImage;
		UIText ratioText;
		UIImage checkMarkImage;

		public override void OnInitialize() {
			panel = new() {
				Width = StyleDimension.Fill,
				Height = StyleDimension.Fill
			};
			panel.SetPadding(0);
			Append(panel);

			itemImage = new(Consumable.Texture);
			itemImage.Left.Set(spacing, 0);
			itemImage.Width.Set(Consumable.Texture.Width(), 0f);
			itemImage.Height.Set(Consumable.Texture.Height(), 0f);
			itemImage.SetPadding(0);
			itemImage.VAlign = 0.5f;
			panel.Append(itemImage);

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

			AddCheckMarkOrCount();
			UpdateTextScale();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);
			if ((itemImage?.IsMouseHovering ?? false) && Consumable.UsingMissingTexture) {
				UICommon.TooltipMouseText(text: "Missing Texture");
			} else if (panel?.IsMouseHovering ?? false) {
				// Since this is added in DrawSelf, using .Value will grab current language without needing to reload mod
				UICommon.TooltipMouseText(text: Consumable.HoverText.Value); // Adds box behind hover text
			}
		}

		#region OnInit setup
		public void AddCheckMarkOrCount() {
			checkMarkImage?.Remove();
			ratioText?.Remove();

			if (Consumable.HasBeenConsumed) {
				// Consumed, add checkmark
				checkMarkImage = new(CheckMarkTexture);
				checkMarkImage.Left.Set(-spacing - CheckMarkTexture.Width(), 1f);
				checkMarkImage.Width.Set(CheckMarkTexture.Width(), 0f);
				checkMarkImage.Height.Set(CheckMarkTexture.Height(), 0f);
				checkMarkImage.SetPadding(0);
				checkMarkImage.VAlign = 0.5f;
				checkMarkImage.Color = Color.SpringGreen;

				panel.Append(checkMarkImage);
				CheckMarkOrRatioTextWidth = CheckMarkTexture.Width();

				return;
			} else if (Consumable.IsMultiUse) {
				// Not consumed and total count is greater than 1, show the ratio
				ratioText = new(text: $"{Consumable.CurrentCount()}/{Consumable.TotalCount()}") {
					TextColor = Color.SpringGreen,
					ShadowColor = Color.Black,
					IsWrapped = false,
					WrappedTextBottomPadding = 0f,
					VAlign = 0.5f,
				};
				ratioText.SetPadding(0);

				panel.Append(ratioText);
				CheckMarkOrRatioTextWidth = ratioText.GetInnerDimensions().Width;
				ratioText.Left.Set(-spacing - CheckMarkOrRatioTextWidth, 1f);

				return;
			} else {
				// At this point, total count must be 1, and if HasBeenConsumed (current == total) is false, then it's a 1x consumable and we should show nothing
				CheckMarkOrRatioTextWidth = 0;
				return;
			}
		}

		public void UpdateTextScale() {
			// If text is too long to fit, scale it down --> DynamicallyScaleDownToWidth doesn't appear to do anything on its own, at least how I have everything setup
			CalculatedStyle textDimensions = text.GetInnerDimensions();
			float restOfPanelWidths;
			if (CheckMarkOrRatioTextWidth == 0) {
				restOfPanelWidths = imageAssetMaxWidth + (spacing * 3); // 2 elements means 3 spacers
			} else {
				restOfPanelWidths = CheckMarkOrRatioTextWidth + imageAssetMaxWidth + (spacing * 4); // 3 elements means 4 spacers
			}
			float maxTextWidth = panel.GetDimensions().Width - restOfPanelWidths;
			if (textDimensions.Width > maxTextWidth) {
				float newScale = maxTextWidth / textDimensions.Width;
				text.SetText(text: Consumable.DisplayText, textScale: newScale, large: false);
			}
		}
		#endregion

		#region Private helpers
		private Color DisplayTextColor => Consumable.Type switch {
			ConsumableType.multiUse => Color.SkyBlue,
			ConsumableType.player_normal => Color.White,
			ConsumableType.player_expert => Main.expertMode ? Color.Orange : Color.Gray,
			ConsumableType.world => new Color(r: 242, g: 111, b: 238),
			_ => throw new System.NotImplementedException(),
		};
		#endregion
	}
}
