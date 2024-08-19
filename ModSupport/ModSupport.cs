using Munchies.Models;
using System;

namespace Munchies.ModSupport {
	public class ExternalMod: IConsumableMod {
		private readonly string _modTabName;
		private readonly string _modTabTexturePath;

		public string ModTabName => _modTabName;
		public string ModTabTexturePath => _modTabTexturePath;

		public ExternalMod(string modTabName, string modTabTexturePath) {
			_modTabName = modTabName;
			_modTabTexturePath = modTabTexturePath;
		}
	}

	public class ExternalConsumable : IConsumable {
		private readonly string _displayText;
		public string DisplayText => _displayText;
		private readonly string _texturePath;
		public string TexturePath => _texturePath;
		private readonly (float X, float Y) _assetDimensions;
		public (float X, float Y) AssetDimensions => _assetDimensions;
		private readonly string _hoverText;
		public string HoverText => _hoverText;
		private readonly ConsumableType _type;
		public ConsumableType Type => _type;

		private Func<bool> _hasBeenEnabled;
		public Func<bool> HasBeenConsumed => _hasBeenEnabled;

		public ExternalConsumable(string displayText, string texturePath, (float X, float Y) assetDimensions, string hoverText, ConsumableType type, Func<bool> hasBeenConsumed) {
			_displayText = displayText;
			_texturePath = texturePath;
			_assetDimensions = assetDimensions;
			_hoverText = hoverText;
			_type = type;
			_hasBeenEnabled = hasBeenConsumed;
		}
	}
}
