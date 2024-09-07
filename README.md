# Munchies
This mod displays a checklist of permanent consumables. Additionally, it allows other mods to call into this mod to add their own lists of items in separate tabs
 
## Munchies Call Structure

### Adding a Singular Consumable

#### 0: `string` "AddSingleConsumable"
This tells Munchies to use this particular call

#### 1: `Mod` Your mod
This is the reference to your mod

#### 2: `string` Version
The version of the calls you want Munchies to use. Passing in the current version of Munchies is recommended. e.g. `"1.3"`. Do NOT use e.g. `munchiesMod.Version.ToString()`, that will not be backwards compatible.

#### 3: `ModItem` Your mod item
The reference to the item you want to add. This allows it to encapsulate several pieces of info like the texture, tooltip, etc.

#### 4: `string` The category for your mod item
Acceptable strings: "player" OR "world" for whether the item applies to the player or the world

#### 5: `Func<bool>` Has been consumed
Whether the item has been consumed or not. This will be triggered to know whether to place a checkmark next to the item in the list.

#### 6: `Color?` Custom text color
The color you would like your item text to display with, if desired. If you pass in `null`, white will be used instead

#### 7: `string` Difficulty
Which difficulty this item is locked behind, e.g. expert only.

Acceptable strings: "classic", "expert", "master" -- "expert" and "master" add a colored tooltip and difficulty icon, but do not affect availability on their own

Alternatively, you can pass in your own difficulty string if you'd like to use your mod's custom difficulty, e.g. Calamity's Revengeance mode

If you pass in `null`, Munchies will use "classic" difficulty

#### 8: `LocalizedText` Extra tooltip
Use this arg to pass in any extra lines you want to add to the normal item tooltip. This will be added as a newline to the existing item tooltip.
`null` can be passed as well if no extra tooltip is needed

#### 9: `Func<bool>` Availability
Whether the item is able to be used in the current context. For example, the Demon Heart can only be used in expert mode, so `() => Main.expertMode` is passed here for the vanilla item.
If this value is false, the item text (and checkmark, if applicable) will be greyed out, but still display. This also prevents the completion checker from considering this item. `null` can be passed if it's always available

#### 10: `LocalizedText` Acquisition text
Use this arg to pass in text explaining how to acquire your item. This will be added as the first line of the the hover tooltip.
`null` can be passed as well if no acquisition text is needed

---

### Adding a Multi-Use Consumable

#### 0: `string` "AddMultiUseConsumable"
This tells Munchies to use this particular call

#### 1: `Mod` Your mod
This is the reference to your mod

#### 2: `string` Version
The version of the calls you want Munchies to use. Passing in the current version of Munchies is recommended. e.g. `"1.3"`. Do NOT use e.g. `munchiesMod.Version.ToString()`, that will not be backwards compatible.

#### 3: `ModItem` Your mod item
The reference to the item you want to add. This allows it to encapsulate several pieces of info like the texture, tooltip, etc.

#### 4: `string` The category for your mod item
Acceptable strings: "player" OR "world" for whether the item applies to the player or the world

#### 5: `Func<int>` Current count
How many of this item have been consumed. If it's a single use item, pass `() => <item.hasBeenConsumed>.ToInt()`

#### 6: `Func<int>` Total count
The total number of uses this item has. For single use items, pass `() => 1`, or use "AddSingleConsumable" call instead

#### 7: `Color?` Custom text color
The color you would like your item text to display with, if desired. If you pass in `null`, white will be used instead

#### 8: `string` Difficulty
Which difficulty this item is locked behind, e.g. expert only.

Acceptable strings: "classic", "expert", "master" -- "expert" and "master" add a colored tooltip and difficulty icon, but do not affect availability on their own

Alternatively, you can pass in your own difficulty string if you'd like to use your mod's custom difficulty, e.g. Calamity's Revengeance mode

If you pass in `null`, Munchies will use "classic" difficulty

