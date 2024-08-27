using Munchies.UIElements;
using Terraria.ModLoader;
using Terraria;

namespace Munchies.Hooks {
	class GlobalItemHandler : GlobalItem {
		public override void OnConsumeItem(Item item, Player player) {
			if (player != Main.LocalPlayer) return;

			// Only update the list if the used item is in the consumables list
			if (ReportUISystem.Instance.ReportUI.CurrentConsumables.Find(i => i.ID == item.type) != null)
				ReportUISystem.Instance.ReportUI.UpdateConsumablesList(item);
		}
	}
}
