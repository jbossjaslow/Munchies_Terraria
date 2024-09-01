# Munchies_Terraria
 
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

#### 4: `string` OR `Color` The category for your mod item
Acceptable strings: "player" OR "world" for whether the item applies to the player or the world

Alternatively, you can pass in a Color object here to display a custom color instead

#### 5: `Func<bool>` Has been consumed
Whether the item has been consumed or not. This will be triggered to know whether to place a checkmark next to the item in the list.

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

### Adding a Multi-Use Consumable

#### 0: `string` "AddMultiUseConsumable"
This tells Munchies to use this particular call

#### 1: `Mod` Your mod
This is the reference to your mod

#### 2: `string` Version
The version of the calls you want Munchies to use. Passing in the current version of Munchies is recommended. e.g. `"1.3"`. Do NOT use e.g. `munchiesMod.Version.ToString()`, that will not be backwards compatible.

#### 3: `ModItem` Your mod item
The reference to the item you want to add. This allows it to encapsulate several pieces of info like the texture, tooltip, etc.

#### 4: `string` OR `Color` The category for your mod item
Acceptable strings: "player" OR "world" for whether the item applies to the player or the world

Alternatively, you can pass in a Color object here to display a custom color instead

#### 5: `Func<int>` Current count
How many of this item have been consumed. If it's a single use item, pass `() => <item.hasBeenConsumed>.ToInt()`

#### 6: `Func<int>` Total count
The total number of uses this item has. For single use items, pass `() => 1`, or use "AddSingleConsumable" call instead

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

### Adding a Vanilla Consumable

#### 0: `string` "AddVanillaConsumable"
This tells Munchies to use this particular call

#### 1: `string` Version
The version of the calls you want Munchies to use. Passing in the current version of Munchies is recommended. e.g. `"1.3"`. Do NOT use e.g. `munchiesMod.Version.ToString()`, that will not be backwards compatible.

#### 2: `int` Item ID
The ID of the vanilla item you wish to add

#### 3: `string` OR `Color` The category for your mod item
Acceptable strings: "player" OR "world" for whether the item applies to the player or the world

Alternatively, you can pass in a Color object here to display a custom color instead

#### 4: `Func<int>` Current count
How many of this item have been consumed. If it's a single use item, pass `() => <item.hasBeenConsumed>.ToInt()`

#### 5: `Func<int>` Total count
The total number of uses this item has. For single use items, pass `() => 1`

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
