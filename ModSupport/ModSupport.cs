using Terraria;
using Terraria.ModLoader;

namespace Munchies.ModSupport {
	public class ModList {
		// Calamity
		public static bool HasCalamity() => ModLoader.HasMod(ModName.Calamity);

		public static bool GetCalamity(out Mod Calamity) => ModLoader.TryGetMod(ModName.Calamity, out Calamity);

		//[JITWhenModsEnabled(ModName.Calamity)]
		//public float CalamityStealth => CalamityStealth.BossHealthBarVisible();
	}
}
