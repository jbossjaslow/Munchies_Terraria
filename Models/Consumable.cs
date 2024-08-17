using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Utilities.Terraria.Utilities;

namespace Munchies.Models {
	public class Consumable {
		public ConsumableItem Item;

		public Consumable(ConsumableItem item) {
			this.Item = item;
		}

		public ConsumableType Type => Item switch {
			// normal
			ConsumableItem.artisanLoaf => ConsumableType.player_normal,
			ConsumableItem.torchGod => ConsumableType.player_normal,
			ConsumableItem.vitalCrystal => ConsumableType.player_normal,
			ConsumableItem.aegisFruit => ConsumableType.player_normal,
			ConsumableItem.arcaneCrystal => ConsumableType.player_normal,
			ConsumableItem.ambrosia => ConsumableType.player_normal,
			ConsumableItem.gummyWorm => ConsumableType.player_normal,
			ConsumableItem.galaxyPearl => ConsumableType.player_normal,

			// expert
			ConsumableItem.demonHeart => ConsumableType.player_expert,
			ConsumableItem.minecartUpgradeKit => ConsumableType.player_expert,

			// world
			ConsumableItem.advCombatTech1 => ConsumableType.world,
			ConsumableItem.advCombatTech2 => ConsumableType.world,
			ConsumableItem.peddlersSatchel => ConsumableType.world,
			_ => throw new System.NotImplementedException(),
		};

		public Color DisplayTextColor => Type switch {
			ConsumableType.player_normal => Color.White,
			ConsumableType.player_expert => Main.expertMode ? Color.Orange : Color.Gray,
			ConsumableType.world => new Color(r: 242, g: 111, b: 238),
			_ => throw new System.NotImplementedException(),
		};

		public string DisplayText => Item switch {
			// normal
			ConsumableItem.artisanLoaf => "Artisan Loaf",
			ConsumableItem.torchGod => "Torch God's Favor",
			ConsumableItem.vitalCrystal => "Vital Crystal",
			ConsumableItem.aegisFruit => "Aegis Fruit",
			ConsumableItem.arcaneCrystal => "Arcane Crystal",
			ConsumableItem.ambrosia => "Ambrosia",
			ConsumableItem.gummyWorm => "Gummy Worm",
			ConsumableItem.galaxyPearl => "Galaxy Pearl",

			// expert
			ConsumableItem.demonHeart => "Demon Heart",
			ConsumableItem.minecartUpgradeKit => "Minecart Upgrade Kit",

			// world
			ConsumableItem.advCombatTech1 => "Adv. Combat Tech V1",
			ConsumableItem.advCombatTech2 => "Adv. Combat Tech V2",
			ConsumableItem.peddlersSatchel => "Peddler's Satchel",
			_ => throw new System.NotImplementedException(),
		};

		public string HoverText => Item switch {
			// normal
			ConsumableItem.artisanLoaf => "Increases range of crafting stations by 4 tiles",
			ConsumableItem.torchGod => "Allows automatic placing of biome torches",
			ConsumableItem.vitalCrystal => "Increases health regen by 20%",
			ConsumableItem.aegisFruit => "Increases defense by 4",
			ConsumableItem.arcaneCrystal => "Increases mana regeneration by 5%",
			ConsumableItem.ambrosia => "Increases mining and tile & wall placement by 5%",
			ConsumableItem.gummyWorm => "Increases fishing power by 3",
			ConsumableItem.galaxyPearl => "Increases luck by 0.03",

			// expert
			ConsumableItem.demonHeart => "[Expert Only] Adds an accessory slot",
			ConsumableItem.minecartUpgradeKit => "[Expert Only] Minecarts go faster and shoot lasers",

			// world
			ConsumableItem.advCombatTech1 => "[Once per World] Increases NPC defense by 6 and damage by 20%",
			ConsumableItem.advCombatTech2 => "[Once per World] Increases NPC defense by 6 and damage by 20% (stacks with Vol 1)",
			ConsumableItem.peddlersSatchel => "[Once per World] Traveling merchant sells an additional item",
			_ => throw new System.NotImplementedException(),
		};

		// "Terraria/Images/Item_" is prefixed before path string
		public string AssetPath => Item switch {
			// normal
			ConsumableItem.artisanLoaf => "5326",
			ConsumableItem.torchGod => "5043",
			ConsumableItem.vitalCrystal => "5337",
			ConsumableItem.aegisFruit => "5338",
			ConsumableItem.arcaneCrystal => "5339",
			ConsumableItem.ambrosia => "5342",
			ConsumableItem.gummyWorm => "5341",
			ConsumableItem.galaxyPearl => "5340",

			// expert
			ConsumableItem.demonHeart => "3335",
			ConsumableItem.minecartUpgradeKit => "5289",

			// world
			ConsumableItem.advCombatTech1 => "4382",
			ConsumableItem.advCombatTech2 => "5336",
			ConsumableItem.peddlersSatchel => "5343",
			_ => throw new System.NotImplementedException(),
		};

		public (float X, float Y) AssetDimensions => Item switch {
			// normal
			ConsumableItem.artisanLoaf => (32f, 32f),
			ConsumableItem.torchGod => (22f, 36f),
			ConsumableItem.vitalCrystal => (36f, 28f),
			ConsumableItem.aegisFruit => (26f, 32f),
			ConsumableItem.arcaneCrystal => (26f, 30f),
			ConsumableItem.ambrosia => (24f, 28f),
			ConsumableItem.gummyWorm => (20f, 24f),
			ConsumableItem.galaxyPearl => (22f, 22f),

			// expert
			ConsumableItem.demonHeart => (30f, 30f),
			ConsumableItem.minecartUpgradeKit => (32f, 32f),

			// world
			ConsumableItem.advCombatTech1 => (28f, 30f),
			ConsumableItem.advCombatTech2 => (28f, 30f),
			ConsumableItem.peddlersSatchel => (34f, 28f),
			_ => throw new System.NotImplementedException(),
		};

		public bool HasBeenConsumed => Item switch {
			// normal
			ConsumableItem.artisanLoaf => Main.LocalPlayer.ateArtisanBread,
			ConsumableItem.torchGod => Main.LocalPlayer.unlockedBiomeTorches,
			ConsumableItem.vitalCrystal => Main.LocalPlayer.usedAegisCrystal,
			ConsumableItem.aegisFruit => Main.LocalPlayer.usedAegisFruit,
			ConsumableItem.arcaneCrystal => Main.LocalPlayer.usedArcaneCrystal,
			ConsumableItem.ambrosia => Main.LocalPlayer.usedAmbrosia,
			ConsumableItem.gummyWorm => Main.LocalPlayer.usedGummyWorm,
			ConsumableItem.galaxyPearl => Main.LocalPlayer.usedGalaxyPearl,

			// expert
			ConsumableItem.demonHeart => Main.LocalPlayer.CanDemonHeartAccessoryBeShown(),
			ConsumableItem.minecartUpgradeKit => Main.LocalPlayer.unlockedSuperCart,

			// world
			ConsumableItem.advCombatTech1 => NPC.combatBookWasUsed,
			ConsumableItem.advCombatTech2 => NPC.combatBookVolumeTwoWasUsed,
			ConsumableItem.peddlersSatchel => NPC.peddlersSatchelWasUsed,
			_ => throw new System.NotImplementedException(),
		};
	}
}
