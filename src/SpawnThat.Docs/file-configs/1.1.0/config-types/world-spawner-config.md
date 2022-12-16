# World Spawner Config

World spawner templates are managed through the `spawn_that.world_spawners_advanced.cfg` file.
There are currently 44 default templates, which can be overridden by setting a matching index (starting from 0).

The [general config](general-config.md) contains debugging options, which can be toggled to create a file containing all templates before the mod applies its changes, and after.

The format for a spawner entry is `[WorldSpawner.ID]`. The **ID** being any number from 0 and up. 

If the ID matches an existing entry, it will be modified. If unused, the config will be added as a new world spawner entry.

## Example

``` INI 
[WorldSpawner.0]
Name=deer
Enabled=True
Biomes=Meadows,BlackForest,
PrefabName=Deer
HuntPlayer=False
MaxSpawned=4
SpawnInterval=100
SpawnChance=50
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=0
SpawnDistance=64
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=3
GroupRadius=5
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=False
OceanDepthMin=0
OceanDepthMax=0
ConditionDistanceToCenterMin=0
ConditionDistanceToCenterMax=0
ConditionWorldAgeDaysMin=0
ConditionWorldAgeDaysMax=0

[WorldSpawner.123]
PrefabName=Boar
Name = Angry Boar
TemplateId=Boar_123
HuntPlayer = true

```

## Config Options 

