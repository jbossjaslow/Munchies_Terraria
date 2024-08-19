using Munchies.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace Munchies.Models {
	public class Report {
		public static IConsumableMod VanillaConsumableMod = new VanillaConsumableMod();
		//public static List<IConsumable> VanillaConsumables = [];
		// Maps mod name to list of consumables from that mod
		//public static Dictionary<IConsumableMod, List<IConsumable>> ModConsumablesDict = [];
		public static List<ConsumablesEntry> ConsumablesList = [];

		public Report() {
			List<IConsumable> vanillaConsumables = EnumUtil
				.AllCases<ConsumableItem>()
				.Select(i => (IConsumable)new VanillaConsumable(i))
				.ToList();

			ConsumablesList.Add(new(VanillaConsumableMod, vanillaConsumables));

			//List<IConsumable> fakeList = [
			//	new VanillaConsumable(ConsumableItem.minecartUpgradeKit),
			//	new VanillaConsumable(ConsumableItem.peddlersSatchel),
			//	new VanillaConsumable(ConsumableItem.artisanLoaf)
			//];
			//ConsumablesList.Add(new(new TestConsumableMod(), fakeList));
		}

		public static bool AddModtoList(IConsumableMod mod) {
			foreach (ConsumablesEntry entry in ConsumablesList) {
				if (entry.Mod == mod) {
					return false;
				}
			}

			// at this point, we don't have the mod registered yet
			// Add new entry with empty consumables arr
			ConsumablesList.Add(new(mod, []));
			return true;
		}

		public static bool AddConsumableToList(string modName, IConsumable consumable) {
			foreach (ConsumablesEntry entry in ConsumablesList) {
				if (entry.Mod.ModTabName == modName) {
					// mod exists
					foreach (IConsumable c in entry.Consumables) {
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

			// mod doesn't exist, exit
			return false;
		}
	}

	public class ConsumablesEntry(IConsumableMod mod, List<IConsumable> consumables) {
		public IConsumableMod Mod = mod;
		public List<IConsumable> Consumables = consumables;
	}
}