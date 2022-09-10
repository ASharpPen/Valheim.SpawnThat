# Tutorial - Creating a new world spawn

This tutorial will show how to make a new creature naturally spawn in the world.

## Creating a spawn entry

- Go to your `BepInEx/Configs` folder.
- Open the `spawn_that.world_spawners_advanced.cfg`.
- Add the following to the end of it.

```toml
[WorldSpawner.666]
PrefabName = Skeleton
```

- Save the file.

Done!

What have we done though? Well, by adding a section called `WorldSpawner`, with an ID (666) that isn't used by anything in the game (or by another mod), we have told Spawn That that instead of changing existing settings, it should be adding a completely new configuration entry.

The PrefabName specified is valheims internal name of the object. See [Finding Prefab Names](finding-prefab-names.md) for further details on how to find these.

## Adding conditions

When running the game, skeletons will now be spawning naturally in all areas of the world.
This is probably not ideal, and what we have in mind, as it will be using default spawn settings, and we don't really want skeletons everywhere (including the ocean).

So lets add some restrictions to where it spawns. Grab some options from [World Spawner Config Options](../config-types/world-spawner-config.md#config-options).

```toml
[WorldSpawner.666]
PrefabName = Skeleton
ConditionAltitudeMin = 0
Biomes = Meadows
SpawnOutsideForest = false
SpawnDuringDay = false
```

Thats a bit better. Now, the skeleton will only spawn above at the waterline or above (ConditionAltitudeMin), in the meadows biome, only in forests and will only be around during night.

## Changing spawn settings

But, they are kinda rare, and it would be cool if there were more of them around.
So lets change some of the spawn settings, so that they aren't just the defaults.

```toml
[WorldSpawner.666]
PrefabName = Skeleton
ConditionAltitudeMin = 0
Biomes = Meadows
SpawnOutsideForest = false
SpawnDuringDay = false
MaxSpawned = 10
GroupSizeMin = 3
GroupSizeMax = 5
SpawnInterval = 100
SpawnChance = 50
```

There, now they should be a lot more common. We should now be seeing them spawn in in groups of 3-5, with a reasonably high saturation due to the MaxSpawned. Using the SpawnInterval and SpawnChance that matches deer, so we can sorta expect the same frequency.

## Adding integration options

Lets be fair, most of you probably have the mod Creature Level and Loot Control installed, and it would be cool if could get that skeleton to be on fire, or use some of that mods other features.

So lets see what we can do here. Check the options we have in the [Creature Level and Loot Control Integration](../mod-specific/creature-level-and-loot-control.md#world-spawner-options) (CLLC) docs.

Add a section to our spawn, where we can use some settings.

```toml
[WorldSpawner.666]
PrefabName = Skeleton
ConditionAltitudeMin = 0
Biomes = Meadows
SpawnOutsideForest = false
SpawnDuringDay = false
MaxSpawned = 10
GroupSizeMin = 3
GroupSizeMax = 5
SpawnInterval = 100
SpawnChance = 50

[WorldSpawner.666.CreatureLevelAndLootControl]
SetInfusion = Fire
```

Thats better, when CLLC is installed, our skeletons will now be on fire when spawning in. If the mod is not installed, the new part we added will simply get ignored.

## Troubleshooting

Sometimes it can be hard to see if the changes are working as expected.

For this, an easy way out is to increase the spawns to the point where they should become immediately obvious.

### 1. Up the spawn rate

Change the settings of your world spawn config:

```toml
SpawnInterval = 0.01
SpawnChance = 100
MaxSpawned = 100
```

### 2. HuntPlayer

If an aggressive feature, setting `HuntPlayer` can make the spawned creatures come running on heir own, making it easier to see that they spawned:

```toml
HuntPlayer = true
SpawnInterval = 0.01
SpawnChance = 100
MaxSpawned = 100
```

### 3. Debug loaded configs

Print the loaded configs, to verify they were read in the expected way

- Go to `spawn_that.cfg`
- Find the section `[WorldSpawner]` and the setting `[WriteConfigsToFile]` under it.
- Set `WriteConfigsToFile=true`
- Save file
- Load the game, enter a world, and wait for a moment (you can watch the logs to see the message of files being printed and where to).
- Go to folder `BepInEx/Debug` (or if you changed the debug folder, then go to that), find the file `world_spawners_loaded_configs.cfg`
- Check the file for configs matching the expected ones from your config folder.

Alternatively, to check changes after applying configurations (loaded configs are not yet applied):
- Go to `spawn_that.cfg`
- Find the section `[WorldSpawner]` and the setting `[WriteSpawnTablesToFileAfterChanges]` under it.
- Set `WriteSpawnTablesToFileAfterChanges=true`
- Save file
- Load the game, enter a world, and wait for a moment (you can watch the logs to see the message of files being printed and where to).
- Go to folder `BepInEx/Debug` (or if you changed the debug folder, then go to that), find the file `world_spawners_post_changes.txt`
