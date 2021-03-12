# Spawn That!

This is an advanced tool for configuring all world spawners.

With this, it is possible to change almost all of the default settings for the way spawners work in Valheim.\
Want to have a world without trolls? Possible! (probably)\
Want to have a world with ONLY trolls? Possible! (almost)\
Want to have a world where greydwarves only spawn at night? Possible!\
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

## Limitations
This mod will not change any non-spawner based entities, such as dungeon mobs that only spawn once.
Nor does it support changes to breeding.

## How does it all work?

Valheim's main way of managing spawns work by having two types of spawners spread throughout the world.\
The world spawners, for which the world has them spread out in a grid fashion, each using the same list of templates to check if something should spawn. These are the general spawners and all currently use the same 43 templates.\
The local spawners, which are intended for fine-tuned spawning. Local spawners only spawn one specific mob type, and only has one alive at a time. These are bound to specific world locations, such as the surtling firehole.

For world spawners, you can either replace existing templates based on their index, or add to the list of possible templates to spawn from.

For local spawners, since these are more custom, you describe a location and the prefab name of the mob you what you want to override.

The mod modification happens at run-time, once for each spawner. Reloading the world resets all changes.
As the player moves through the world, the game loads in the various spawners, and the mod applies its own settings.

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

World spawner templates are managed through the "spawn_that.world_spawners_advanced.cfg" file.\
There are currently 43 default templates, which can be overriden by setting a matching index (starting from 0).

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

# Full list of SpawnerSystem templates (World Spawners)

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

[WorldSpawner.1]
Name=Boar
Enabled=True
Biomes=Meadows,
PrefabName=Boar
HuntPlayer=False
MaxSpawned=4
SpawnInterval=150
SpawnChance=50
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=800
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
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.2]
Name=Neck lakes
Enabled=True
Biomes=Meadows,
PrefabName=Neck
HuntPlayer=False
MaxSpawned=6
SpawnInterval=100
SpawnChance=50
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=800
SpawnDistance=5
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=2
GroupSizeMax=4
GroupRadius=5
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=-1.5
ConditionAltitudeMax=0.5
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=1

