using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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
			SetTextureAndDimensions(modTabTexturePath);
		}

		private void SetTextureAndDimensions(string texturePath) {
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
