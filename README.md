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

Acceptable strings: "classic", "expert", "master"

Alternatively, you can pass in your own difficulty string if you'd like to use your mod's custom difficulty, e.g. Calamity's Revengeance mode

If you pass in `null`, Munchies will use "classic" difficulty

#### 8: `LocalizedText` Extra tooltip
Use this arg to pass in any extra lines you want to add to the normal item tooltip. This will be added as a newline to the existing item tooltip.
`null` can be passed as well if no extra tooltip is needed

#### 9: `Func<bool>` Availability
Whether the item is able to be used in the current context. For example, the Demon Heart can only be used in expert mode, so `() => Main.expertMode` is passed here for the vanilla item.
If this value is false, the item text (and checkmark, if applicable) will be greyed out, but still display. This also prevents the completion checker from considering this item. `null` can be passed if it's always available

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

Acceptable strings: "classic", "expert", "master"

Alternatively, you can pass in your own difficulty string if you'd like to use your mod's custom difficulty, e.g. Calamity's Revengeance mode

If you pass in `null`, Munchies will use "classic" difficulty

#### 9: `LocalizedText` Extra tooltip
Use this arg to pass in any extra lines you want to add to the normal item tooltip. This will be added as a newline to the existing item tooltip.
`null` can be passed as well if no extra tooltip is needed

#### 10: `Func<bool>` Availability
Whether the item is able to be used in the current context. For example, the Demon Heart can only be used in expert mode, so `() => Main.expertMode` is passed here for the vanilla item.
If this value is false, the item text (and checkmark, if applicable) will be greyed out, but still display. This also prevents the completion checker from considering this item. `null` can be passed if it's always available

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

Acceptable strings: "classic", "expert", "master"

Alternatively, you can pass in your own difficulty string if you'd like to use your mod's custom difficulty, e.g. Calamity's Revengeance mode

If you pass in `null`, Munchies will use "classic" difficulty

#### 6: `LocalizedText` Extra tooltip
Use this arg to pass in any extra lines you want to add to the normal item tooltip. This will be added as a newline to the existing item tooltip.
`null` can be passed as well if no extra tooltip is needed

#### 7: `Func<bool>` Availability
Whether the item is able to be used in the current context. For example, the Demon Heart can only be used in expert mode, so `() => Main.expertMode` is passed here for the vanilla item.
If this value is false, the item text (and checkmark, if applicable) will be greyed out, but still display. This also prevents the completion checker from considering this item. `null` can be passed if it's always available

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

Acceptable strings: "classic", "expert", "master"

Alternatively, you can pass in your own difficulty string if you'd like to use your mod's custom difficulty, e.g. Calamity's Revengeance mode

If you pass in `null`, Munchies will use "classic" difficulty

#### 7: `LocalizedText` Extra tooltip
Use this arg to pass in any extra lines you want to add to the normal item tooltip. This will be added as a newline to the existing item tooltip.
`null` can be passed as well if no extra tooltip is needed

#### 8: `Func<bool>` Availability
Whether the item is able to be used in the current context. For example, the Demon Heart can only be used in expert mode, so `() => Main.expertMode` is passed here for the vanilla item.
If this value is false, the item text (and checkmark, if applicable) will be greyed out, but still display. This also prevents the completion checker from considering this item. `null` can be passed if it's always available

---

## Code example
```
public override void PostSetupContent() {
	try {
		if (ModLoader.TryGetMod("Munchies", out Mod munchiesMod)) {
			AddMyMod(munchiesMod);
			AddModConsumables(munchiesMod);
		} else {
			Logger.Error("Error: couldn't find the Munchies mod");
		}
	} catch (Exception e) {
		Logger.Error($"PostSetupContent Error: {e.StackTrace} {e.Message}");
	}
}

private void AddMyMod(Mod munchiesMod) {
	string[] args = {
		"AddMod",
		"<Mod name here>",
		"<Path to tab icon>"
	};
	munchiesMod.Call(args);
}

private void AddModConsumables(Mod munchiesMod) {
	object[] consumableArgs = {
		"AddConsumable",
		"<Mod name here>",
		"<Item name here>",
		"<Path to item icon>",
		"<Hover text here>",
		"<item category here>", // Options: player_normal, player_expert, world; Alternatively, can use a custom color such as Color.red (the color is not a string, do not add quotes)
		new Func<bool>(IsMyItemEnabled),
		"<item icon width>", // this is a string, representing pixels
		"<item icon height>" // this is a string, representing pixels
	};
	munchiesMod.Call(consumableArgs);

	static bool IsMyItemEnabled() {
		return <Code to get if the item is enabled>;
	}
}
```