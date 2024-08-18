using Munchies.Models;
using System;
using Terraria.ModLoader;

namespace Munchies
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.

    public class Munchies : Mod {
		internal static Munchies instance;
		internal static Report report;

		internal static ModKeybind ToggleReportHotKey;

		public Munchies() {}

		public override void Load() {
			instance = this;

			report = new Report();

			ToggleReportHotKey = KeybindLoader.RegisterKeybind(this, "ToggleReport", "K");
		}

		public override void Unload() {
			instance = null;
			report = null;
			ToggleReportHotKey = null;
		}

		/*
		 * 0: Mod tab name
		 * 1: Mod tab texture
		 * 2: Consumable name
		 * 3: Consumable texture
		 * 4: Consumable hover text
		 * 5: Consumable type
		 * 6: Consumable hasBeenConsumed
		 */
		public override object Call(params object[] args) {
			try {

			} catch (Exception e) {
				Logger.Error($"Call Error: {e.StackTrace} {e.Message}");
			}
			return "Failure";
		}
	}
}
