using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munchies.Models {
	public interface IConsumable {
		string DisplayText { get; }
		string TexturePath { get; }
		(float X, float Y) AssetDimensions { get; }
		string HoverText {  get; }
		ConsumableType Type { get; }
		Func<bool> HasBeenConsumed { get; }
	}

	public interface IConsumableMod {
		string ModTabName { get; }
		string ModTabTexturePath { get; }
	}

	//public class TestConsumableMod : IConsumableMod {
	//	public string ModTabName => "Test Mod";

	//	public string ModTabTexturePath() {
	//		return "Terraria/Images/Item_4760";
	//	}

	//	public TestConsumableMod() { }
	//}
}
