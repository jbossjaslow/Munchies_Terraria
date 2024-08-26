using Munchies.Models;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace Munchies {
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.

	public class Munchies: Mod {
		internal static Munchies instance;
		internal static Report report;

		internal static ModKeybind ToggleReportHotKey;

		// Maybe remove this line
		public Munchies() { }

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

		public override object Call(params object[] args) {
			try {
				if (args.Length >= 1) {
					return args[0] switch {
						"AddConsumable" => HandleAddConsumable(args),
						"AddVanillaConsumable" => HandleAddVanillaConsumable(args),
						// can add more types of calls here in the future
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

		private bool HandleAddConsumable(params object[] args) {
			Mod mod = null;
			try {
				Consumable consumable;
				mod = args[1] as Mod;
				ConsumableMod externalMod = new(mod);

				object apiString = args[2];
				Version apiVersion = apiString is string ? new Version(apiString as string) : this.Version; // current as of this update is 1.3
				ModItem item = args[3] as ModItem;
				Func<int> currentCount = args[5] as Func<int>;
				Func<int> totalCount = args[6] as Func<int>;

				consumable = new(
					modItem: item,
					CategoryOrCustomColor: args[4],
					currentCount: currentCount,
					totalCount: totalCount
				);

				return Report.AddConsumableToList(mod: externalMod, consumable: consumable);

				// Add optional fields for max # of items, option of func<int> for how many have been consumed
			} catch (Exception e) {
				(mod ?? this).Logger.Error($"Error adding consumable: {e.Message}, {e.StackTrace}");
				return false;
			}
		}

		private bool HandleAddVanillaConsumable(params object[] args) {
			try {
				ConsumableMod vanillaMod = new(modTabName: "Terraria", modTabTexturePath: "Terraria/Images/Item_4765");
				Consumable consumable;

				object apiString = args[2];
				Version apiVersion = apiString is string ? new Version(apiString as string) : this.Version; // current as of this update is 1.3
				if (apiVersion != new Version(1, 3, 0)) return false; // exit if not using verison 1.3 of this mod

				int itemId = int.Parse(args[3] as string);
				Func<int> currentCount = args[5] as Func<int>;
				Func<int> totalCount = args[6] as Func<int>;

				consumable = new(
					vanillaItemId: itemId,
					type: GetType(args[4] as string),
					currentCount: currentCount,
					totalCount: totalCount
				);

				return Report.AddConsumableToList(mod: vanillaMod, consumable: consumable);
			} catch (Exception e) {
				Logger.Error($"Error adding consumable: {e.Message}, {e.StackTrace}");
				return false;
			}
		}

		static ConsumableType GetType(string type) {
			if (Enum.TryParse(type, out ConsumableType consumableType)) {
				return consumableType;
			} else {
				return ConsumableType.player_normal;
			}
		}
	}
}
