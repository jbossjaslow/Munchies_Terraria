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
				.Select(i => new VanillaConsumable(i))
				.Select(vc => new Consumable(vc))
				.ToList();

			ConsumablesList.Add(new(VanillaConsumableMod, vanillaConsumables));

			//public class TestConsumableMod : IConsumableMod {
			//	public string ModTabName => "Test Mod";

			//	public string ModTabTexturePath() {
			//		return "Terraria/Images/Item_4760";
			//	}

			//	public TestConsumableMod() { }
			//}

			//List<IConsumable> fakeList = [
			//	new VanillaConsumable(ConsumableItem.minecartUpgradeKit),
			//	new VanillaConsumable(ConsumableItem.peddlersSatchel),
			//	new VanillaConsumable(ConsumableItem.artisanLoaf)
			//];
			//ConsumablesList.Add(new(new TestConsumableMod(), fakeList));
		}

		public static bool AddModtoList(ConsumableMod mod) {
			foreach (ConsumablesEntry entry in ConsumablesList) {
				if (entry.Mod.ModTabName == mod.ModTabName) {
					return false;
				}
			}

			// at this point, we don't have the mod registered yet
			// Add new entry with empty consumables arr
			ConsumablesList.Add(new(mod, []));
			return true;
		}

		public static bool AddConsumableToList(string modName, Consumable consumable) {
			foreach (ConsumablesEntry entry in ConsumablesList) {
				if (entry.Mod.ModTabName == modName) {
					// mod exists
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

			// mod doesn't exist, exit
			return false;
		}
	}
}