using Microsoft.Xna.Framework;
using Munchies.Models;
using Munchies.Models.Enums;
using Munchies.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Munchies {
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.

	public class Munchies : Mod {
		internal static Munchies instance;
		internal static Report report;
		internal static LocalizedText ConcatenateNewline;

		internal static ModKeybind ToggleReportHotKey;

		// Maybe remove this line
		public Munchies() { }

		public override void Load() {
			instance = this;

			EnumUtil.LoadEnumText(this);
			ConcatenateNewline = this.GetLocalization("Common.ConcatenateNewline");
			ToggleReportHotKey = KeybindLoader.RegisterKeybind(this, "ToggleReport", "K");

			report ??= new(); // initialize the report if it is null
		}

		public override void Unload() {
			instance = null;
			report = null;
			ToggleReportHotKey = null;
		}

		public override object Call(params object[] args) {
			try {
				if (args.Length >= 1) {
					return args[0] switch {
						"AddSingleConsumable" => HandleAddSingleConsumable(args),
						"AddMultiUseConsumable" => HandleAddMultiConsumable(args),
						"AddVanillaConsumable" => HandleAddVanillaConsumable(args),
						"AddVanillaMultiUseConsumable" => HandleAddVanillaMultiConsumable(args),
						// can add more types of calls here in the future
						_ => false
					};
				} else {
					return false;
				}
			} catch {
				Logger.Error($"Munchies Call Error");
			}
			return "Failure";
		}

		private bool HandleAddSingleConsumable(params object[] args) {
			Mod mod = null;
			try {
				mod = args[1] as Mod;
				ConsumableMod externalMod = new(mod);

				object apiString = args[2];
				Version apiVersion = apiString is string ? new Version(apiString as string) : this.Version; // current as of this update is 1.3
				ModItem item = args[3] as ModItem;
				string categoryName = args[4] as string;
				Func<bool> hasBeenConsumed = args[5] as Func<bool>;
				object customColorObject = args.Length >= 7 ? args[6] : null;
				string difficulty = args.Length >= 8 ? args[7] as string : "classic";
				LocalizedText extraTooltip = args.Length >= 9 ? args[8] as LocalizedText : null;
				Func<bool> isAvailable = args.Length >= 10 ? args[9] as Func<bool> : null;

				Consumable consumable = new(
					modItem: item,
					categoryName: categoryName,
					CustomColor: (Color?)customColorObject,
					currentCount: () => hasBeenConsumed().ToInt(),
					totalCount: () => 1,
					difficulty: difficulty,
					available: isAvailable,
					extraTooltip: extraTooltip
				);

				return Report.AddConsumableToList(mod: externalMod, consumable: consumable);
			} catch {
				Logger.Error($"Error adding consumable from the {mod?.Name ?? "Unknown mod"} mod, see log for details");
				return false;
			}
		}

		private bool HandleAddMultiConsumable(params object[] args) {
			Mod mod = null;
			try {
				mod = args[1] as Mod;
				ConsumableMod externalMod = new(mod);

				object apiString = args[2];
				Version apiVersion = apiString is string ? new Version(apiString as string) : this.Version; // current as of this update is 1.3
				ModItem item = args[3] as ModItem;
				string categoryName = args[4] as string;
				Func<int> currentCount = args[5] as Func<int>;
				Func<int> totalCount = args[6] as Func<int>;
				object customColorObject = args.Length >= 8 ? args[7] : null;
				string difficulty = args.Length >= 9 ? args[8] as string : "classic";
				LocalizedText extraTooltip = args.Length >= 10 ? args[9] as LocalizedText : null;
				Func<bool> isAvailable = args.Length >= 11 ? args[10] as Func<bool> : null;

				Consumable consumable = new(
					modItem: item,
					categoryName: categoryName,
					CustomColor: (Color?)customColorObject,
					currentCount: currentCount,
					totalCount: totalCount,
					difficulty: difficulty,
					available: isAvailable,
					extraTooltip: extraTooltip
				);

				return Report.AddConsumableToList(mod: externalMod, consumable: consumable);
			} catch {
				Logger.Error($"Error adding consumable from the {mod?.Name ?? "Unknown mod"} mod, see log for details");
				return false;
			}
		}

		private bool HandleAddVanillaConsumable(params object[] args) {
			try {
				object apiString = args[1];
				Version apiVersion = apiString is string ? new Version(apiString as string) : this.Version; // current as of this update is 1.3
				if (apiVersion != new Version(1, 3, 0)) return false; // exit if not using verison 1.3 of this mod

				int itemId = int.Parse(args[2] as string);
				Func<bool> hasBeenConsumed = args[4] as Func<bool>;
				string difficulty = args.Length >= 6 ? args[5] as string : "classic";
				LocalizedText extraTooltip = args.Length >= 7 ? args[6] as LocalizedText : null;
				Func<bool> isAvailable = args.Length >= 8 ? args[7] as Func<bool> : null;

				Consumable consumable = new(
					vanillaItemId: itemId,
					type: GetType(args[3] as string),
					currentCount: () => hasBeenConsumed().ToInt(),
					totalCount: () => 1,
					difficulty: difficulty,
					available: isAvailable,
					extraTooltip: extraTooltip
				);

				return Report.AddConsumableToList(mod: Report.VanillaConsumableMod, consumable: consumable);
			} catch {
				Logger.Error($"Error adding vanilla consumable, see log for details");
				return false;
			}
		}

		private bool HandleAddVanillaMultiConsumable(params object[] args) {
			try {
				object apiString = args[1];
				Version apiVersion = apiString is string ? new Version(apiString as string) : this.Version; // current as of this update is 1.3
				if (apiVersion != new Version(1, 3, 0)) return false; // exit if not using verison 1.3 of this mod

				int itemId = int.Parse(args[2] as string);
				Func<int> currentCount = args[4] as Func<int>;
				Func<int> totalCount = args[5] as Func<int>;
				string difficulty = args.Length >= 7 ? args[6] as string : "classic";
				LocalizedText extraTooltip = args.Length >= 8 ? args[7] as LocalizedText : null;
				Func<bool> isAvailable = args.Length >= 9 ? args[8] as Func<bool> : null;

				Consumable consumable = new(
					vanillaItemId: itemId,
					type: GetType(args[3] as string),
					currentCount: currentCount,
					totalCount: totalCount,
					difficulty: difficulty,
					available: isAvailable,
					extraTooltip: extraTooltip
				);

				return Report.AddConsumableToList(mod: Report.VanillaConsumableMod, consumable: consumable);
			} catch {
				Logger.Error($"Error adding vanilla multi-use consumable, see log for details");
				return false;
			}
		}

		internal static ConsumableType GetType(string type) {
			if (Enum.TryParse(type, out ConsumableType consumableType)) {
				return consumableType;
			} else {
				return ConsumableType.player;
			}
		}

		internal static Difficulty? GetDifficulty(string diff) {
			if (diff == "") return null;

			if (Enum.TryParse(diff, out Difficulty difficulty)) {
				return difficulty;
			} else {
				return Difficulty.mod_custom;
			}
		}
	}
}