| Setting | Type | Default | Example | Description |
| --- | --- | --- | --- | --- |
| Name | string | My spawner | Murder Dwarf 9000 | Just a field for naming the configuration template |
| TemplateId | string | | Murder_Dwarf_9000 | Technical setting intended for cross-mod identification of mobs spawned by this template. Sets a custom identifier which will be assigned to the spawned mobs ZDO as 'ZDO.Set(\"spawn_template_id\", TemplateIdentifier)'
| PrefabName | string | Deer | Greydwarf | Prefab name of the entity to spawn |
| Enabled | bool | true | false | Enable/disable this spawner entry |
| TemplateEnabled | bool | true | false | Enable/disable this configuration. This does not disable existing entries, just this configuration itself |
| Biomes | string | | Meadows, BlackForest | Biomes in which entity can spawn. Leave empty to allow all |
| HuntPlayer | bool | false | true | Sets AI to hunt a player target |
| MaxSpawned | int | 1 | 100 | Maximum number of prefab spawned in local surroundings |
| SpawnInterval | int | 90 | 300 | Seconds between new spawn checks. |
| SpawnChance | int | 100 | 0 | Chance to spawn pr spawn check. Range 0 to 100 |
| LevelMin | int | 1 | 35 | Minimum level to spawn |
| LevelMax | int | 1 | 3 | Maximum level to spawn |
| LevelUpChance | float | 10 | 45 | Chance to level up, starting at LevelMin and rolling again for each level gained. Range is 0 to 100 |
| LevelUpMinCenterDistance | float | 0 | 2000 | Minimum distance from world center, to allow higher than min level
| SpawnRadiusMin | float | 0 | 40 | Minimum spawn distance from player. 0 defaults to 40 | 
| SpawnRadiusMax | float | 0 | 80 | Maximum spawn distance from player. 0 defaults to 80 |
| SpawnDistance | float | 0 | 10 | Must not have another spawn of same prefab within this distance for this template to spawn |
| RequiredGlobalKey | string | | defeated_bonemass | Required global key to spawn |
| RequiredNotGlobalKey | string | | defeated_bonemass, KilledTroll | Global keys which disable the spawning of this template |
| RequiredEnvironments | string | | Misty, Thunderstorm | Require one of the listed environments to spawn. Leave empty to allow all |
| GroupSizeMin | int | 1 | 3 | Minimum count to spawn at a time. Group spawning | 
| GroupSizeMax | int | 1 | 10 | Maximum count to spawn at a time. Group spawning |
| GroupRadius | float | 3 | 10 | Size of circle to spawn group inside. A spot within SpawnRadiusMin and SpawnRadiusMax will be picked as center of this circle |
| GroundOffset | float | 0.5 | 5 | Offset to ground to spawn at. Negative means below ground, the higher the further into the sky |
| SpawnDuringDay | bool | true | false | Toggles spawning at day. Will also cause despawning for creatures at day if false |
| SpawnDuringNight | bool | true | false | Toggles spawning during night | 
| SpawnInForest | bool | true | false | Toggles spawning in forests |
| SpawnOutsideForest | bool | true | false | Toggles spawning outside forests |
| OceanDepthMin | float | 0 | 0 | Minimum ocean depth to spawn in. Ignored if min == max |
| OceanDepthMax | float | 0 | 0 | Maximum ocean depth to spawn in. Ignored if min == max |
| BiomeArea | string | Everything | Edge | Toggles spawning in biome border zones (Edge), non-border zones (Median) or both (Everything). A border zone is a zone where two or more biomes are next to each other |
| ConditionAltitudeMin | float | -1000 | 123 | Minimum altitude (distance to water surface) to spawn at |
| ConditionAltitudeMax | float | -1000 | 123 | Maximum altitude (distance to water surface) to spawn at |
| ConditionTiltMin | float | 0 | 120 | Minimum tilt of terrain surface to spawn at |
| ConditionTiltMax | float | 35 | 45 | Maximum tilt of terrain surface to spawn at |
| ConditionDistanceToCenterMin | float | 0 | 1000 | Minimum distance to center to spawn |
| ConditionDistanceToCenterMax | float | 0 | 5000 | Maximum distance to center to spawn. 0 means limitless |
| ConditionWorldAgeDaysMin | float | 0 | 12 | Minimum world age in in-game days to spawn |
| ConditionWorldAgeDaysMax | float | 0 | 35 | Maximum world age in in-game days. 0 means no max |
| DistanceToTriggerPlayerConditions | float | 100 | 55 | Distance of player to spawner, for player to be included in player based checks such as ConditionNearbyPlayersCarryValue |
| ConditionNearbyPlayersCarryValue | float | 0 | 100 | Checks if nearby players have a combined value in inventory above this condition. Eg. If set to 100, entry will only activate if nearby players have more than 100 worth of values combined |
| ConditionNearbyPlayerCarriesItem | string | | IronScrap, DragonEgg | Checks if any nearby player have any of the listed item prefab names in inventory |
| ConditionNearbyPlayersNoiseThreshold | float | 0 | 80 | Checks if any nearby player has a noise level at or above the threshold |
| ConditionNearbyPlayersStatus | string | | Wet, Burning | Checks if any nearby player has one of the listed status effects |
| SetFaction | string | | Undead | Assign a specific faction to spawn. If empty uses default of prefab |
| SetRelentless | bool | false | true | When true, forces mob AI to always be alerted |
| SetTryDespawnOnConditionsInvalid | bool | false | true | When true, mob will try to run away and despawn when spawn conditions become invalid. Eg. if spawning only during night, it will run away and despawn at night. Currently this only take into account conditions for daytime and environment |
| SetTryDespawnOnAlert | bool | false | true | When true, mob will try to run away and despawn when alerted |
| SetTamed | bool | false | true | When true, mob will be set to tamed status on spawn. Only possible if mob can be tamed |
| SetTamedCommandable | bool | false | true | Experimental. When true, will set mob as commandable when tamed. When false, whatever was default for the creature is used. Does not always seem to work for creatures not tameable in vanilla |
| ConditionLocation | string | | Runestone_Boars, FireHole | Locations in which this template is enabled. Leave empty for all |
| ConditionAreaSpawnChance | float | 100 | 1.5 | Chance for spawn to spawn at all in the area. The chance will be rolled once for the area. Range is 0 to 100. Eg. if a whole area of BlackForest rolls higher than the indicated chance, this spawn template will never be active in that forest. Another BlackForest will have another roll however, that may activate this template there. Chance is rolled based on world seed, area id and template index |
| ConditionAreaIds | string | | 1, 123, 543 | Advanced feature. List of area id's in which the template is valid. Note: If ConditionAreaSpawnChance is not 100 or disabled, it will still roll area chance |

## Supplemental

Spawn That will load additional configurations from configs with names following the pattern `spawn_that.world_spawners.*.cfg`

This allows for adding your own custom templates to Spawn That, or simply separate your configs into more manageable pieces.
Eg. `spawn_that.world_spawners.my_custom_configuration.cfg`

The configurations loaded will be merged with the one loaded from the main files.

Merging may override previously loaded spawner templates.

The `spawn_that.world_spawners_advanced.cfg` is always loaded last, and will therefore override anything it overlaps with. This is to allow mods to use supplemental files for their configs, but let the user have a single spot to add their own overrides.
