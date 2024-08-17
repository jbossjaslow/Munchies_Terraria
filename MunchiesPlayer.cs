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
				if (!ReportUI.Visible) {
					//Munchies.report.updateInfo();
					ReportUISystem.Instance.ReportUI.AddConsumablesToList();
					Main.playerInventory = false; // hide the player inventory
				}
				ReportUI.Visible = !ReportUI.Visible;
			}
		}

		public override void OnEnterWorld() {
			//base.OnEnterWorld();
			ReportUI.Visible = false;
		}
	}
}
