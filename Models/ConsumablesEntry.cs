using System.Collections.Generic;

namespace Munchies.Models {
	public class ConsumablesEntry(ConsumableMod mod, List<Consumable> consumables) {
		public ConsumableMod Mod = mod;
		public List<Consumable> Consumables = consumables;
	}
}
