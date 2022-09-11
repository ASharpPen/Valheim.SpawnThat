# Local Spawner Config

Local spawner templates are managed through the `spawn_that.local_spawners_advanced.cfg` file.

Local spawners are known internally in the game as `CreatureSpawners`.
Each spawner manages only a single creature, and will never have more than one spawned at a time.

Local spawners are customly set up in the world, based on pre-defined location setups. This means they are each customized to the location at which they are placed, and are not as easily "targeted" as the world spawners templates are. 

Eg. the skeletons of an old ruined tower will each be spawned by a their own invisible local spawner, placed at very specific positions. If only spawning once, it is often due to respawn time being set to 0.

To modify a local spawner, you must specify a location and mob prefab name you want to apply a configuration to.
You can also use a room name, if more fine control is necessary for villages, dungeons and camps.

The format for a new template is `[Location.PrefabName]` or `[DungeonRoom.PrefabName]`. 

Eg.
To override the boar spawners at their runestone, use 
`[Runestone_Boars.Boar]`

Every combination of Location and PrefabName must be unique.

The [general config](general-config.md) contains debugging options, which can be toggled to create a file containing all default local spawner configs before the mod applies its changes.
This can also help identify location names, as well as dungeon/dungeon-room names.

Requirements:
- Location/dungeon-room specified must contain a local spawner
- Local spawner must have an existing prefab set to override

Limitations:
- Can only override existing local spawners
- When multiple local spawners have same location/dungeon-room name and same prefab, Spawn That cannot differentiate between them, so all gets overridden
- Only one template can be assigned to each local spawner

## Example 
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

## Config Options

| Setting | Type | Default | Example | Description |
| --- | --- | --- | --- | --- |
| PrefabName | string | | Troll | Name of prefab to spawn instead of existing |
| Enabled | bool | true | false | Toggle this template on-off. This means the settings of this template will NOT be applied, it is not a way to disable existing mobs |
| SpawnAtDay | bool | true | false | Enable spawning during day. |
| SpawnAtNight | bool | true | false | Enable spawning during night. |
| LevelMin | int | 1 | 3 | Minimum level of spawn |
| LevelMax | int | 1 | 15 | Maximum level of spawn |
| LevelUpChance | float | 15 | 100 | Chance to level up, starting at LevelMin and rolling again for each level gained. Range is 0 to 100 |
| RespawnTime | float | 20 | 0 | Minutes between checks for respawn. Only one mob can be spawned at time per spawner |
| TriggerDistance | float | 60 | 100 | Distance of spawner to player to trigger spawning |
| TriggerNoise | float | 0 | 50 | If not 0, adds a minimum noise required for spawning, on top of distance requirement |
| SpawnInPlayerBase | bool | false | true | Allow spawning inside player base boundaries |
| SetPatrolPoint | bool | false | true | Sets position of spawn as patrol point for spawned creature |
| SetFaction | string | | Undead | Assign a specific faction to spawn. If empty uses prefab default |
| SetTamed | bool | false | true | When true, mob will be set to tamed status on spawn |
| SetTamedCommandable | bool | false | true | Experimental. When true, will set mob as commandable when tamed. When false, whatever was default for the creature is used. Does not always seem to work for creatures not tameable in vanilla |

## Supplemental Files

Spawn That will load additional configurations from configs with names prefixed with `spawn_that.local_spawners.`.

This allows for adding your own custom templates to Spawn That, or simply separate your configs into more manageable pieces.
Eg. `spawn_that.local_spawners.my_custom_configuration.cfg`

The configurations loaded will be merged with the one loaded from the main files.