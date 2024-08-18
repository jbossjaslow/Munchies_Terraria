using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munchies.Models {
	public interface IConsumable {
		string ModTabName { get; }
		Asset<Texture2D> ModTabTexture { get; }
		string DisplayText { get; }
		Asset<Texture2D> Texture { get; }
		string HoverText {  get; }
		ConsumableType Type { get; }
		bool HasBeenConsumed();

	}
}
