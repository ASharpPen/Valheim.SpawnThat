# Finding Prefab Names

Prefab's are the "blueprints" of objects in the game, used to create basically anything. To get the name, we have a few options.

## Creatures

### 1. Wiki
Find the creature on the [wiki](https://valheim.fandom.com/wiki/Creatures), and find its "Internal Id".

### 2. Jötunn documentation

Find the creature using the excellent list from the [Jotunn documentation](https://valheim-modding.github.io/Jotunn/data/prefabs/character-list.html).

### 3. Datamine using Spawn That.

### 3.1 World Spawns
  - Open `spawn_that.cfg` in your BepInEx/Configs folder.
  - Find the section `[WorldSpawner]` and the setting `WriteSpawnTablesToFileBeforeChanges` under it.
  - Set it to `WriteSpawnTablesToFileBeforeChanges=true`.
  - Save the file
  - Run the game, and enter a world.
  - There should now be a file called `spawn_that.world_spawners_pre_changes.txt` in your `BepInEx/Debug/` folder, which have extracted data from the game to get the existing world spawner settings, including their prefab name.

Alternatively, a pre-extracted version exists [here](../data/world-spawner-vanilla.md).

### 3.2 Spawns in dungeons
  - Open `spawn_that.cfg` in your BepInEx/Configs folder.
  - Find the section `[LocalSpawner]` and the setting `WriteSpawnTablesToFileBeforeChanges` under it.
  - Set it to `WriteSpawnTablesToFileBeforeChanges=true`.
  - Save the file
  - Run the game, and enter a world.
  - There should now be a file called `spawn_that.local_spawners_dungeons_pre_changes.txt` in your `BepInEx/Debug/` folder, which have extracted data from the game to get the existing world spawner settings, including their prefab name.

Alternatively, a pre-extracted version exists [here](../data/local-spawner-room-vanilla.md).

### 3.3 Spawns at points of interest
  - Open `spawn_that.cfg` in your BepInEx/Configs folder.
  - Find the section `[LocalSpawner]` and the setting `WriteSpawnTablesToFileBeforeChanges` under it.
  - Set it to `WriteSpawnTablesToFileBeforeChanges=true`.
  - Save the file
  - Run the game, and enter a world.
  - There should now be a file called `spawn_that.local_spawners_pre_changes.txt` in your `BepInEx/Debug/` folder, which have extracted data from the game to get the existing world spawner settings, including their prefab name.

  Alternatively, a pre-extracted version exists [here](../data/local-spawner-location-vanilla.md).
