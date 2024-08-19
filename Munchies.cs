using Munchies.Models;
using Munchies.ModSupport;
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

			report ??= new(); // initialize the report if it is null

			ToggleReportHotKey = KeybindLoader.RegisterKeybind(this, "ToggleReport", "K");
		}

		public override void Unload() {
			instance = null;
			report = null;
			ToggleReportHotKey = null;
		}

		/*
		 * 0: Function: AddMod => string
		 * 0: Mod tab name => string
		 * 1: Mod tab texture => string
		 * 
		 * 0: Function: AddConsumable => string
		 * 1: Consumable mod => string
		 * 2: Consumable name => string
		 * 3: Consumable texture => string
		 * 4: Consumable hover text => string
		 * 5: Consumable type => string
		 * 6: Consumable hasBeenConsumed => Func<bool>
		 * 7: Asset dimensions X => float
		 * 8: Asset dimensions Y => float
		 */
		public override object Call(params object[] args) {
			try {
				if (args.Length >= 1) {
					return args[0] switch {
						"AddMod" => HandleAddMod(args),
						"AddConsumable" => HandleAddConsumable(args),
						_ => false
					};
				} else {
				return false;
			}
			} catch (Exception e) {
				Logger.Error($"Call Error: {e.StackTrace} {e.Message}");
			}
			return "Failure";
		}

		private bool HandleAddMod(params object[] args) {
			if (args.Length == 3) {
				IConsumableMod mod = new ExternalMod(modTabName: args[1] as string, modTabTexturePath: args[2] as string);

				return Report.AddModtoList(mod);
			} else {
				return false;
			}
		}

		private bool HandleAddConsumable(params object[] args) {
			if (args.Length == 9) {
				(float X, float Y) assetDims = (X: float.Parse(args[7] as string), Y: float.Parse(args[8] as string));
				IConsumable consumable = new ExternalConsumable(displayText: args[2] as string, texturePath: args[3] as string, assetDimensions: assetDims, hoverText: args[4] as string, type: GetType(type: args[5] as string), hasBeenConsumed: args[6] as Func<bool>);

				return Report.AddConsumableToList(modName: args[1] as string, consumable: consumable);
			} else {
				return false;
			}
		}

		private ConsumableType GetType(string type) {
			if (Enum.TryParse(type, out ConsumableType consumableType)) {
				return consumableType;
			} else {
				return ConsumableType.player_normal;
			}
		}
	}
}
