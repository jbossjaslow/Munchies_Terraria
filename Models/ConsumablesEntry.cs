using System.Collections.Generic;
using Terraria.ModLoader;

namespace Munchies.Models {
	//public class ConsumablesEntry(ConsumableMod mod, List<Consumable> consumables) {
	//	public ConsumableMod Mod = mod;
	//	public List<Consumable> Consumables = consumables;
	//}

	public class ConsumablesEntry(ConsumableMod mod, List<Consumable> consumables) {
		public ConsumableMod Mod = mod;
		public List<Consumable> Consumables = consumables;
	}
}
