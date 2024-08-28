using Microsoft.Xna.Framework.Graphics;
using Munchies.Models;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader.UI;

namespace Munchies.UIElements {
	public class ReportTab(ConsumableMod mod, int index, Action<ConsumableMod> OnSelectTab) : UIElement() {

		public readonly ConsumableMod mod = mod;
		public int index = index;
		public UIPanel panel = new();
		public UIImage image;

		public override void OnInitialize() {
			panel.Width = StyleDimension.Fill;
			panel.Height = StyleDimension.Fill; // use outside height
			panel.SetPadding(0);
			panel.OnLeftClick += new MouseEvent(OnTabClick);
			Append(panel);

			image = new(mod.GetTexture()) {
				VAlign = 0.5f,
				HAlign = 0.5f,
			};
			image.SetPadding(0f);
			if (mod.UsingMissingTexture) {
				image.Width.Set(6f, 0.5f);
				image.Height.Set(16f, 0.5f);
				image.ScaleToFit = false;
			} else {
				image.Width.Set(-10f, 1f);
				image.Height.Set(-10f, 1f);
				image.ScaleToFit = true;
			}
			//image.OnMouseOver += new MouseEvent(MyMouseOver);
			panel.Append(image);
		}

		private void OnTabClick(UIMouseEvent evt, UIElement listeningElement) {
			OnSelectTab(mod);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);
			if (panel.IsMouseHovering) {
				UICommon.TooltipMouseText(text: HoverText()); // Adds box behind hover text
			}
		}

		//private void MyMouseOver(UIMouseEvent evt, UIElement listeningElement) {
		//	SoundEngine.PlaySound(SoundID.MenuTick);
		//	// 37 anvil
		//	// 113 minion spawn
		//	// npc_hit_5 sparkle
		//	// tinks
		//}

		private string HoverText() {
			return mod.UsingMissingTexture ? "[MISSING TEXTURE] " + mod.ModTabName : mod.ModTabName;
		}
	}
}
