using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munchies.Localization {
	internal class MunchiesLocKey {
		private MunchiesLocKey() { }

		// Consumable Types
		public static readonly string PlayerTypeTooltip = "ConsumableTypes.Player.Tooltip";
		public static readonly string WorldTypeTooltip = "ConsumableTypes.World.Tooltip";

		// Difficulty Types
		public static readonly string ClassicDifficultyTooltip = "Difficulties.Classic.Tooltip"; // unused currently
		public static readonly string ExpertDifficultyTooltip = "Difficulties.Expert.Tooltip";
		public static readonly string MasterDifficultyTooltip = "Difficulties.Master.Tooltip";
		public static readonly string CustomModDifficultyTooltip = "Difficulties.CustomMod.Tooltip";
	}
}