#### 9: `LocalizedText` Extra tooltip
Use this arg to pass in any extra lines you want to add to the normal item tooltip. This will be added as a newline to the existing item tooltip.
`null` can be passed as well if no extra tooltip is needed

#### 10: `Func<bool>` Availability
Whether the item is able to be used in the current context. For example, the Demon Heart can only be used in expert mode, so `() => Main.expertMode` is passed here for the vanilla item.
If this value is false, the item text (and checkmark, if applicable) will be greyed out, but still display. This also prevents the completion checker from considering this item. `null` can be passed if it's always available

#### 11: `LocalizedText` Acquisition text
Use this arg to pass in text explaining how to acquire your item. This will be added as the first line of the the hover tooltip.
`null` can be passed as well if no acquisition text is needed

---

### Adding a Vanilla Consumable

#### 0: `string` "AddVanillaConsumable"
This tells Munchies to use this particular call

#### 1: `string` Version
The version of the calls you want Munchies to use. Passing in the current version of Munchies is recommended. e.g. `"1.3"`. Do NOT use e.g. `munchiesMod.Version.ToString()`, that will not be backwards compatible.

#### 2: `int` Item ID
The ID of the vanilla item you wish to add

#### 3: `string` The category for your vanilla item
Acceptable strings: "player" OR "world" for whether the item applies to the player or the world

#### 4: `Func<bool>` Has been consumed
Whether the item has been consumed or not. This will be triggered to know whether to place a checkmark next to the item in the list.

#### 5: `string` Difficulty
Which difficulty this item is locked behind, e.g. expert only.

Acceptable strings: "classic", "expert", "master" -- "expert" and "master" add a colored tooltip and difficulty icon, but do not affect availability on their own

Alternatively, you can pass in your own difficulty string if you'd like to use your mod's custom difficulty, e.g. Calamity's Revengeance mode

If you pass in `null`, Munchies will use "classic" difficulty

#### 6: `LocalizedText` Extra tooltip
Use this arg to pass in any extra lines you want to add to the normal item tooltip. This will be added as a newline to the existing item tooltip.
`null` can be passed as well if no extra tooltip is needed

#### 7: `Func<bool>` Availability
Whether the item is able to be used in the current context. For example, the Demon Heart can only be used in expert mode, so `() => Main.expertMode` is passed here for the vanilla item.
If this value is false, the item text (and checkmark, if applicable) will be greyed out, but still display. This also prevents the completion checker from considering this item. `null` can be passed if it's always available

#### 8: `LocalizedText` Acquisition text
Use this arg to pass in text explaining how to acquire your item. This will be added as the first line of the the hover tooltip.
`null` can be passed as well if no acquisition text is needed

---

### Adding a Multi-Use Vanilla Consumable

#### 0: `string` "AddVanillaMultiUseConsumable"
This tells Munchies to use this particular call

#### 1: `string` Version
The version of the calls you want Munchies to use. Passing in the current version of Munchies is recommended. e.g. `"1.3"`. Do NOT use e.g. `munchiesMod.Version.ToString()`, that will not be backwards compatible.

#### 2: `int` Item ID
The ID of the vanilla item you wish to add

#### 3: `string` The category for your vanilla item
Acceptable strings: "player" OR "world" for whether the item applies to the player or the world

#### 4: `Func<int>` Current count
How many of this item have been consumed

#### 5: `Func<int>` Total count
The total number of uses this item has

#### 6: `string` Difficulty
Which difficulty this item is locked behind, e.g. expert only.

Acceptable strings: "classic", "expert", "master" -- "expert" and "master" add a colored tooltip and difficulty icon, but do not affect availability on their own

Alternatively, you can pass in your own difficulty string if you'd like to use your mod's custom difficulty, e.g. Calamity's Revengeance mode

If you pass in `null`, Munchies will use "classic" difficulty

#### 7: `LocalizedText` Extra tooltip
Use this arg to pass in any extra lines you want to add to the normal item tooltip. This will be added as a newline to the existing item tooltip.
`null` can be passed as well if no extra tooltip is needed

