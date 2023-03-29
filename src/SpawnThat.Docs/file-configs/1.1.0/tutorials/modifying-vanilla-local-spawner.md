----

⚠️ This is an archived version of the documentation. Find the latest version [here](/configs/general/intro.html) ⚠️

----

# Tutorial - Modifying a local spawner

This tutorial will describe how to modify a local spawner.

Local spawners are very different from world spawners, in that they don't have a simple ID to identify them by.
There can be a variable amount of them pr zone, each being a separate invisible manager, spawned based on pre-defined points of interest (locations). Each local spawner managing a single spawn.

For instance, boar runestones will have a semi-random number of local spawners surrounding them, in specific positions in relation to the runestone itself. Each of them ensuring a single boar spawns.

To "target" which spawner to modify, Spawn That uses the name of the location (or in case of dungeons/camps, rooms) and the prefab name that it is set to spawn.

This may result in each configuration affecting multiple spawners. Taking the example of the boar runestones, when we modify the spawner `[Runestone_Boars.Boar]`, the configuration affects all the boar spawners, and not just one of them.

## Identify what to modify

To start with, we need to figure out which spawners exist, so we can select which to modify.

1. To start with, find the `spawn_that.cfg` file in the BepInEx/config folder. If it doesn't exist yet, run the game to have it generate.

2. Set `WriteSpawnTablesToFileBeforeChanges` under `LocalSpawners` to `true`, to tell Spawn That to create a file extracting local spawners after entering a game.

    ![image](https://user-images.githubusercontent.com/16554392/189235054-05e53a0c-930f-41d7-842f-35b74bc42c47.png)

3. Run the game, and enter a world.

4. Check the BepInEx/Debug folder, for a file called either `spawn_that.local_spawners_dungeons_pre_changes.txt` or `spawn_that.local_spawners_pre_changes.txt`. If you can't find it, check your logs. There should be a line describing what files are being created and where.

5. Open it up, and find the local spawner entry that you want to change.

    ![image](https://user-images.githubusercontent.com/16554392/189235500-bcd16ad7-3a63-48be-9866-6ae2ec691c0e.png)
    ![image](https://user-images.githubusercontent.com/16554392/189235586-cad6b268-cf77-4355-aabe-b8b97ea68bfd.png)

## Modifying the spawner

Lets pick the `[Runestone_Boars.Boar]` to modify.

1. Go to your `BepInEx/Configs` folder.
2. Open the `spawn_that.local_spawners_advanced.cfg`.
3. Add the following to the end of it.

```toml
[Runestone_Boars.Boar]
PrefabName = Troll
```

This will make all boar runestones spawn trolls instead of boars. But only if they haven't already spawned a creature once (Valheim treats the `RespawnTime=0` as the spawner should only spawn the prefab a single time).

Lets make things a bit more dangerous. Lets add in respawning to every 10 minutes, change the faction so that the trolls will fight creatures in meadows, and increase the level up chance.

```toml
[Runestone_Boars.Boar]
PrefabName = Troll
RespawnTime = 10
SetFaction = Undead
LevelUpChance = 30
```

The final file should now look like this:

![image](https://user-images.githubusercontent.com/16554392/189237850-31c1897f-5126-4d02-86b8-3cc93e3301a0.png)

(If you don't have the green text above, don't worry. Its just auto-generated comments that gets added when the file is first generated in newer versions of Spawn That, it does nothing)

## Troubleshooting

If we want to verify that our modifications worked, an easy way is to increase the respawn time.
Setting it to a very low number, combined with a devcommand for killing all nearby creatures, usually help with identifying if the config is taking effect.

Eg.
```toml
RespawnTime = 0.001
```