[WorldSpawner.3]
Name=Neck IN RAIN
Enabled=True
Biomes=Meadows,
PrefabName=Neck
HuntPlayer=False
MaxSpawned=8
SpawnInterval=30
SpawnChance=100
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=800
SpawnDistance=32
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=RainThunderStorm,LightRain,
GroupSizeMin=2
GroupSizeMax=4
GroupRadius=5
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=10
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.4]
Name=Seagull
Enabled=True
Biomes=Meadows,BlackForest,Plains,Ocean,
PrefabName=Seagal
HuntPlayer=False
MaxSpawned=5
SpawnInterval=120
SpawnChance=50
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=51
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=2
GroupRadius=2
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=-4
ConditionAltitudeMax=0.2
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.5]
Name=Fish1
Enabled=True
Biomes=Meadows,BlackForest,Plains,
PrefabName=Fish1
HuntPlayer=False
MaxSpawned=5
SpawnInterval=120
SpawnChance=50
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=20
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=2
GroupSizeMax=6
GroupRadius=2
GroundOffset=0.1
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=-3
ConditionAltitudeMax=-1.5
ConditionTiltMin=0
ConditionTiltMax=99
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.6]
Name=Fish2
Enabled=True
Biomes=Meadows,BlackForest,Plains,
PrefabName=Fish2
HuntPlayer=False
MaxSpawned=4
SpawnInterval=120
SpawnChance=50
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=20
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=2
GroupSizeMax=4
GroupRadius=2
GroundOffset=0.1
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=-5
ConditionAltitudeMax=-2
ConditionTiltMin=0
ConditionTiltMax=99
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.7]
Name=Fish3
Enabled=True
Biomes=Meadows,BlackForest,Plains,Ocean,
PrefabName=Fish3
HuntPlayer=False
MaxSpawned=3
SpawnInterval=120
SpawnChance=50
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=20
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=2
GroupRadius=2
GroundOffset=0.1
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=-999
ConditionAltitudeMax=-5
ConditionTiltMin=0
ConditionTiltMax=99
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.8]
Name=greydwarf DAY
Enabled=True
Biomes=BlackForest,
PrefabName=Greydwarf
HuntPlayer=False
MaxSpawned=3
SpawnInterval=120
SpawnChance=30
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=2
GroupSizeMax=3
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=False
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.9]
Name=greydwarf After boss
Enabled=True
Biomes=Meadows,
PrefabName=Greydwarf
HuntPlayer=False
MaxSpawned=1
SpawnInterval=120
SpawnChance=30
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=defeated_eikthyr
RequiredEnvironments=
GroupSizeMin=2
GroupSizeMax=3
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=False
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.10]
Name=greydwarf Night
Enabled=True
Biomes=BlackForest,
PrefabName=Greydwarf
HuntPlayer=False
MaxSpawned=6
SpawnInterval=60
SpawnChance=100
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=3
GroupSizeMax=4
GroupRadius=10
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.11]
Name=greydwarf ELITE
Enabled=True
Biomes=BlackForest,
PrefabName=Greydwarf_Elite
HuntPlayer=False
MaxSpawned=1
SpawnInterval=120
SpawnChance=50.6
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=2000
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=1
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.12]
Name=
Enabled=True
Biomes=Mountain,
PrefabName=Fenring
HuntPlayer=False
MaxSpawned=2
SpawnInterval=400
SpawnChance=20
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=1
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.13]
Name=
Enabled=True
Biomes=Meadows,
PrefabName=Greydwarf_Elite
HuntPlayer=False
MaxSpawned=1
SpawnInterval=120
SpawnChance=50.6
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=defeated_gdking
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=1
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.14]
Name=
Enabled=True
Biomes=Meadows,
PrefabName=Greydwarf_Shaman
HuntPlayer=False
MaxSpawned=1
SpawnInterval=120
SpawnChance=50.6
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=defeated_gdking
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=1
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.15]
Name=
Enabled=True
Biomes=Meadows,Swamp,Mountain,BlackForest,Plains,
PrefabName=Skeleton
HuntPlayer=False
MaxSpawned=3
SpawnInterval=300
SpawnChance=25
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=defeated_bonemass
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=3
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.16]
Name=
Enabled=True
Biomes=Meadows,Mountain,BlackForest,Plains,
PrefabName=Draugr
HuntPlayer=False
MaxSpawned=1
SpawnInterval=300
SpawnChance=5
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=defeated_gdking
RequiredEnvironments=Misty
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=1
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.17]
Name=
Enabled=True
Biomes=Meadows,Mountain,BlackForest,
PrefabName=Goblin
HuntPlayer=True
MaxSpawned=3
SpawnInterval=3000
SpawnChance=5
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=defeated_goblinking
RequiredEnvironments=
GroupSizeMin=3
GroupSizeMax=3
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.18]
Name=
Enabled=True
Biomes=BlackForest,
PrefabName=Greydwarf_Shaman
HuntPlayer=False
MaxSpawned=2
SpawnInterval=60
SpawnChance=50.6
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=1
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.19]
Name=
Enabled=True
Biomes=Meadows,
PrefabName=Greydwarf
HuntPlayer=False
MaxSpawned=2
SpawnInterval=200
SpawnChance=50
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=20
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=defeated_eikthyr
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.20]
Name=
Enabled=True
Biomes=Meadows,
PrefabName=Greyling
HuntPlayer=False
MaxSpawned=2
SpawnInterval=300
SpawnChance=50
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=30
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=3
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