#### 8: `Func<bool>` Availability
Whether the item is able to be used in the current context. For example, the Demon Heart can only be used in expert mode, so `() => Main.expertMode` is passed here for the vanilla item.
If this value is false, the item text (and checkmark, if applicable) will be greyed out, but still display. This also prevents the completion checker from considering this item. `null` can be passed if it's always available

#### 9: `LocalizedText` Acquisition text
Use this arg to pass in text explaining how to acquire your item. This will be added as the first line of the the hover tooltip.
`null` can be passed as well if no acquisition text is needed

---

## Code example
```cs
public override void PostSetupContent() {
	try {
		if (ModLoader.TryGetMod("Munchies", out Mod munchiesMod)) {
			AddSingleModConsumable(munchiesMod);
			AddMultiModConsumable(munchiesMod);
			AddSingleVanillaConsumable(munchiesMod);
			AddMultiVanillaConsumable(munchiesMod);
		} else {
			Logger.Error("Error: couldn't find the Munchies mod");
		}
	} catch (Exception e) {
		Logger.Error($"PostSetupContent Error: {e.StackTrace} {e.Message}");
	}
}

private void AddSingleModConsumable(Mod munchiesMod) {
	object[] consumableArgs = {
		"AddSingleConsumable",
		MyMod, // reference to your Mod object
		"1.3", // version
		ModContent.GetInstance<MyModItem>(), // type is ModItem
		"player", // category
		() => consumed, // Func<bool>
		Color.Red, // custom text color (or null)
		"expert", // difficulty (or null) - this changes the tooltip text and adds a difficulty icon to show that this item is only available in expert mode. Does not affect availablility, this still needs to be set on its own
		null, // extra tooltip of type LocalizedText
		() => Main.expertMode, // availability, or null if always available
		null // acquisition text of type LocalizedText, or null if not necessary
	};
	munchiesMod.Call(consumableArgs);
}

private void AddMultiModConsumable(Mod munchiesMod) {
	object[] consumableArgs = {
		"AddMultiUseConsumable",
		MyMod, // reference to your Mod object
		"1.3", // version
		ModContent.GetInstance<MyModItem>(), // type is ModItem
		"player", // category
		() => MyModItem.currentCount, // Func<int>
		() => MyModItem.totalCount, // Func<int>
		Color.Red, // custom text color (or null)
		"expert", // difficulty (or null) - this changes the tooltip text and adds a difficulty icon to show that this item is only available in expert mode. Does not affect availablility, this still needs to be set on its own
		null, // extra tooltip of type LocalizedText
		() => Main.expertMode, // availability, or null if always available
		null // acquisition text of type LocalizedText, or null if not necessary
	};
	munchiesMod.Call(consumableArgs);
}

private void AddSingleVanillaConsumable(Mod munchiesMod) {
	object[] consumableArgs = {
		"AddVanillaConsumable",
		"1.3", // version
		ItemID.ArtisanLoaf, // item id
		"player", // category
		() => Main.LocalPlayer.ateArtisanBread, // Func<bool>
		"classic", // difficulty (or null)
		null, // extra tooltip of type LocalizedText
		() => true, // availability, or null if always available
		null // acquisition text of type LocalizedText, or null if not necessary
	};
	munchiesMod.Call(consumableArgs);
}

private void AddMultiVanillaConsumable(Mod munchiesMod) {
	object[] consumableArgs = {
		"AddVanillaMultiUseConsumable",
		"1.3", // version
		ItemID.LifeCrystal, // item id
		"player", // category
		() => Main.LocalPlayer.ConsumedLifeCrystals, // Func<int>
		() => 15, // Func<int>
		"classic", // difficulty (or null)
		null, // extra tooltip of type LocalizedText
		() => true, // availability, or null if always available
		null // acquisition text of type LocalizedText, or null if not necessary
	};
	munchiesMod.Call(consumableArgs);
}
```