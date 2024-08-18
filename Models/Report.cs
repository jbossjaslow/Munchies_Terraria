using Munchies.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Munchies.Models {
	public class Report {
		public static List<VanillaConsumable> VanillaConsumables = [];
		// Maps mod name to list of consumables from that mod
		public static Dictionary<string, List<IConsumable>> ModConsumables = [];

		public Report() {
			//UpdateInfo();

			VanillaConsumables = EnumUtil
				.AllCases<ConsumableItem>()
				.Select(i => new VanillaConsumable(i))
				.ToList();
		}

		//public static void UpdateInfo() {
		//	VanillaConsumables = EnumUtil
		//		.AllCases<ConsumableItem>()
		//		.Select(i => new VanillaConsumable(i))
		//		.ToList();
		//}
	}
}