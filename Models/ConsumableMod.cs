using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Munchies.UIElements;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace Munchies.Models {
	public class ConsumableMod {
		public string ModTabName;
		private Mod mod;
		private string texturePath;
		public bool UsingMissingTexture = false;
		private bool VanillaMod;

		public ConsumableMod(string modTabName, string modTabTexturePath) {
			ModTabName = modTabName;
			mod = null;
			texturePath = modTabTexturePath;
			VanillaMod = true;
		}

		public ConsumableMod(Mod mod) {
			ModTabName = mod.DisplayNameClean;
			this.mod = mod;
			texturePath = null;
			VanillaMod = false;
		}

		public Asset<Texture2D> GetTexture() {
			if (VanillaMod && texturePath != null && ModContent.HasAsset(texturePath)) {
				return ModContent.Request<Texture2D>(texturePath);
			} else if (mod?.HasAsset("icon_small") ?? false) {
				Asset<Texture2D> texture = mod.Assets.Request<Texture2D>("icon_small", AssetRequestMode.ImmediateLoad); //Dimensions checked for parity with tml so it needs Immediate
				if (texture.Size() == new Vector2(30)) {
					return texture;
				} else {
					UsingMissingTexture = true;
					return Munchies.UnknownTexture;
				}
			} else {
				UsingMissingTexture = true;
				return Munchies.UnknownTexture;
			}
		}

		public bool SameAs(ConsumableMod other) => mod == other.mod;
	}
}