[WorldSpawner.21]
Name=ODIN
Enabled=True
Biomes=Meadows,Swamp,BlackForest,Plains,
PrefabName=odin
HuntPlayer=False
MaxSpawned=1
SpawnInterval=3000
SpawnChance=10
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=35
SpawnRadiusMax=35
RequiredGlobalKey=defeated_gdking
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.22]
Name=
Enabled=True
Biomes=BlackForest,
PrefabName=Troll
HuntPlayer=False
MaxSpawned=1
SpawnInterval=4000
SpawnChance=5
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=2000
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.23]
Name=Marsh draugr
Enabled=True
Biomes=Swamp,
PrefabName=Draugr
HuntPlayer=False
MaxSpawned=4
SpawnInterval=120
SpawnChance=50
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=2
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=-1.5
ConditionAltitudeMax=10
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.24]
Name=
Enabled=True
Biomes=Swamp,
PrefabName=Skeleton
HuntPlayer=False
MaxSpawned=4
SpawnInterval=400
SpawnChance=25
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=0
SpawnDistance=15
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=2
GroupSizeMax=4
GroupRadius=4
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=-1
ConditionAltitudeMax=10
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.25]
Name=
Enabled=True
Biomes=Swamp,
PrefabName=Draugr_Elite
HuntPlayer=False
MaxSpawned=1
SpawnInterval=300
SpawnChance=5
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=15
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=1
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=-1.5
ConditionAltitudeMax=10
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.26]
Name=Marsh surtling
Enabled=False
Biomes=Swamp,
PrefabName=Surtling
HuntPlayer=False
MaxSpawned=2
SpawnInterval=40
SpawnChance=30
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=30
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=2
GroupRadius=1
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.27]
Name=Blob
Enabled=True
Biomes=Swamp,
PrefabName=Blob
HuntPlayer=False
MaxSpawned=2
SpawnInterval=500
SpawnChance=20
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=20
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=2
GroupRadius=1
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=-1
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.28]
Name=
Enabled=True
Biomes=Swamp,
PrefabName=Leech
HuntPlayer=False
MaxSpawned=10
SpawnInterval=200
SpawnChance=50
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=0
SpawnDistance=5
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=2
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=-3
ConditionAltitudeMax=-0.1
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.29]
Name=
Enabled=True
Biomes=Swamp,
PrefabName=BlobElite
HuntPlayer=False
MaxSpawned=1
SpawnInterval=120
SpawnChance=20
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=20
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=1
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=-1
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.30]
Name=
Enabled=True
Biomes=Plains,
PrefabName=Goblin
HuntPlayer=False
MaxSpawned=5
SpawnInterval=60
SpawnChance=33
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=0
SpawnDistance=30
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=3
GroupSizeMax=5
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.31]
Name=
Enabled=True
Biomes=Plains,
PrefabName=Goblin
HuntPlayer=False
MaxSpawned=2
SpawnInterval=1000
SpawnChance=33
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=0
SpawnDistance=30
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=2
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=False
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.32]
Name=
Enabled=True
Biomes=Plains,
PrefabName=GoblinBrute
HuntPlayer=False
MaxSpawned=1
SpawnInterval=1000
SpawnChance=20
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=0
SpawnDistance=100
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=2
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=False
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.33]
Name=
Enabled=True
Biomes=Plains,
PrefabName=Lox
HuntPlayer=False
MaxSpawned=3
SpawnInterval=1000
SpawnChance=5
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=100
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=2
GroupSizeMax=4
GroupRadius=10
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=1
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.34]
Name=
Enabled=True
Biomes=Plains,
PrefabName=Deathsquito
HuntPlayer=False
MaxSpawned=3
SpawnInterval=500
SpawnChance=20
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=30
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=2
GroupRadius=10
GroundOffset=1
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=1
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=99
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.35]
Name=
Enabled=True
Biomes=Swamp,
PrefabName=Wraith
HuntPlayer=False
MaxSpawned=1
SpawnInterval=200
SpawnChance=30
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=20
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=1
GroundOffset=50
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=-2
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.36]
Name=
Enabled=True
Biomes=Mountain,
PrefabName=StoneGolem
HuntPlayer=False
MaxSpawned=1
SpawnInterval=120
SpawnChance=25.6
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=4
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=120
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.37]
Name=
Enabled=True
Biomes=Mountain,
PrefabName=Hatchling
HuntPlayer=False
MaxSpawned=2
SpawnInterval=120
SpawnChance=25.6
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=4
GroundOffset=10
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=100
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=66
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.38]
Name=
Enabled=True
Biomes=Mountain,
PrefabName=Wolf
HuntPlayer=False
MaxSpawned=6
SpawnInterval=120
SpawnChance=50
LevelMin=1
LevelMax=3
LevelUpMinCenterDistance=0
SpawnDistance=30
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=2
GroupSizeMax=4
GroupRadius=5
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=45
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.39]
Name=
Enabled=True
Biomes=Mountain,
PrefabName=Wolf
HuntPlayer=False
MaxSpawned=3
SpawnInterval=400
SpawnChance=25.6
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=30
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=2
GroupRadius=5
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=False
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=45
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.40]
Name=
Enabled=True
Biomes=BlackForest,
PrefabName=FireFlies
HuntPlayer=False
MaxSpawned=4
SpawnInterval=20
SpawnChance=100
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=0
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.41]
Name=
Enabled=True
Biomes=AshLands,
PrefabName=Surtling
HuntPlayer=False
MaxSpawned=10
SpawnInterval=8
SpawnChance=100
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=10
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=-1000
ConditionAltitudeMax=1000
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.42]
Name=
Enabled=True
Biomes=Ocean,
PrefabName=Serpent
HuntPlayer=False
MaxSpawned=1
SpawnInterval=1000
SpawnChance=5
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=50
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=False
SpawnDuringNight=True
ConditionAltitudeMin=-1000
ConditionAltitudeMax=-5
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

