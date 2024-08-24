using Munchies.UIElements;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Munchies.Configuration {
	public class Config: ModConfig {
		// Required
		public override ConfigScope Mode => ConfigScope.ClientSide;

		public static Config instance;

		[DefaultValue(false)]
		public bool AllowDragging;

		[DefaultValue(false)]
		public bool ResetDragCoords;

		//public override void OnChanged() {
		//	DraggablePanel.AllowsDragging = AllowDragging;
		//}

		public override void OnLoaded() {
			instance = this;
		}
	}
}
