----

⚠️ This is an archived version of the documentation. Find the latest version [here](/configs/general/intro.html) ⚠️

----

# SpawnArea Spawner Config

SpawnArea spawner templates are managed through the `spawn_that.spawnarea_spawners.cfg` file.

This type of spawners covers draugr piles, greydwarf nests, and such (listed as [Creature Spawner](https://valheim.fandom.com/wiki/Creature_spawner) in the wiki). SpawnArea is the name used internally in Valheim code for these.

They are the spawners that continuously spawn creatures, until a certain limit has been reached, by selecting weighted-randomly from a list of prefabs.

Each spawner is therefore split into two parts. The main spawner itself, and the individual spawns that it can spawn.

Since SpawnArea spawners can exist anywhere and are not bound to any easily structured hierarchy, configurations must specify a set of criteria. These are known as **Identifiers**.

If two sets of configurations have overlapping identifiers, using the same values, Spawn That will merge them.

If two sets of configurations match the same spawner, the one with most identifiers will be selected.
In case they have a matching number of identifiers, it becomes a bit more complicated, and Spawn That will attempt to calculate which set of identifiers is the most specific, and use that.
Eg. location gets selected over biome.

The format for a spawner config is `[Name]`. The **Name** is not important, and is simply used to identify this config.

Spawns are specified by `[Name.ID]`. Where **Name** must match the spawner, and **ID** is a number from 0 and up, matching either an existing spawn to override, or if an unused ID, a new spawn is added to the spawner.

Limitations:
- Can only override existing SpawnArea entities.
- SpawnArea counts numbers of nearby creatures to find its limits, it has no concept of what those creatures are, or if you want it to count something else. It can only count creatures.

## Example

```toml
[MeadowsGreydwarves]
# Only target SpawnArea's in Meadows biome.
# And only if that SpawnArea prefab is named Spawner_GreydwarfNest
IdentifyByBiome = Meadows
IdentifyByName = Spawner_GreydwarfNest

# Change spawner settings
SpawnInterval = 1

# Change existing greydwarf spawn, by picking a used ID (0) and swapping to a troll instead
[MeadowsGreydwarves.0]
PrefabName = Troll

# Add a new spawn to the SpawnArea, by picking an unused ID.
[MeadowsGreydwarves.100]
PrefabName = Draugr_Ranged
SpawnWeight = 5
LevelMin = 3
LevelMax = 3
```

## Spawner Config Options

| Setting | Type | Default | Example | Description |
| --- | --- | --- | --- | --- |
| IdentifyByName | string | | Spawner_GreydwarfNest, Spawner_DraugrPile | List of spawner prefab names to enable config for. Ignored if empty |
| IdentifyByBiome | string | | Meadows, Mountain | List of biomes to enable config in. Ignored if empty |
| IdentifyByLocation | string | | StartTemple, StoneTowerRuins04 | List of Locations to enable config in Ignored if empty |
| IdentifyByRoom | string | | sunkencrypt_Corridor3, meadowsvillage_greathall | List of rooms (eg., dungeons, camp buildings) to enable config in. Ignored if empty |
| LevelUpChance | float | 15 | 35.6 | Chance to level up from MinLevel. Range 0 to 100 |
| SpawnInterval | float | 10 | 12.5 | Seconds between spawn checks |
| SetPatrol | bool | false | Sets if spawn should patrol its spawn point |
| ConditionPlayerWithinDistance | float | 60 | 20.5 | Minimum distance to player for enabling spawn |
| ConditionMaxCloseCreatures | integer | 3 | 15 | Sets maximum number of creatures within DistanceConsideredClose, for spawner to be active |
| ConditionMaxCreatures | integer | 100 | 10 | Sets maximum number of creatures currently loaded, for spawner to be active
| DistanceConsideredClose | float | 20 | 25.5 | Distance within which another entity is counted as being close to spawner |
| DistanceConsideredFar | float | 1000 | 100.1 | Distance within which another entity is counted as being far to spawner |
| OnGroundOnly | bool | false | true | Only spawn if spawn point is on the ground (ie., not in a dungeon) and open to the sky |
| RemoveNotConfiguredSpawns | bool | false | true | Sets if spawns of spawner that were not configured should be removed

## Spawn Config Options

| Setting | Type | Default | Example | Description |
| --- | --- | --- | --- | --- |
| Enabled | bool | true | false | Toggles this template. If disabled, this spawn entry will never be selected for spawning. Can be used to disable existing spawn entries |
| TemplateEnabled | bool | true | false | Toggles this configuration on / off. If disabled, template will be ignored. Cannot be used to disable existing spawn entries |
| PrefabName | string | | Bat | Prefab name of entity to spawn |
| SpawnWeight | float | 1 | 1.5 | Sets spawn weight. SpawnArea spawners choose their next spawn by a weighted random of all their possible spawns. Increasing weight means an increased chance that this particular spawn will be selected for spawning |
| LevelMin | integer | 1 | 3 | Minimum level to spawn at |
| LevelMax | integer | 1 | 5 | Maximum level to spawn at |
| ConditionDistanceToCenterMin | float | 0 | 555.5 | Minimum distance to center for configuration to apply |
| ConditionDistanceToCenterMax | float | 0 | 700.3 | Maximum distance to center for configuration to apply. 0 means limitless |
| ConditionWorldAgeDaysMin | integer | 0 | 15 | Minimum world age in in-game days for this configuration to apply |
| ConditionWorldAgeDaysMax | integer | 0 | 100 | Maximum world age in in-game days for this configuration to apply. 0 means no max |
| DistanceToTriggerPlayerConditions | float | 100 | 50.5 | Distance of player to spawner, for player to be included in player based checks such as ConditionNearbyPlayersCarryValue | 
| ConditionNearbyPlayersCarryValue | integer | 0 | 275 | Checks if nearby players have a combined value in inventory above this condition. Eg. If set to 100, entry will only activate if nearby players have more than 100 worth of values combined |
| ConditionNearbyPlayerCarriesItem | string | | IronScrap, DragonEgg | Checks if nearby players have any of the listed item prefab names in inventory  |
| ConditionNearbyPlayersNoiseThreshold | float | 0 | 13.5 | Checks if any nearby players have accumulated noise at or above the threshold |
| ConditionNearbyPlayersStatus | string | | Wet, Burning | Checks if any nearbly players have any of the listed status effects |
| ConditionAreaSpawnChance | float | 100 | 0.6 | Chance for spawn to spawn at all in the area. The chance will be rolled once for the area. Range is 0 to 100. Eg. if a whole area of BlackForest rolls higher than the indicated chance, this spawn template will never be active in that forest. Another BlackForest will have another roll however, that may activate this template there. Chance is rolled based on world seed, area id and template index |
| ConditionLocation | string | | Runestone_Boars, FireHole | List of locations in which this spawn is enabled. If empty, allows all. See [Locations](../data/locations-by-biome.md) |
| ConditionAreaIds | integer | | 1, 123, 543 | Advanced feature. List of area id's in which the template is valid. Note: If ConditionSpawnChanceInArea is not 100 or disabled, it will still roll area chance |
| ConditionBiome | string | | Meadows, Mountain | Biomes in which entity can spawn. Leave empty for all. See [Biomes](../field-options/field-options.md#biomes)) |
| ConditionAllOfGlobalKeys | string | | defeated_eikthyr, KilledTroll | Global keys required to allow spawning. All listed keys must be present. Ignored if empty. See [Global Keys](../field-options/field-options.md#global-keys) |
| ConditionAnyOfGlobalKeys | string | | KilledBat, killed_surtling | Global keys allowing spawning. One of the listed keys must be present. Ignored if empty. See [Global Keys](../field-options/field-options.md#global-keys) |
| ConditionNoneOfGlobalKeys | string | | defeated_goblinking, defeated_dragon | Global keys disabling spawning. None of the listed keys must be present. Ignored if empty. See [Global Keys](../field-options/field-options.md#global-keys) |
| ConditionEnvironment | string | | Misty, Thunderstorm | List of environments required to allow spawning. Leave empty to allow all. See [Environment](../field-options/field-options.md#environments) |
| ConditionDaytime | string | | Afternoon, Night | Toggles period in which spawning is active. See [Daytime](../field-options/field-options.md#daytime) |
| ConditionAltitudeMin | float | -1000 | 0.5 | Minimum altitude (distance above water surface) to spawn in |
| ConditionAltitudeMax | float | 10000 | 0.1 | Maximum altitude (distance above water surface) to spawn in |
| ConditionForestState | string | Both | InForest | Toggles spawning when inside the specified state of forestation. Note that the forestation is based on world generation | 
| ConditionOceanDepthMin | float | 0 | 10 | Minimum ocean depth (distance below water surface) to spawn in. Ignored if min == max |
| ConditionOceanDepthMax | float | 0 | 20 | Maximum ocean depth (distance below water surface) to spawn in. Ignored if min == max |
| SetFaction | string | | Demon | Assign a specific faction to spawn. If empty uses default |
| SetRelentless | bool | false | true | When true, forces mob AI to always be alerted |
| SetTryDespawnOnAlert | bool | false | true | When true, mob will try to run away and despawn when alerted |
| SetTamed | bool | false | true | When true, mob will be set to tamed status on spawn |
| SetTamedCommandable | bool | false | true | Experimental. When true, will set mob as commandable when tamed. When false, whatever was default for the creature is used. Does not always seem to work for creatures not tameable in vanilla |
| SetHuntPlayer | bool | false | true | Sets AI to hunt a player target |
| ConditionPositionMustBeNearAllPrefabs | string | | FineWood, Blueberries | List of prefab names which must all be present with distance of spawn position to allow spawning. Leave empty to always allow |
| ConditionPositionMustBeNearAllPrefabsDistance | int | 32 | 123 | Distance within attempted spawn position to check for prefabs listed in ConditionPositionMustBeNearAllPrefabs |
| ConditionPositionMustBeNearPrefabs | string | | FineWood, Blueberries | List of prefab names for which one or more must be present with distance of spawn position to allow spawning. Leave empty to always allow |
| ConditionPositionMustBeNearPrefabsDistance | int | 32 | 123 | Distance within attempted spawn position to check for prefabs listed in ConditionPositionMustBeNearPrefabs |
| ConditionPositionMustNotBeNearPrefabs | string | | FineWood, Blueberries | List of prefab names for which none must be present with distance of spawn position to allow spawning. Leave empty to always allow |
| ConditionPositionMustNotBeNearPrefabsDistance | int | 32 | 123 | Distance within attempted spawn position to check for prefabs listed in ConditionPositionMustNotBeNearPrefabs |

## Supplemental

Spawn That will load additional configurations from configs with names following the pattern `spawn_that.spawnarea_spawners.*.cfg`, where the `*` can be anything.

This allows for adding your own custom templates to Spawn That, or simply separate your configs into more manageable pieces.
Eg. `spawn_that.spawnarea_spawners.my_custom_configuration.cfg`

The configurations loaded will be merged with the one loaded from the main files.

Merging may override previously loaded spawner templates.

The `spawn_that.spawnarea_spawners.cfg` is always loaded last, and will therefore override anything it overlaps with. This is to allow mods to use supplemental files for their configs, but let the user have a single spot to add their own overrides.