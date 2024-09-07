using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.UI;

namespace Munchies.UIElements {
	public class CenteredUIImage: UIElement {
		private Asset<Texture2D> _texture;
		public float ImageScale = 1f;
		public float Rotation;
		public Color Color = Color.White;
		private Vector2 CenteredOrigin = new(0.5f, 0.5f);

		public CenteredUIImage(Asset<Texture2D> texture) {
			SetImage(texture);
		}

		public void SetImage(Asset<Texture2D> texture) {
			_texture = texture;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			//CalculatedStyle dimensions = GetDimensions();

			//Vector2 size = texture2D.Size();
			//Vector2 position = dimensions.Center() - (size * CenteredOrigin);
			//Vector2 origin = new((-dimensions.Width / 2) + (texture2D.Width / 2), (-dimensions.Height / 2) + (texture2D.Height / 2));

			// It would appear that position renders down and right, whereas origin renders up and left
			// -(size * CenteredOrigin) for position == (size * CenteredOrigin) for origin
			spriteBatch.Draw(
				texture: _texture.Value,
				position: GetDimensions().Center(),
				sourceRectangle: null,
				color: Color,
				rotation: Rotation,
				origin: _texture.Value.Size() * CenteredOrigin,
				scale: ImageScale,
				effects: SpriteEffects.None,
				layerDepth: 0f
			);
		}
	}
}
