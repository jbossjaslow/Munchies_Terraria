using Munchies.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Munchies.Models {
	public class Report {
		public static List<Consumable> Consumables = [];

		public Report() {
			//UpdateInfo();

			Consumables = EnumUtil
				.AllCases<ConsumableItem>()
				.Select(i => new Consumable(i))
				.ToList();
		}

		//public static void UpdateInfo() {
		//	Consumables = EnumUtil
		//		.AllCases<ConsumableItem>()
		//		.Select(i => new Consumable(i))
		//		.ToList();
		//}
	}
}