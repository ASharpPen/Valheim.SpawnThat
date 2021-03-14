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

## Limitations
This mod will not change any non-spawner based entities, such as dungeon mobs that only spawn once.
Nor does it support changes to breeding.

## FAQ

Why are the config files empty?
- All configs but spawn_that.cfg are intended empty initially. This is so that the game will behave as normal, until you start adding configurations to it.
- Enable any of the "WriteX" in spawn_that.cfg, and files containing the default values will be created in the plugins folder.
	- You can also go to the github page, where the main (massive) readme have them list.

Where do I find the prefab names?
- Multiple pages have long lists, you can check out this one [here](https://gist.github.com/Sonata26/e2b85d53e125fb40081b18e2aee6d584), or the [valheim wiki](https://valheim.fandom.com/wiki/Creatures)

## How does it all work?

Valheim's main way of managing spawns work by having two types of spawners spread throughout the world.
The world spawners, for which the world has them spread out in a grid fashion, each using the same list of templates to check if something should spawn. These are the general spawners and all currently use the same 43 templates.
The local spawners, which are intended for fine-tuned spawning. Local spawners only spawn one specific mob type, and only has one alive at a time. These are bound to specific world locations, such as the surtling firehole.

For world spawners, you can either replace existing templates based on their index, or add to the list of possible templates to spawn from.

For local spawners, since these are more custom, you describe a location and the prefab name of the mob you what you want to override.

The mod modification happens at run-time, once for each spawner. Reloading the world resets all changes.
As the player moves through the world, the game loads in the various spawners, and the mod applies its own settings.

## Client / Server

Drop That needs to be installed on all clients (and server) to work.

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

```

# Local spawners

Local spawners are customly set up in the world, based on pre-defined location setups. This means they are each customized to the location at which they are placed, and are not as easily "targetted" as the world spawners templates are.\
To modify a local spawner, you must specify a location and mob prefab name you want to apply a configuration to.

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

# World Spawners - Details

``` INI

[WorldSpawner.0]

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

## Maximum entities spawned from this spawner type (it's complicated. See documentation.).
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

```

For those who got this far: An additional "feature" hint. The game does not care what prefab you give it, it does NOT need to be a mob. Do with this knowledge what you will.

Changelog:
- v0.3.0
	- Server-to-client config sync added
- v0.2.0
	- Initial release