[WorldSpawner.43]
Name=
Enabled=True
Biomes=Ocean,
PrefabName=Serpent
HuntPlayer=False
MaxSpawned=1
SpawnInterval=1000
SpawnChance=5
LevelMin=1
LevelMax=1
LevelUpMinCenterDistance=0
SpawnDistance=50
SpawnRadiusMin=0
SpawnRadiusMax=0
RequiredGlobalKey=
RequiredEnvironments=ThunderStormRain,
GroupSizeMin=1
GroupSizeMax=1
GroupRadius=3
GroundOffset=0.5
SpawnDuringDay=True
SpawnDuringNight=True
ConditionAltitudeMin=-1000
ConditionAltitudeMax=-5
ConditionTiltMin=0
ConditionTiltMax=35
SpawnInForest=True
SpawnOutsideForest=True
OceanDepthMin=0
OceanDepthMax=0

```

# Full list of CreatureSpawners (Local Spawners):

``` INI

## Biome: Meadows, Location: Runestone_Boars, Spawner: 0 
[Runestone_Boars.Boar]
PrefabName=Boar
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

## Biome: Meadows, Location: Runestone_Boars, Spawner: 1 
[Runestone_Boars.Boar]
PrefabName=Boar
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

## Biome: Meadows, Location: Runestone_Boars, Spawner: 2 
[Runestone_Boars.Boar]
PrefabName=Boar
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

## Biome: Meadows, Location: Runestone_Boars, Spawner: 3 
[Runestone_Boars.Boar]
PrefabName=Boar
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

## Biome: Meadows, Location: Runestone_Boars, Spawner: 4 
[Runestone_Boars.Boar]
PrefabName=Boar
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

## Biome: Meadows, Location: Runestone_Boars, Spawner: 5 
[Runestone_Boars.Boar]
PrefabName=Boar
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

## Biome: Meadows, Location: Runestone_Boars, Spawner: 6 
[Runestone_Boars.Boar]
PrefabName=Boar
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

## Biome: Meadows, Location: Runestone_Boars, Spawner: 7 
[Runestone_Boars.Boar]
PrefabName=Boar
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

## Biome: Meadows, Location: Runestone_Boars, Spawner: 8 
[Runestone_Boars.Boar]
PrefabName=Boar
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


