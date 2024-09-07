using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Munchies.Models;
using Munchies.Models.Enums;
using Munchies.Utilities;
using ReLogic.Content;
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
		internal static LocalizedText DefaultTitle;

		internal static ModKeybind ToggleReportHotKey;
		public static Asset<Texture2D> UnknownTexture;

		internal static LocalizedText LifeCrystalAcquisitionText;
		internal static LocalizedText LifeFruitAcquisitionText;
		internal static LocalizedText ManaCrystalAcquisitionText;
		internal static LocalizedText ArtisanLoafAcquisitionText;
		internal static LocalizedText TorchGodsFavorAcquisitionText;
		internal static LocalizedText VitalCrystalAcquisitionText;
		internal static LocalizedText AegisFruitAcquisitionText;
		internal static LocalizedText ArcaneCrystalAcquisitionText;
		internal static LocalizedText AmbrosiaAcquisitionText;
		internal static LocalizedText GummyWormAcquisitionText;
		internal static LocalizedText GalaxyPearlAcquisitionText;
		internal static LocalizedText DemonHeartAcquisitionText;
		internal static LocalizedText MinecartUpgradeKitAcquisitionText;
		internal static LocalizedText AdvCombatTech1AcquisitionText;
		internal static LocalizedText AdvCombatTech2AcquisitionText;
		internal static LocalizedText PeddlersSatchelAcquisitionText;

		public override void Load() {
			instance = this;

			EnumUtil.LoadEnumText(this);
			ConcatenateNewline = this.GetLocalization("Common.ConcatenateNewline");
			ToggleReportHotKey = KeybindLoader.RegisterKeybind(this, "ToggleReport", "K");
			DefaultTitle = this.GetLocalization("UI.Report.Consumables");
			UnknownTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Locked");

			LifeCrystalAcquisitionText = this.GetLocalization("Acquisition.LifeCrystal"); 
			LifeFruitAcquisitionText = this.GetLocalization("Acquisition.LifeFruit"); 
			ManaCrystalAcquisitionText = this.GetLocalization("Acquisition.ManaCrystal"); 
			ArtisanLoafAcquisitionText = this.GetLocalization("Acquisition.ArtisanLoaf"); 
			TorchGodsFavorAcquisitionText = this.GetLocalization("Acquisition.TorchGodsFavor"); 
			VitalCrystalAcquisitionText = this.GetLocalization("Acquisition.VitalCrystal"); 
			AegisFruitAcquisitionText = this.GetLocalization("Acquisition.AegisFruit"); 
			ArcaneCrystalAcquisitionText = this.GetLocalization("Acquisition.ArcaneCrystal"); 
			AmbrosiaAcquisitionText = this.GetLocalization("Acquisition.Ambrosia"); 
			GummyWormAcquisitionText = this.GetLocalization("Acquisition.GummyWorm"); 
			GalaxyPearlAcquisitionText = this.GetLocalization("Acquisition.GalaxyPearl"); 
			DemonHeartAcquisitionText = this.GetLocalization("Acquisition.DemonHeart"); 
			MinecartUpgradeKitAcquisitionText = this.GetLocalization("Acquisition.MinecartUpgradeKit"); 
			AdvCombatTech1AcquisitionText = this.GetLocalization("Acquisition.AdvCombatTech1"); 
			AdvCombatTech2AcquisitionText = this.GetLocalization("Acquisition.AdvCombatTech2"); 
			PeddlersSatchelAcquisitionText = this.GetLocalization("Acquisition.PeddlersSatchel"); 

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
				Version apiVersion = apiString is string ? new Version(apiString as string) : this.Version; // current as of this update is 1.4

				Consumable consumable = new(
					modItem: args[3] as ModItem,
					categoryName: args[4] as string,
					CustomColor: (Color?)(args.Length >= 7 ? args[6] : null),
					currentCount: () => (args[5] as Func<bool>)().ToInt(),
					totalCount: () => 1,
					difficulty: (args.Length >= 8 ? args[7] as string : Difficulty.classic.ToString()) ?? Difficulty.classic.ToString(),
					available: args.Length >= 10 ? args[9] as Func<bool> : null,
					extraTooltip: args.Length >= 9 ? args[8] as LocalizedText : null,
					acquisitionText: args.Length >= 11 ? args[10] as LocalizedText : null
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
				Version apiVersion = apiString is string ? new Version(apiString as string) : this.Version; // current as of this update is 1.4

				Consumable consumable = new(
					modItem: args[3] as ModItem,
					categoryName: args[4] as string,
					CustomColor: (Color?)(args.Length >= 8 ? args[7] : null),
					currentCount: args[5] as Func<int>,
					totalCount: args[6] as Func<int>,
					difficulty: (args.Length >= 9 ? args[8] as string : Difficulty.classic.ToString()) ?? Difficulty.classic.ToString(),
					available: args.Length >= 11 ? args[10] as Func<bool> : null,
					extraTooltip: args.Length >= 10 ? args[9] as LocalizedText : null,
					acquisitionText: args.Length >= 12 ? args[11] as LocalizedText : null
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
				Version apiVersion = apiString is string ? new Version(apiString as string) : this.Version; // current as of this update is 1.4

				Consumable consumable = new(
					vanillaItemId: int.Parse(args[2].ToString()),
					type: GetType(args[3] as string),
					currentCount: () => (args[4] as Func<bool>)().ToInt(),
					totalCount: () => 1,
					difficulty: (args.Length >= 6 ? args[5] as string : Difficulty.classic.ToString()) ?? Difficulty.classic.ToString(),
					available: args.Length >= 8 ? args[7] as Func<bool> : null,
					extraTooltip: args.Length >= 7 ? args[6] as LocalizedText : null,
					acquisitionText: args.Length >= 9 ? args[8] as LocalizedText : null
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
				Version apiVersion = apiString is string ? new Version(apiString as string) : this.Version; // current as of this update is 1.4

				Consumable consumable = new(
					vanillaItemId: int.Parse(args[2].ToString()),
					type: GetType(args[3] as string),
					currentCount: args[4] as Func<int>,
					totalCount: args[5] as Func<int>,
					difficulty: (args.Length >= 7 ? args[6] as string : Difficulty.classic.ToString()) ?? Difficulty.classic.ToString(),
					available: args.Length >= 9 ? args[8] as Func<bool> : null,
					extraTooltip: args.Length >= 8 ? args[7] as LocalizedText : null,
					acquisitionText: args.Length >= 10 ? args[9] as LocalizedText : null
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
