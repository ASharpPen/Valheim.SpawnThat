# Spawn That! 

This is an advanced tool for configuring all world spawners.

With this, it is possible to change almost all of the default settings for the way spawners work in Valheim.
Want to have a world without trolls? Possible! (probably)
Want to have a world with ONLY trolls? Possible! (almost)
Want to have a world where greydwarves only spawn at night? Possible!
Just want to have more/less of a mob type? Simple modifiers exist!

## Features

- Change spawning rates of specific mobs
- Replace existing spawn configurations throughout the world
- Set almost any of the default parameters the game uses
- Add your own spawn configuration to the world
- Modify the localized spawners by mob type and location
- Dump existing game templates as files using the same format as the mod configs. 
	- Easy to copy-paste and change the parts you want.
	- Investigate what the world throws at you.
- Server-side configs
- Modify the spawners in camps, villages and dungeons
- Support for [Creature Level and Loot Control](https://valheim.thunderstore.io/package/Smoothbrain/CreatureLevelAndLootControl/)
- Support for [MobAILib](https://www.nexusmods.com/valheim/mods/1188)

## FAQ

Why are the config files empty?
- All configs but spawn_that.cfg and spawn_that.simple.cfg are intended empty initially. This is so that the game will behave as normal, until you start adding configurations to it.
- Enable any of the "WriteX" in spawn_that.cfg, and files containing the default values will be created in the <GameDirectory>/BepInEx/plugins folder.
	- I also have a pre-generated version [here](https://gist.github.com/ASharpPen/fa142b8aed0205b4e4c059644c58c2cf), if you just cannot find the files.

Where do I find the prefab names?
- Multiple pages have long lists, you can check out this one [here](https://gist.github.com/Sonata26/e2b85d53e125fb40081b18e2aee6d584), or the [valheim wiki](https://valheim.fandom.com/wiki/Creatures)

## How does it all work?

Valheim's main way of managing spawns work by having two types of spawners spread throughout the world.
The world spawners, for which the world has them spread out in a grid fashion, each using the same list of templates to check if something should spawn. These are the general spawners and all currently use the same 44 templates.
The local spawners, which are intended for fine-tuned spawning. Local spawners only spawn one specific mob type, and only has one alive at a time. These are bound to specific world locations, such as the surtling firehole.

For world spawners, you can either replace existing templates based on their index, or add to the list of possible templates to spawn from.

For local spawners, since these are more custom, you describe a location and the prefab name of the mob you what you want to override.

The mod modification happens at run-time, once for each spawner. Reloading the world resets all changes.
As the player moves through the world, the game loads in the various spawners, and the mod applies its own settings.

## Client / Server

Spawn That needs to be installed on all clients (and server) to work.

From v0.3.0 clients will request the configurations currently loaded by the server, and use those without affecting the clients config files.
This means you should be able to have server-specific configurations, and the client can have its own setup for singleplayer.
For this to work, the mod needs to be installed on the server, and have configs set up properly there. When players join with Spawn That v0.3.0, their mod will use the servers configs.

# Simple spawning

All of this might be more complicated than what you need or want. Therefore, "spawn_that.simple.cfg" exists to provide simpler modifiers to world spawner mobs.
These will simply scale the number of mobs up or down.

Be aware, these will be applied after any other configurations to world spawners have been set. 
Meaning if you have 10 times spawning in your "spawn_that.world_spawners_advanced.cfg", and the same in the simple config, you are going to end up with 100x spawning.

``` INI

[YourUniqueNameHere]

## Prefab name of entity to modify.
# Setting type: String
PrefabName = Greydwarf

## Enable/Disable this set of modifiers.
# Setting type: Boolean
Enable = true

## Change maximum of total spawned entities. 2 means twice as many.
# Setting type: Single
SpawnMaxMultiplier = 1

## Change min number of entities that will spawn at once. 2 means twice as many.
# Setting type: Single
GroupSizeMinMultiplier = 1

## Change max number of entities that will spawn at once. 2 means twice as many.
# Setting type: Single
GroupSizeMaxMultiplier = 1

## Change how often the game will try to spawn in new creatures.
## Higher means more often. 2 is twice as often, 0.5 is double the time between spawn checks.
# Setting type: Single
SpawnFrequencyMultiplier = 1

```

# World Spawners

World spawner templates are managed through the "spawn_that.world_spawners_advanced.cfg" file.
There are currently 44 default templates, which can be overriden by setting a matching index (starting from 0).

The general config contains debugging options, which can be toggled to create a file containing all templates before the mod applies its changes, and after.

## Example:

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

```

# Local spawners

Local spawners are customly set up in the world, based on pre-defined location setups. This means they are each customized to the location at which they are placed, and are not as easily "targetted" as the world spawners templates are. 
To modify a local spawner, you must specify a location and mob prefab name you want to apply a configuration to.
You can also use a room name, if more fine control is necessary for villages, dungeons and camps.

Every combination of Location and PrefabName must be unique.

The general config contains debugging options, which can be toggled to create a file containing all default local spawner configs before the mod applies its changes.

## Example

Replaces all boars usually spawning at boar runestones, with trolls.

``` INI

[Runestone_Boars.Boar]
PrefabName=Troll
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=3
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

```

## Example 2

Replaces spawning of blobs in a specific dungeon room with a treasure chest that can respawn every 60 minutes (need to be destroyed for a new one to spawn).

``` INI
[sunkencrypt_Corridor3.Blob]
PrefabName = TreasureChest_sunkencrypt
RespawnTime = 60
```

# Supplemental

Spawn That will load additional configurations from configs with names prefixed with "spawn_that.world_spawners." and "spawn_that.local_spawners.".

This allows for adding your own custom templates to Spawn That, or simply separate your configs into more manageable pieces.
Eg. "spawn_that.world_spawners.my_custom_configuration.cfg" and "spawn_that.local_spawners.my_custom_configuration.cfg"

The configurations loaded will be merged with the one loaded from the main files.

# Console Commands

Additional console commands are added for debugging purposes.

```
spawnthat room - prints if in a dungeon room and which one
spawnthat area - prints the area id of the players current location
spawnthat arearoll <index> - prints the rolled chance for a template, in area player is currently in
spawnthat arearoll <index> <x> <y> - prints the rolled chance for a template, in the area with indicated coordinates
spawnthat arearollheatmap <index> - prints a png map of area rolls for a template to disk.
spawnthat wheredoesitspawn <index> - prints a png map of areas in which the world spawner template with <index> spawns to disk.
```

All the indexes mentioned refer to the number used in your WorldSpawn template.

Eg. 
To print a png of where `[WorldSpawner.321]` is allowed to spawn, write the command:
```
spawnthat wheredoesitspawn 321
```

# World Spawners - Details

``` INI

[WorldSpawner.0]

## Technical setting intended for cross-mod identification of mobs spawned by this template. Sets a custom identifier which will be assigned to the spawned mobs ZDO as 'ZDO.Set("spawn_template_id", TemplateIdentifier)'.
# Setting type: String
TemplateId = 

## Just a field for naming the configuration entry.
# Setting type: String
Name = deer

## Enable/disable this entry.
# Setting type: Boolean
Enabled = true

## Biomes in which entity can spawn. Leave empty for all.
# Setting type: String
Biomes = Meadows,BlackForest,

## Prefab name of the entity to spawn.
# Setting type: String
PrefabName = Deer

## Sets AI to hunt a player target.
# Setting type: Boolean
HuntPlayer = false

## Assign a specific faction to spawn. If empty uses default.
SetFaction = 

## Maximum entities of type spawned in area.
# Setting type: Int32
MaxSpawned = 4

## Seconds between spawn checks.
# Setting type: Single
SpawnInterval = 100

## Chance to spawn per check. Range 0 to 100.
# Setting type: Single
SpawnChance = 50

## Minimum level to spawn.
# Setting type: Int32
LevelMin = 1

## Maximum level to spawn.
# Setting type: Int32
LevelMax = 3

## Minimum distance from world center, to allow higher than min level.
# Setting type: Single
LevelUpMinCenterDistance = 0

## Minimum distance to another entity.
# Setting type: Single
SpawnDistance = 64

## Minimum spawn radius.
# Setting type: Single
SpawnRadiusMin = 0

## Maximum spawn radius.
# Setting type: Single
SpawnRadiusMax = 0

## Required global key to spawn.	Eg. defeated_bonemass
# Setting type: String
RequiredGlobalKey = 

## Array of global keys which disable the spawning of this entity if any are detected.
# Setting type: String
RequiredNotGlobalKey = 

## Array (separate by comma) of environments required to spawn in.	Eg. Misty, Thunderstorm. Leave empty to allow all.
# Setting type: String
RequiredEnvironments = 

## Minimum count to spawn at per check.
# Setting type: Int32
GroupSizeMin = 1

## Maximum count to spawn at per check.
# Setting type: Int32
GroupSizeMax = 3

## Size of circle to spawn group inside.
# Setting type: Single
GroupRadius = 5

## Offset to ground to spawn at.
# Setting type: Single
GroundOffset = 0.5

## Toggles spawning at day.
# Setting type: Boolean
SpawnDuringDay = true

## Toggles spawning at night.
# Setting type: Boolean
SpawnDuringNight = true

## Minimum altitude (distance to water surface) to spawn in.
# Setting type: Single
ConditionAltitudeMin = 0

## Maximum altitude (distance to water surface) to spawn in.
# Setting type: Single
ConditionAltitudeMax = 1000

## Minium tilt of terrain to spawn in.
# Setting type: Single
ConditionTiltMin = 0

## Maximum tilt of terrain to spawn in.
# Setting type: Single
ConditionTiltMax = 35

## Toggles spawning in forest.
# Setting type: Boolean
SpawnInForest = true

## Toggles spawning outside of forest.
# Setting type: Boolean
SpawnOutsideForest = false

## Minimum ocean depth to spawn in. Ignored if min == max.
# Setting type: Single
OceanDepthMin = 0

## Maximum ocean depth to spawn in. Ignored if min == max.
# Setting type: Single
OceanDepthMax = 0

## Minimum distance to center for configuration to apply.
# Setting type: Single
ConditionDistanceToCenterMin = 0

## Maximum distance to center for configuration to apply. 0 means limitless.
# Setting type: Single
ConditionDistanceToCenterMax = 0

## Minimum world age in in-game days for this configuration to apply.
# Setting type: Single
ConditionWorldAgeDaysMin = 0

## Maximum world age in in-game days for this configuration to apply. 0 means no max.
# Setting type: Single
ConditionWorldAgeDaysMax = 0

## Distance of player to spawner, for player to be included in player based checks such as ConditionNearbyPlayersCarryValue.
# Setting type: Single
DistanceToTriggerPlayerConditions = 100

## Checks if nearby players have a combined value in inventory above this condition.
## Eg. If set to 100, entry will only activate if nearby players have more than 100 worth of values combined.
# Setting type: Int32
ConditionNearbyPlayersCarryValue = 0

## Checks if nearby players have any of the listed item prefab names in inventory.
## Eg. IronScrap, DragonEgg
# Setting type: String
ConditionNearbyPlayerCarriesItem = 

## When true, forces mob AI to always be alerted.
# Setting type: Boolean
SetRelentless = false

## When true, mob will try to run away and despawn when spawn conditions become invalid. Eg. if spawning only during night, it will run away and despawn at night. Currently this only take into account conditions for daytime and environment.
# Setting type: Boolean
SetTryDespawnOnConditionsInvalid = false

## When true, mob will try to run away and despawn when alerted.
# Setting type: Boolean
SetTryDespawnOnAlert = false

## Checks if any nearby players have accumulated noise at or above the threshold.
# Setting type: Single
ConditionNearbyPlayersNoiseThreshold = 0

## When true, mob will be set to tamed status on spawn.
# Setting type: Boolean
SetTamed =  false

## Experimental. When true, will set mob as commandable when tamed. When false, whatever was default for the creature is used. Does not always seem to work for creatures not tameable in vanilla.
# Setting type: Boolean
SetTamedCommandable = false

## Checks if any nearbly players have any of the listed status effects.
## Eg. Wet, Burning
# Setting type: String
ConditionNearbyPlayersStatus =

## Chance for spawn to spawn at all in the area. The chance will be rolled once for the area. Range is 0 to 100. Eg. if a whole area of BlackForest rolls higher than the indicated chance, this spawn template will never be active in that forest. Another BlackForest will have another roll however, that may activate this template there. Chance is rolled based on world seed, area id and template index.
# Setting type: Single
ConditionAreaSpawnChance = 100

## Advanced feature. List of area id's in which the template is valid. Note: If ConditionSpawnChanceInArea is not 100 or disabled, it will still roll area chance.
## Eg. 1, 123, 543
# Setting type: String
ConditionAreaIds =

```

# Local Spawners - Details

``` INI

[Location.PrefabName]

## PrefabName of entity to spawn.
# Setting type: String
PrefabName = Troll

## Enable/disable this configuration.
# Setting type: Boolean
Enabled = true

## Enable spawning during day.
# Setting type: Boolean
SpawnAtDay = true

## Enable spawning during night.
# Setting type: Boolean
SpawnAtNight = true

## Minimum level of spawn.
# Setting type: Int32
LevelMin = 1

## Maximum level of spawn.
# Setting type: Int32
LevelMax = 1

## Chance to level up, starting at minimum level and rolling again for each level gained. Range is 0 to 100.
# Setting type: Single
LevelUpChance = 10

## Minutes between checks for respawn. Only one mob can be spawned at time per spawner.
# Setting type: Single
RespawnTime = 20

## Distance to trigger spawning.
# Setting type: Single
TriggerDistance = 60

## If not 0, adds a minimum noise required for spawning, on top of distance requirement.
# Setting type: Single
TriggerNoise = 0

## Allow spawning inside player base boundaries.
# Setting type: Boolean
SpawnInPlayerBase = true

## Sets position of spawn as patrol point.
# Setting type: Boolean
SetPatrolPoint = true

## Assign a specific faction to spawn. If empty uses default.
# Setting type: String
SetFaction = 

## When true, mob will be set to tamed status on spawn.
# Setting type: Boolean
SetTamed =  false

## Experimental. When true, will set mob as commandable when tamed. When false, whatever was default for the creature is used. Does not always seem to work for creatures not tameable in vanilla.
# Setting type: Boolean
SetTamedCommandable = false

```

# Mod specific configuration

These are implemented soft-dependant, meaning if the mod is not present, the configuration will simply do nothing.

## MobAILib

Options for setting [MobAILib](https://www.nexusmods.com/valheim/mods/1188) ai's and configuration. See the mod page for more in-depth documentation of the options. By default, only the built-in AI's will be available, but should support any customly registered by other mods.

Note, MobAI will most likely completely take over any AI related features, so don't expect things like SetTryDespawnOnAlert to work when assigning a custom ai.

### World Spawners

Mod-specific configs can be added to each world spawner as `[WorldSpawner.Index.MobAI]`

``` INI
## Name of MobAI to register for spawn. Eg. the defaults 'Fixer' and 'Worker'.
SetAI = 

## Configuration file to use for the SetAI. Eg. 'MyFixerConfig.json', can include path, but will always start searching from config folder. See MobAI documentation for file setup.
AIConfigFile = 
``` 

Example of a repair boar.

```
[WorldSpawner.321]
Name = My fixing boar
PrefabName = Boar

[WorldSpawner.321.MobAI]
SetAI = Fixer
AIConfigFile=MyFixerConfig.json
```

### Local Spawners

Mod-specific configs can be added to each local spawner as `[Location.PrefabName.MobAI]`

``` INI
## Name of MobAI to register for spawn. Eg. the defaults 'Fixer' and 'Worker'.
SetAI = 

## Configuration file to use for the SetAI. Eg. 'MyFixerConfig.json', can include path, but will always start searching from config folder. See MobAI documentation for file setup.
AIConfigFile = 
``` 

Example of a boar repairman spawning at boar runestones.

```
[Runestone_Boars.Boar]
Name = Repair Boar
PrefabName = Boar

[Runestone_Boars.Boar.MobAI]
SetAI = Fixer
AIConfigFile=MyFixerConfig.json
```


## Creature Level and Loot Control

Additional options for [Creature Level and Loot Control](https://valheim.thunderstore.io/package/Smoothbrain/CreatureLevelAndLootControl/).
See the mod page for more in-depth documentation for the options.

### World Spawners

Mod-specific configs can be added to each world spawner as `[WorldSpawner.Index.CreatureLevelAndLootControl]`

``` INI

## Minimum CLLC world level for spawn to activate. Negative value disables this condition.
ConditionWorldLevelMin = -1

## Maximum CLLC world level for spawn to active. Negative value disables this condition.
ConditionWorldLevelMax = -1

## Assigns the specified infusion to creature spawned. Ignored if empty.
SetInfusion = Fire

## Assigns the specified effect to creature spawned. Ignored if empty.
SetExtraEffect = 

## Assigns the specified boss affix to creature spawned. Only works for the default 5 bosses. Ignored if empty.
SetBossAffix = 

## Use the default LevelMin and LevelMax for level assignment, ignoring the usual CLLC level control.
UseDefaultLevels = true

```

Example of a world spawner template, for a boar that will spawn with a fire infusion, when the CLLC world level is high enough.

``` INI

[WorldSpawner.1]
Name = FireBoar
PrefabName = Boar

[WorldSpawner.1.CreatureLevelAndLootControl]
ConditionWorldLevelMin = 3
SetInfusion = Fire

```

### Local Spawners

Mod-specific configs can be added to each local spawner as `[Location.PrefabName.CreatureLevelAndLootControl]`

``` INI

## Minimum CLLC world level for spawn to activate. Negative value disables this condition.
ConditionWorldLevelMin = -1

## Maximum CLLC world level for spawn to active. Negative value disables this condition.
ConditionWorldLevelMax = -1

## Assigns the specified infusion to creature spawned. Ignored if empty.
SetInfusion = Fire

## Assigns the specified effect to creature spawned. Ignored if empty.
SetExtraEffect = 

## Assigns the specified boss affix to creature spawned. Only works for the default 5 bosses. Ignored if empty.
SetBossAffix = 

## Use the default LevelMin and LevelMax for level assignment, ignoring the usual CLLC level control.
UseDefaultLevels = true

```

Example of local spawners around the boar runestone, which will always spawn with a fire infusion.

``` INI
[Runestone_Boars.Boar]
PrefabName = Boar
Enabled = true

[Runestone_Boars.Boar.CreatureLevelAndLootControl]
SetInfusion = Fire

```

### Boss Affixes
- None
- Reflective
- Shielded
- Mending
- Summoner
- Elementalist
- Enraged
- Twin

### Extra Effects
- None
- Aggressive
- Quick
- Regenerating
- Curious
- Splitting
- Armored

### Infusions
- None
- Lightning
- Fire
- Frost
- Poison
- Chaos
- Spirit

# Field options

## Biomes
- Meadows
- Swamp
- Mountain
- BlackForest
- Plains
- AshLands
- DeepNorth
- Ocean
- Mistlands

## Global Keys

- defeated_eikthyr
- defeated_gdking
- defeated_bonemass
- defeated_dragon
- defeated_goblinking
- KilledTroll
- killed_surtling

Additional keys can be created manually through console commands, or by a mod like [Enhanced Progress Tracker](https://valheim.thunderstore.io/package/ASharpPen/Enhanced_Progress_Tracker/).

## Environments
- Clear
- Twilight_Clear
- Misty
- Darklands_dark
- Heath clear
- DeepForest Mist
- GDKing
- Rain
- LightRain
- ThunderStorm
- Eikthyr
- GoblinKing
- nofogts
- SwampRain
- Bonemass
- Snow
- Twilight_Snow
- Twilight_SnowStorm
- SnowStorm
- Moder
- Ashrain
- Crypt
- SunkenCrypt

## Factions
- Players
- AnimalsVeg
- ForestMonsters
- Undead
- Demon
- MountainMonsters
- SeaMonsters
- PlainsMonsters
- Boss

## Status Effects
Valheim status effect options are not easily identified. But this is a list of at least some of the possibilities.
- Burning
- Spirit
- Poison
- Frost
- Lightning
- Smoked
- Wet
- Rested
- Shelter
- CampFire
- Resting
- Cold
- Freezing
- Encumbered
- SoftDeath

## Noise
Noise is set on each player based on certain activities they perform. It is set directly, and does not accumulate, meaning a player chopping trees will have the same noise of 100 for each chop and not increasingly higher.

Certain creatures will treat the noise as a "sound range". This means if the noise is greater than their "hearing" setting, and "noise" is within range of the creature (100 noise is 100 meters), they will react.

Noise constantly decays if no action is performed.
Known causes of noise:
- Dodge: 5
- Punching: 5
- Walking: 15
- Running: 30
- Jumping: 30
- Chopping / Pickaxing: 40
- Remove building piece: 50
- Chopping trees: 100
- Breaking rocks: 100

Apart from that, every attack will have a hit-noise and swing noise. By default this is 30 and 10, but this could be different for each attack type.

For those who got this far: An additional "feature" hint. The game does not care what prefab you give it, it does NOT need to be a mob. Do with this knowledge what you will.

Changelog: 
- v0.11.0: 
	- Added region labelling for map biomes. Will now scan for connected biome zones, and assign an id for that whole area.
	- Added condition for spawning only in specified areas. Intended as a world specific setting. For those who have been waiting, this is the option to use for designated monster islands.
	- Added condition for spawning in an area, chance is pr area and only rolled once, to allow for variety in spawning across the world.
	- Added console commands for areas.
- v0.10.1: 
	- Fixed issue with world-spawner mobs not spawning in mountains.
- v0.10.0: 
	- Optimized network package sizes.
	- Added initial support for [MobAILib](https://www.nexusmods.com/valheim/mods/1188).
	- Added setting "SetTamed" for local- and world spawners.
	- Added setting "SetTamedCommandable" for local- and world spawners.
	- Added world spawner condition "" for nearby players having status effect.
- v0.9.1: 
	- Fixed issue with too early access of location info. Should resolve issue with local spawners not spawning creatures.
- v0.9.0: 
	- Added setting "UseDefaultLevels" to CLLC integraiton, to let Spawn That set levels.
	- Added setting "SetRelentless".
	- Added setting "SetTryDespawnOnConditionsInvalid".
	- Added setting "SetTryDespawnOnAlert".
	- Added setting "TemplateId", for assigning a specific identifier to mobs spawned by a template, for other mods to use (intended for future Drop That setting).
	- Added condition "ConditionNearbyPlayersNoiseThreshold".
- v0.8.2: 
	- Added more helpful warning- and error messages for when configurations are incorrectly set up.
	- Changed StopTouchingMyConfigs to be set to true by default when spawn_that.cfg is created. Due to the massive loading time impact of large config files.
- v0.8.1: 
	- Additional error handling for conditions. Should help fix potential errors coming out of the newly added player conditions.
- v0.8.0: 
	- Added world spawner condition for nearby players carried items.
	- Added world spawner condition for nearby players carried valuables.
	- Added option for assigning faction for world- and local spawner entries.
	- Added faction to pre-change debug files.
	- Changed when configs are applied to spawners, for increased compatibility with mods adding prefabs.
	- Additional error checks.
- v0.7.1: 
	- Fixed simple config being generated with wrong prefab-names.
	- Additional error handling.
	- Changed location info from server to client slightly, to hopefully stop issues with local spawners.
- v0.7.0: 
	- Added support for Creature Level and Loot Control (CLLC)
	- Added CLLC creature effect options for world- and local spawners.
	- Added CLLC world level condition to world spawners.
- v0.6.0: 
	- Added world spawner condition - RequiredNotGlobalKey
	- Added support for supplemental world- and local spawner config files.
	- Local spawner configs now work in multiplayer.
- v0.5.1: 
	- Added a (probably temporary) config to not run local spawner configs, due to issues with multiplayer.
	- Fixed error message from local spawners.
	- Removed a couple of pointless warnings.
- v0.5.0: 
	- Added new local spawner defaults to file dumps.
	- Added condition for world spawners. World age in days.
	- Added console command for getting current room in which player is standing.
	- Added configuration for Dungeons, Camps and Villages. All are considered local spawners.
	- Lots of bug fixes. Spawners should have an easier time having configuration "stick" now.
- v0.4.0: 
	- New condition for world spawners. Distance to center min/max.
	- Simple config initialized with creatures on file creation by default.
	- Various attempts at stabilizing and guarding code.
- v0.3.0: 
	- Server-to-client config sync added
- v0.2.0: 
	- Initial release