## Biome: Swamp, Location: FireHole, Spawner: 0 
[FireHole.Surtling]
PrefabName=Surtling
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=3
LevelUpChance=10
RespawnTime=5
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: FireHole, Spawner: 1 
[FireHole.Surtling]
PrefabName=Surtling
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=3
LevelUpChance=10
RespawnTime=5
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: FireHole, Spawner: 2 
[FireHole.Surtling]
PrefabName=Surtling
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=3
LevelUpChance=10
RespawnTime=5
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Swamp, Location: Runestone_Draugr, Spawner: 0 
[Runestone_Draugr.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: Runestone_Draugr, Spawner: 1 
[Runestone_Draugr.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: Runestone_Draugr, Spawner: 2 
[Runestone_Draugr.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Swamp, Location: SunkenCrypt1, Spawner: 0 
[SunkenCrypt1.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt1, Spawner: 1 
[SunkenCrypt1.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt1, Spawner: 2 
[SunkenCrypt1.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt1, Spawner: 3 
[SunkenCrypt1.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt1, Spawner: 4 
[SunkenCrypt1.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt1, Spawner: 5 
[SunkenCrypt1.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt1, Spawner: 6 
[SunkenCrypt1.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt1, Spawner: 7 
[SunkenCrypt1.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt1, Spawner: 8 
[SunkenCrypt1.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt1, Spawner: 9 
[SunkenCrypt1.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt1, Spawner: 10 
[SunkenCrypt1.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt1, Spawner: 11 
[SunkenCrypt1.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt1, Spawner: 12 
[SunkenCrypt1.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt1, Spawner: 13 
[SunkenCrypt1.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Swamp, Location: SunkenCrypt2, Spawner: 0 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 1 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 2 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 3 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 4 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 5 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 6 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 7 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 8 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 9 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 10 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 11 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 12 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 13 
[SunkenCrypt2.Draugr_Ranged]
PrefabName=Draugr_Ranged
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=20
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 14 
[SunkenCrypt2.Draugr_Ranged]
PrefabName=Draugr_Ranged
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=20
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 15 
[SunkenCrypt2.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 16 
[SunkenCrypt2.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 17 
[SunkenCrypt2.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 18 
[SunkenCrypt2.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 19 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 20 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 21 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 22 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 23 
[SunkenCrypt2.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 24 
[SunkenCrypt2.Greydwarf_Elite]
PrefabName=Greydwarf_Elite
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

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 25 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 26 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 27 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 28 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 29 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 30 
[SunkenCrypt2.Wraith]
PrefabName=Wraith
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 31 
[SunkenCrypt2.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 32 
[SunkenCrypt2.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt2, Spawner: 33 
[SunkenCrypt2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Swamp, Location: SunkenCrypt3, Spawner: 0 
[SunkenCrypt3.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 1 
[SunkenCrypt3.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 2 
[SunkenCrypt3.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 3 
[SunkenCrypt3.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=10
TriggerNoise=80
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 4 
[SunkenCrypt3.Wraith]
PrefabName=Wraith
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 5 
[SunkenCrypt3.Wraith]
PrefabName=Wraith
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 6 
[SunkenCrypt3.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 7 
[SunkenCrypt3.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 8 
[SunkenCrypt3.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 9 
[SunkenCrypt3.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 10 
[SunkenCrypt3.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 11 
[SunkenCrypt3.Blob]
PrefabName=Blob
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 12 
[SunkenCrypt3.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 13 
[SunkenCrypt3.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 14 
[SunkenCrypt3.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 15 
[SunkenCrypt3.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 16 
[SunkenCrypt3.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt3, Spawner: 17 
[SunkenCrypt3.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Swamp, Location: SunkenCrypt4, Spawner: 0 
[SunkenCrypt4.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SunkenCrypt4, Spawner: 1 
[SunkenCrypt4.BlobElite]
PrefabName=BlobElite
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Swamp, Location: SwampHut1, Spawner: 0 
[SwampHut1.Wraith]
PrefabName=Wraith
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Swamp, Location: SwampHut2, Spawner: 0 
[SwampHut2.Wraith]
PrefabName=Wraith
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Swamp, Location: SwampHut3, Spawner: 0 
[SwampHut3.Wraith]
PrefabName=Wraith
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Swamp, Location: SwampHut4, Spawner: 0 
[SwampHut4.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SwampHut4, Spawner: 1 
[SwampHut4.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SwampHut4, Spawner: 2 
[SwampHut4.Draugr_Ranged]
PrefabName=Draugr_Ranged
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SwampHut4, Spawner: 3 
[SwampHut4.Draugr_Ranged]
PrefabName=Draugr_Ranged
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Swamp, Location: SwampHut5, Spawner: 0 
[SwampHut5.Wraith]
PrefabName=Wraith
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Swamp, Location: SwampRuin1, Spawner: 0 
[SwampRuin1.Draugr_Elite]
PrefabName=Draugr_Elite
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SwampRuin1, Spawner: 1 
[SwampRuin1.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SwampRuin1, Spawner: 2 
[SwampRuin1.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Swamp, Location: SwampRuin2, Spawner: 0 
[SwampRuin2.Draugr_Elite]
PrefabName=Draugr_Elite
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SwampRuin2, Spawner: 1 
[SwampRuin2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SwampRuin2, Spawner: 2 
[SwampRuin2.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Swamp, Location: SwampWell1, Spawner: 0 
[SwampWell1.Draugr_Elite]
PrefabName=Draugr_Elite
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Swamp, Location: SwampWell1, Spawner: 1 
[SwampWell1.Draugr_Elite]
PrefabName=Draugr_Elite
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Mountain, Location: AbandonedLogCabin02, Spawner: 0 
[AbandonedLogCabin02.StoneGolem]
PrefabName=StoneGolem
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Mountain, Location: AbandonedLogCabin02, Spawner: 1 
[AbandonedLogCabin02.StoneGolem]
PrefabName=StoneGolem
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Mountain, Location: AbandonedLogCabin03, Spawner: 0 
[AbandonedLogCabin03.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: AbandonedLogCabin03, Spawner: 1 
[AbandonedLogCabin03.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: AbandonedLogCabin03, Spawner: 2 
[AbandonedLogCabin03.StoneGolem]
PrefabName=StoneGolem
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Mountain, Location: AbandonedLogCabin04, Spawner: 0 
[AbandonedLogCabin04.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: AbandonedLogCabin04, Spawner: 1 
[AbandonedLogCabin04.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: AbandonedLogCabin04, Spawner: 2 
[AbandonedLogCabin04.StoneGolem]
PrefabName=StoneGolem
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Mountain, Location: AbandonedLogCabin04, Spawner: 3 
[AbandonedLogCabin04.StoneGolem]
PrefabName=StoneGolem
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Mountain, Location: DrakeNest01, Spawner: 0 
[DrakeNest01.Hatchling]
PrefabName=Hatchling
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Mountain, Location: DrakeNest01, Spawner: 1 
[DrakeNest01.Hatchling]
PrefabName=Hatchling
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Mountain, Location: DrakeNest01, Spawner: 2 
[DrakeNest01.Hatchling]
PrefabName=Hatchling
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=1
LevelMax=1
LevelUpChance=15
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Mountain, Location: StoneTowerRuins04, Spawner: 0 
[StoneTowerRuins04.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins04, Spawner: 1 
[StoneTowerRuins04.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins04, Spawner: 2 
[StoneTowerRuins04.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Mountain, Location: StoneTowerRuins04, Spawner: 3 
[StoneTowerRuins04.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True

## Biome: Mountain, Location: StoneTowerRuins04, Spawner: 4 
[StoneTowerRuins04.Draugr]
PrefabName=Draugr
Enabled=True
SpawnAtDay=True
SpawnAtNight=True
LevelMin=3
LevelMax=1
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 0 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 1 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 2 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 3 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 4 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 5 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 6 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 7 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 8 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 9 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 10 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 11 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 12 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 13 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 14 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 15 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 16 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 17 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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

## Biome: Mountain, Location: StoneTowerRuins05, Spawner: 18 
[StoneTowerRuins05.Skeleton]
PrefabName=Skeleton
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


## Biome: BlackForest, Location: Crypt2, Spawner: 0 
[Crypt2.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: Crypt2, Spawner: 1 
[Crypt2.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: Crypt2, Spawner: 2 
[Crypt2.Skeleton]
PrefabName=Skeleton
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


## Biome: BlackForest, Location: Crypt3, Spawner: 0 
[Crypt3.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: Crypt3, Spawner: 1 
[Crypt3.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: Crypt3, Spawner: 2 
[Crypt3.Skeleton]
PrefabName=Skeleton
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


## Biome: BlackForest, Location: Crypt4, Spawner: 0 
[Crypt4.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: Crypt4, Spawner: 1 
[Crypt4.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: Crypt4, Spawner: 2 
[Crypt4.Skeleton]
PrefabName=Skeleton
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


## Biome: BlackForest, Location: Ruin1, Spawner: 0 
[Ruin1.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: Ruin1, Spawner: 1 
[Ruin1.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: Ruin1, Spawner: 2 
[Ruin1.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: Ruin1, Spawner: 3 
[Ruin1.Greydwarf_Shaman]
PrefabName=Greydwarf_Shaman
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

## Biome: BlackForest, Location: Ruin1, Spawner: 4 
[Ruin1.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: Ruin1, Spawner: 5 
[Ruin1.Greydwarf]
PrefabName=Greydwarf
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


## Biome: BlackForest, Location: Ruin2, Spawner: 0 
[Ruin2.Greydwarf_Elite]
PrefabName=Greydwarf_Elite
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

## Biome: BlackForest, Location: Ruin2, Spawner: 1 
[Ruin2.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: Ruin2, Spawner: 2 
[Ruin2.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: Ruin2, Spawner: 3 
[Ruin2.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: Ruin2, Spawner: 4 
[Ruin2.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: Ruin2, Spawner: 5 
[Ruin2.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: Ruin2, Spawner: 6 
[Ruin2.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: Ruin2, Spawner: 7 
[Ruin2.Greydwarf]
PrefabName=Greydwarf
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


## Biome: BlackForest, Location: StoneHouse3, Spawner: 0 
[StoneHouse3.Greydwarf]
PrefabName=Greydwarf
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


## Biome: BlackForest, Location: StoneHouse4, Spawner: 0 
[StoneHouse4.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: StoneHouse4, Spawner: 1 
[StoneHouse4.Greydwarf]
PrefabName=Greydwarf
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


## Biome: BlackForest, Location: StoneTowerRuins03, Spawner: 0 
[StoneTowerRuins03.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: StoneTowerRuins03, Spawner: 1 
[StoneTowerRuins03.Greydwarf_Elite]
PrefabName=Greydwarf_Elite
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

## Biome: BlackForest, Location: StoneTowerRuins03, Spawner: 2 
[StoneTowerRuins03.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: StoneTowerRuins03, Spawner: 3 
[StoneTowerRuins03.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: StoneTowerRuins03, Spawner: 4 
[StoneTowerRuins03.Greydwarf]
PrefabName=Greydwarf
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

## Biome: BlackForest, Location: StoneTowerRuins03, Spawner: 5 
[StoneTowerRuins03.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins03, Spawner: 6 
[StoneTowerRuins03.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins03, Spawner: 7 
[StoneTowerRuins03.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins03, Spawner: 8 
[StoneTowerRuins03.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins03, Spawner: 9 
[StoneTowerRuins03.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins03, Spawner: 10 
[StoneTowerRuins03.Skeleton]
PrefabName=Skeleton
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


## Biome: BlackForest, Location: StoneTowerRuins07, Spawner: 0 
[StoneTowerRuins07.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins07, Spawner: 1 
[StoneTowerRuins07.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins07, Spawner: 2 
[StoneTowerRuins07.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins07, Spawner: 3 
[StoneTowerRuins07.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins07, Spawner: 4 
[StoneTowerRuins07.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins07, Spawner: 5 
[StoneTowerRuins07.Skeleton]
PrefabName=Skeleton
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


## Biome: BlackForest, Location: StoneTowerRuins08, Spawner: 0 
[StoneTowerRuins08.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins08, Spawner: 1 
[StoneTowerRuins08.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins08, Spawner: 2 
[StoneTowerRuins08.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins08, Spawner: 3 
[StoneTowerRuins08.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins08, Spawner: 4 
[StoneTowerRuins08.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins08, Spawner: 5 
[StoneTowerRuins08.Skeleton]
PrefabName=Skeleton
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


## Biome: BlackForest, Location: StoneTowerRuins09, Spawner: 0 
[StoneTowerRuins09.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins09, Spawner: 1 
[StoneTowerRuins09.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins09, Spawner: 2 
[StoneTowerRuins09.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins09, Spawner: 3 
[StoneTowerRuins09.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins09, Spawner: 4 
[StoneTowerRuins09.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins09, Spawner: 5 
[StoneTowerRuins09.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins09, Spawner: 6 
[StoneTowerRuins09.Skeleton]
PrefabName=Skeleton
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


## Biome: BlackForest, Location: StoneTowerRuins10, Spawner: 0 
[StoneTowerRuins10.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins10, Spawner: 1 
[StoneTowerRuins10.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins10, Spawner: 2 
[StoneTowerRuins10.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins10, Spawner: 3 
[StoneTowerRuins10.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins10, Spawner: 4 
[StoneTowerRuins10.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins10, Spawner: 5 
[StoneTowerRuins10.Skeleton]
PrefabName=Skeleton
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

## Biome: BlackForest, Location: StoneTowerRuins10, Spawner: 6 
[StoneTowerRuins10.Skeleton]
PrefabName=Skeleton
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


## Biome: BlackForest, Location: TrollCave, Spawner: 0 
[TrollCave.Troll]
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


## Biome: BlackForest, Location: TrollCave02, Spawner: 0 
[TrollCave02.Troll]
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

## Biome: BlackForest, Location: TrollCave02, Spawner: 1 
[TrollCave02.Troll]
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


## Biome: 9, Location: Dolmen01, Spawner: 0 
[Dolmen01.Skeleton_NoArcher]
PrefabName=Skeleton_NoArcher
Enabled=True
SpawnAtDay=False
SpawnAtNight=True
LevelMin=1
LevelMax=3
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: 9, Location: Dolmen02, Spawner: 0 
[Dolmen02.Skeleton_NoArcher]
PrefabName=Skeleton_NoArcher
Enabled=True
SpawnAtDay=False
SpawnAtNight=True
LevelMin=1
LevelMax=3
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: 9, Location: Dolmen03, Spawner: 0 
[Dolmen03.Skeleton_NoArcher]
PrefabName=Skeleton_NoArcher
Enabled=True
SpawnAtDay=False
SpawnAtNight=True
LevelMin=1
LevelMax=3
LevelUpChance=10
RespawnTime=0
TriggerDistance=60
TriggerNoise=0
SpawnInPlayerBase=False
SetPatrolPoint=True


## Biome: Plains, Location: GoblinCamp1, Spawner: 0 
[GoblinCamp1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: GoblinCamp1, Spawner: 1 
[GoblinCamp1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: GoblinCamp1, Spawner: 2 
[GoblinCamp1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: GoblinCamp1, Spawner: 3 
[GoblinCamp1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: GoblinCamp1, Spawner: 4 
[GoblinCamp1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: GoblinCamp1, Spawner: 5 
[GoblinCamp1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: GoblinCamp1, Spawner: 6 
[GoblinCamp1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: GoblinCamp1, Spawner: 7 
[GoblinCamp1.Goblin]
PrefabName=Goblin
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


## Biome: Plains, Location: Ruin3, Spawner: 0 
[Ruin3.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: Ruin3, Spawner: 1 
[Ruin3.Goblin]
PrefabName=Goblin
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


## Biome: Plains, Location: StoneHenge1, Spawner: 0 
[StoneHenge1.GoblinBrute]
PrefabName=GoblinBrute
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

## Biome: Plains, Location: StoneHenge1, Spawner: 1 
[StoneHenge1.GoblinBrute]
PrefabName=GoblinBrute
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

## Biome: Plains, Location: StoneHenge1, Spawner: 2 
[StoneHenge1.GoblinBrute]
PrefabName=GoblinBrute
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


## Biome: Plains, Location: StoneHenge2, Spawner: 0 
[StoneHenge2.GoblinBrute]
PrefabName=GoblinBrute
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

## Biome: Plains, Location: StoneHenge2, Spawner: 1 
[StoneHenge2.GoblinBrute]
PrefabName=GoblinBrute
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

## Biome: Plains, Location: StoneHenge2, Spawner: 2 
[StoneHenge2.GoblinBrute]
PrefabName=GoblinBrute
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


## Biome: Plains, Location: StoneHenge3, Spawner: 0 
[StoneHenge3.GoblinBrute]
PrefabName=GoblinBrute
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

## Biome: Plains, Location: StoneHenge3, Spawner: 1 
[StoneHenge3.GoblinBrute]
PrefabName=GoblinBrute
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

## Biome: Plains, Location: StoneHenge3, Spawner: 2 
[StoneHenge3.GoblinBrute]
PrefabName=GoblinBrute
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


## Biome: Plains, Location: StoneHenge4, Spawner: 0 
[StoneHenge4.GoblinBrute]
PrefabName=GoblinBrute
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

## Biome: Plains, Location: StoneHenge4, Spawner: 1 
[StoneHenge4.GoblinBrute]
PrefabName=GoblinBrute
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


## Biome: Plains, Location: StoneHenge5, Spawner: 0 
[StoneHenge5.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneHenge5, Spawner: 1 
[StoneHenge5.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneHenge5, Spawner: 2 
[StoneHenge5.Goblin]
PrefabName=Goblin
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


## Biome: Plains, Location: StoneHouse1_heath, Spawner: 0 
[StoneHouse1_heath.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneHouse1_heath, Spawner: 1 
[StoneHouse1_heath.Goblin]
PrefabName=Goblin
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


## Biome: Plains, Location: StoneHouse2_heath, Spawner: 0 
[StoneHouse2_heath.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneHouse2_heath, Spawner: 1 
[StoneHouse2_heath.Goblin]
PrefabName=Goblin
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


## Biome: Plains, Location: StoneTower1, Spawner: 0 
[StoneTower1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower1, Spawner: 1 
[StoneTower1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower1, Spawner: 2 
[StoneTower1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower1, Spawner: 3 
[StoneTower1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower1, Spawner: 4 
[StoneTower1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower1, Spawner: 5 
[StoneTower1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower1, Spawner: 6 
[StoneTower1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower1, Spawner: 7 
[StoneTower1.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower1, Spawner: 8 
[StoneTower1.Goblin]
PrefabName=Goblin
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


## Biome: Plains, Location: StoneTower2, Spawner: 0 
[StoneTower2.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower2, Spawner: 1 
[StoneTower2.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower2, Spawner: 2 
[StoneTower2.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower2, Spawner: 3 
[StoneTower2.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower2, Spawner: 4 
[StoneTower2.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower2, Spawner: 5 
[StoneTower2.Goblin]
PrefabName=Goblin
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


## Biome: Plains, Location: StoneTower3, Spawner: 0 
[StoneTower3.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower3, Spawner: 1 
[StoneTower3.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower3, Spawner: 2 
[StoneTower3.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower3, Spawner: 3 
[StoneTower3.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower3, Spawner: 4 
[StoneTower3.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower3, Spawner: 5 
[StoneTower3.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower3, Spawner: 6 
[StoneTower3.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower3, Spawner: 7 
[StoneTower3.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower3, Spawner: 8 
[StoneTower3.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower3, Spawner: 9 
[StoneTower3.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower3, Spawner: 10 
[StoneTower3.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower3, Spawner: 11 
[StoneTower3.Goblin]
PrefabName=Goblin
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


## Biome: Plains, Location: StoneTower4, Spawner: 0 
[StoneTower4.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower4, Spawner: 1 
[StoneTower4.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower4, Spawner: 2 
[StoneTower4.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower4, Spawner: 3 
[StoneTower4.Goblin]
PrefabName=Goblin
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

## Biome: Plains, Location: StoneTower4, Spawner: 4 
[StoneTower4.Goblin]
PrefabName=Goblin
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


## Biome: AshLands, Location: Meteorite, Spawner: 0 
[Meteorite.Surtling]
PrefabName=Surtling
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

## Biome: AshLands, Location: Meteorite, Spawner: 1 
[Meteorite.Surtling]
PrefabName=Surtling
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

## Biome: AshLands, Location: Meteorite, Spawner: 2 
[Meteorite.Surtling]
PrefabName=Surtling
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

## Biome: AshLands, Location: Meteorite, Spawner: 3 
[Meteorite.Surtling]
PrefabName=Surtling
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
