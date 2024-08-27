using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace Munchies.Models {
	public class ConsumableMod {
		public string ModTabName;
		//public string ModTabTexturePath = modTabTexturePath;
		public Asset<Texture2D> ModTabTexture;
		public bool UsingMissingTexture;

		public ConsumableMod(string modTabName, string modTabTexturePath) {
			ModTabName = modTabName;
			//ModTabTexture = ModContent.Request<Texture2D>(modTabTexturePath);
			SetTextureForVanilla(modTabTexturePath);
		}

		public ConsumableMod(Mod mod) {
			ModTabName = mod.DisplayNameClean;
			SetTextureForExternalMod(mod);
		}

		private void SetTextureForExternalMod(Mod mod) {
			if (mod.HasAsset("icon_small")) {
				Asset<Texture2D> texture = mod.Assets.Request<Texture2D>("icon_small");
				if (texture.Size() == new Vector2(30)) {
					ModTabTexture = texture;
					UsingMissingTexture = false;
				} else {
					ModTabTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/UI_quickicon1");
					UsingMissingTexture = true;
				}
			} else {
				ModTabTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/UI_quickicon1");
				UsingMissingTexture = true;
			}
		}

		private void SetTextureForVanilla(string texturePath) {
			if (ModContent.HasAsset(texturePath)) {
				ModTabTexture = ModContent.Request<Texture2D>(texturePath);
				UsingMissingTexture = false;
			} else {
				ModTabTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/UI_quickicon1");
				UsingMissingTexture = true;
			}
		}
	}
}
