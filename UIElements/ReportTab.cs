using Microsoft.Xna.Framework.Graphics;
using Munchies.Models;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using ReLogic.Content;
using Terraria.ModLoader;
using System;
using Terraria.Audio;
using Terraria.ID;

namespace Munchies.UIElements {
	public class ReportTab(IConsumableMod mod, int index, Action<IConsumableMod> OnSelectTab) : UIElement() {

		public readonly IConsumableMod mod = mod;
		//public IConsumableMod selectedMod = selectedMod;
		public int index = index;
		public UIPanel panel = new();
		public UIImage image;

		public override void OnInitialize() {
			panel.Width = StyleDimension.Fill;
			panel.Height = StyleDimension.Fill; // use outside height
			panel.SetPadding(0);
			panel.OnLeftClick += new MouseEvent(OnTabClick);
			Append(panel);

			Asset<Texture2D> texture = ModContent.Request<Texture2D>(mod.ModTabTexturePath);
			image = new(texture) {
				VAlign = 0.5f,
				HAlign = 0.5f,
				ScaleToFit = true,
			};
			image.SetPadding(0f);
			image.Width.Set(-10f, 1f);
			image.Height.Set(-10f, 1f);
			image.OnMouseOver += new MouseEvent(MyMouseOver);
			panel.Append(image);
		}

		private void OnTabClick(UIMouseEvent evt, UIElement listeningElement) {
			OnSelectTab(mod);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);
			if (panel.IsMouseHovering) {
				Main.hoverItemName = mod.ModTabName;
			}
		}

		private void MyMouseOver(UIMouseEvent evt, UIElement listeningElement) {
			SoundEngine.PlaySound(SoundID.MenuTick);
			// 37 anvil
			// 113 minion spawn
			// npc_hit_5 sparkle
			// tinks
		}
	}
}
