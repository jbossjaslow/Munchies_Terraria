using Munchies.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;

namespace Munchies.Models {
	public class Report {
		public static ConsumableMod VanillaConsumableMod = new(modTabName: "Terraria", modTabTexturePath: "Terraria/Images/Item_4765");
		public static List<ConsumablesEntry> ConsumablesList = [];

		private static readonly int[] VanillaItems = [
			// multi-use
			ItemID.LifeCrystal,
			ItemID.LifeFruit,
			ItemID.ManaCrystal,

			// normal
			ItemID.ArtisanLoaf,
			ItemID.TorchGodsFavor,
			ItemID.AegisCrystal,
			ItemID.AegisFruit,
			ItemID.ArcaneCrystal,
			ItemID.Ambrosia,
			ItemID.GummyWorm,
			ItemID.GalaxyPearl,

			// expert
			ItemID.DemonHeart,
			ItemID.MinecartPowerup,

			// world
			ItemID.CombatBook,
			ItemID.CombatBookVolumeTwo,
			ItemID.PeddlersSatchel,
		];

		public Report() {
			List<Consumable> vanillaConsumables = VanillaItems
				.Select(id => new Consumable(vanillaItemId: id))
				.ToList();

			ConsumablesList.Add(new(VanillaConsumableMod, vanillaConsumables));
		}

		internal static ConsumablesEntry GetModEntryOrAddIfNeeded(ConsumableMod mod) {
			ConsumablesEntry foundEntry = ConsumablesList.Find(e => e.Mod.ModTabName == mod.ModTabName);
			if (foundEntry != null) {
				return foundEntry;
			} else {
				ConsumablesEntry newEntry = new(mod, []);
				ConsumablesList.Add(newEntry);
				return newEntry;
			}
		}

		public static bool AddConsumableToList(ConsumableMod mod, Consumable consumable) {
			ConsumablesEntry entry = GetModEntryOrAddIfNeeded(mod);

			foreach (Consumable c in entry.Consumables) {
				if (c.ID == consumable.ID) {
					// consumable already exists, exit
					return false;
				}
			}

			// consumable does not exist, add it
			entry.Consumables.Add(consumable);
			return true;
		}
	}
}