# Tutorial - Modifying a modded world spawn

This tutorial will show how to modify an existing world spawner entry, added by another mod.

Note that modifying entries added by mods may or may not be possible / work, depending on how the mod added it. Entries added by mods using Spawn That should always be possible to modify.

## Identify what to modify

First of all, to modify a world spawner entry we need to either find the world spawner config file of what we are modifying, or extract a list of what is being loaded.

Since the file might not exist for mods using Spawn That code directly, we will focus on the more sure way of extracting world spawner entries from the game.

1. To start with, find the `spawn_that.cfg` file in the BepInEx/config folder. If it doesn't exist yet, run the game to have it generate.

2. Set WriteConfigsToFile under `WorldSpawners` to `true`, to tell Spawn That to create a file showing all its loaded configurations, no matter the source.

  ![image](https://user-images.githubusercontent.com/16554392/188332095-d14c04e6-94cc-42c6-822c-28383ef7eefd.png)

3. Run the game, and enter a world.

4. Check the BepInEx/Debug folder, for a file called `spawn_that.world_spawners_loaded_configs.cfg`. If you can't find it, check your logs. There should be a line describing what files are being created and where.

5. Open it up, and find the world spawner entry that you want to change.

  ![image](https://user-images.githubusercontent.com/16554392/188331273-44e67823-d9f8-4dbf-8bd4-8510d8e18790.png)

## Mod the mod

From the extracted file, we can see a few things we might want to change. We have something disabling deer spawns (`WORLDSPAWNER.0`), adding a troll spawner (`WORLDSPAWNER.10000001`) and boars being dropped from the sky (`WORLDSPAWNER.10000000`).

Lets get the deer spawns re-enabled, restrict the troll entry and disable the dropping boars.

Open up the file `spawn_that.world_spawners_advanced.cfg`, in the BepInEx/config folder.

Any configuration added to this file will be loaded last by Spawn That (see [config load order](../general/load-order.md)), and will therefore allow for overriding any configuration coming from other files or mods.

To re-enable the deer, we add this to the file:

```toml
[WorldSpawner.0]
Enabled=true
```

To disable the boars, we add:

```toml
[WorldSpawner.10000000]
Enabled=false
```

And finally, lets keep the trolls, but lets restrict their biome to meadows (as seemed intended in the extracted config, but wasn't actually done), but only in forrested areas.
We will also reduce their maximum count a bit, as its a bit excessive, and make them rarer.

```toml
[WorldSpawner.10000001]
Biomes=Meadows
SpawnInForest=true
SpawnOutsideForest=false
MaxSpawned=2
SpawnChance=5
SpawnInterval=4000
```

The final file should now look like this:

![image](https://user-images.githubusercontent.com/16554392/188324595-f557935f-e943-4f4d-99dd-e892553cd771.png)

(If you don't have the green text above, don't worry. Its just auto-generated comments that gets added when the file is first generated in newer versions of Spawn That, it does nothing)

If we rerun the steps from [Identify what to modify](#identify-what-to-modify), the `spawn_that.world_spawners_loaded_configs.cfg` should now show how our changes were applied.

![image](https://user-images.githubusercontent.com/16554392/188331009-58493816-668f-462e-ae76-8df6529467ea.png)
