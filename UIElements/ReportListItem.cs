using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Munchies.Models;
using ReLogic.Content;
using Steamworks;
using System;
using System.Drawing.Drawing2D;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Munchies.UIElements {
	public class ReportListItem: UIElement {
		public readonly VanillaConsumable Consumable;

		// Maximum width of any of the images
		private readonly float imageAssetMaxWidth = 36;
		private readonly float spacing = 10;
		public static Asset<Texture2D> CheckMarkTexture;

		UIPanel panel;
		UIText text;

		public ReportListItem(VanillaConsumable consumable): base() {
			Consumable = consumable;
		}

		public override void OnInitialize() {
			panel = new();
			panel.Width.Pixels = GetDimensions().Width;
			panel.Height = StyleDimension.Fill;
			panel.SetPadding(0);
			Append(panel);

			string consumableTexturePath = "Terraria/Images/Item_" + Consumable.AssetPath;
			Asset<Texture2D> consumableTexture = ModContent.Request<Texture2D>(consumableTexturePath);
			UIImage itemImage = new(consumableTexture);
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
			if (Consumable.HasBeenConsumed) {
				panel.Append(checkMarkImage);
			}

			text = new(text: Consumable.DisplayText) {
				TextColor = Consumable.DisplayTextColor,
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
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);
			if (panel.IsMouseHovering) {
				Main.hoverItemName = Consumable.HoverText;
			}
			
			//if (VanillaConsumable.Type == ConsumableType.player_expert && !Main.expertMode) {
			//	CalculatedStyle pDim = panel.GetDimensions();
			//	Terraria.Utils.DrawRectangle(
			//		sb: spriteBatch,
			//		start: new(x: pDim.X, y: (pDim.Height / 2) - 3),
			//		end: new(x: pDim.X + pDim.Width, y: (pDim.Height / 2) + 3),
			//		colorStart: Color.Red,
			//		colorEnd: Color.Green,
			//		width: 50
			//	);
			//}
		}
	}
}
