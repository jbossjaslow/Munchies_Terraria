using Munchies.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Munchies.Models {
	public class Report {
		public static ConsumableMod VanillaConsumableMod = new(modTabName: "Terraria", modTabTexturePath: "Terraria/Images/Item_4765");
		public static List<ConsumablesEntry> ConsumablesList = [];

		public Report() {
			List<Consumable> vanillaConsumables = EnumUtil
				.AllCases<ConsumableItem>()
				.Select(ci => new VanillaConsumable(ci))
				.Select(vc => new Consumable(vanillaItemId: vc.ItemId, type: vc.Type, currentCount: vc.CurrentCount, totalCount: vc.TotalCount))
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
				if (c.DisplayText == consumable.DisplayText) {
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