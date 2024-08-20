using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace Munchies.Models {
	public class Consumable {
		public string DisplayText;
		//public string TexturePath;
		public Asset<Texture2D> Texture;
		public (float X, float Y) AssetDimensions;
		public string HoverText;
		public ConsumableType? Type;
		public Color? CustomTextColor;
		public Func<bool> HasBeenConsumed;

		public bool UsingMissingTexture;

		public Consumable(string displayText, string texturePath, (float X, float Y) assetDimensions, string hoverText, ConsumableType type, Func<bool> hasBeenConsumed) {
			DisplayText = displayText;
			//TexturePath = texturePath;
			//AssetDimensions = assetDimensions;
			HoverText = hoverText;
			Type = type;
			CustomTextColor = null;
			HasBeenConsumed = hasBeenConsumed;

			SetTextureAndDimensions(texturePath: texturePath, assetDimensions: assetDimensions);
		}

		public Consumable(string displayText, string texturePath, (float X, float Y) assetDimensions, string hoverText, Color customHoverColor, Func<bool> hasBeenConsumed) {
			DisplayText = displayText;
			//TexturePath = texturePath;
			//AssetDimensions = assetDimensions;
			HoverText = hoverText;
			Type = null;
			CustomTextColor = customHoverColor;
			HasBeenConsumed = hasBeenConsumed;

			SetTextureAndDimensions(texturePath: texturePath, assetDimensions: assetDimensions);
		}

		public Consumable(VanillaConsumable vc) {
			DisplayText = vc.DisplayText;
			//TexturePath = vc.TexturePath;
			//AssetDimensions = vc.AssetDimensions;
			HoverText = vc.HoverText;
			Type = vc.Type;
			CustomTextColor = null;
			HasBeenConsumed = vc.HasBeenConsumed;

			SetTextureAndDimensions(texturePath: vc.TexturePath, assetDimensions: vc.AssetDimensions);
		}

		private void SetTextureAndDimensions(string texturePath, (float X, float Y) assetDimensions) {
			if (ModContent.HasAsset(texturePath)) {
				Texture = ModContent.Request<Texture2D>(texturePath);
				AssetDimensions = assetDimensions;
				UsingMissingTexture = false;
			} else {
				AssetDimensions = (6, 16);
				Texture = ModContent.Request<Texture2D>("Terraria/Images/UI/UI_quickicon1");
				UsingMissingTexture = true;
			}
		}

		//private Asset<Texture2D> GetTexture(string path) {
		//	try {
		//		return ModContent.Request<Texture2D>(path);
		//	} catch {
		//		UsingMissingTexture = true;
		//		//Munchies.instance.Logger.Error($"Error getting consumable texture: {e.StackTrace} {e.Message}");
		//		return ModContent.Request<Texture2D>("Terraria/Images/UI/UI_quickicon1");
		//	}
		//}
	}
}
