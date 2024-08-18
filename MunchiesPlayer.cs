using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria;
using Munchies.UIElements;

namespace Munchies
{
    class MunchiesPlayer : ModPlayer {
		public override void ProcessTriggers(TriggersSet triggerSet) {
			if (Munchies.ToggleReportHotKey.JustPressed) {
				// toggle visibility for hotkey
				ReportUI.SetVisible(!ReportUI.Visible);
			} else if (triggerSet.Inventory && !Main.playerInventory && ReportUI.Visible) {
				// close if inventory is opened
				ReportUI.SetVisible(false, playCloseSound: false);
			}
		}

		public override void OnEnterWorld() {
			//base.OnEnterWorld();
			ReportUI.SetVisible(false);
		}
	}
}
