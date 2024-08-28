using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Munchies.UIElements {
	internal class ReportCloseButton(Asset<Texture2D> texture, string hoverText, Color color): UIImageButton(texture: texture) {
		// Tooltip text that will be shown on hover
		//internal string hoverText = hoverText;

		private readonly float _visibilityActive = 1f;
		private readonly float _visibilityInactive = 0.4f;

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			// When you override UIElement methods, don't forget call the base method
			// This helps to keep the basic behavior of the UIElement
			//base.DrawSelf(spriteBatch);
			CalculatedStyle dimensions = GetDimensions();
			spriteBatch.Draw(texture.Value, dimensions.Position(), color * (base.IsMouseHovering ? _visibilityActive : _visibilityInactive));

			// IsMouseHovering becomes true when the mouse hovers over the current UIElement
			if (IsMouseHovering) Main.hoverItemName = hoverText;
		}
	}
}