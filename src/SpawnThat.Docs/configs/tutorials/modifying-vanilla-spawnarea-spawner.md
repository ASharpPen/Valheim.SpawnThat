# Tutorial - Modifying a SpawnArea spawner

This tutorial will describe how to modify a SpawnArea spawner.

SpawnArea spawners are entities that will continously try to spawn in creatures from a list specific for that type of spawner.
The most typical example are the [Greydwarf nests](https://valheim.fandom.com/wiki/Greydwarf_nest) and [draugr piles](https://valheim.fandom.com/wiki/Body_pile).

SpawnArea spawners are more like local spawners than world spawners. There can be a variable amount of them per zone, and they are spawned based on pre-defined points of interest (locations). Instead of managing a single spawn though, they will check the surrounding area for creatures, to see if there is room for more.

To "target" which spawner to modify, we will use "Identifiers". Each "Identifier" setting specifies a state that must be correct for that spawner, for the configuration to take effect.

## Identify what to modify

To start with, we need to figure out which spawners exist, so we can select which to modify.

1. To start with, find the `spawn_that.cfg` file in the BepInEx/config folder. If it doesn't exist yet, run the game to have it generate.

2. Set `WriteSpawnTablesToFileBeforeChanges` under `SpawnAreaSpawner` to `true`, to tell Spawn That to create a file extracting SpawnArea spawners after entering a game.

    ![image](https://user-images.githubusercontent.com/16554392/189481088-cf665859-1741-406f-bc86-ecfa3ed21f7e.png)

3. Run the game, and enter a world.

4. Check the BepInEx/Debug folder, for a file called `spawn_that.spawnarea_spawners_pre_changes.txt`. If you can't find it, check your logs. There should be a line describing what files are being created and where.

5. Open it up, and find the SpawnArea spawner entry that you want to change.

    ![image](https://user-images.githubusercontent.com/16554392/189481260-787fa1dc-8b3b-4fa6-aa9f-ffc2d7457c15.png)

## Modifying the spawner

We are going to be changing the greydwarf nests that we found the configs for.

Lets pick the `[Spawner_Greydwarfnest]` to modify.

1. Go to your `BepInEx/Configs` folder.
2. Open the `spawn_that.spawnarea_spawners.cfg`.

Lets take a look at the info we have available:

![image](https://user-images.githubusercontent.com/16554392/189481532-c3a9517d-8088-49cc-b05d-836d5c1b35ef.png)

### Change spawner settings

Lets start by making the spawner iftself more active.

Add the following to the end of the `spawn_that.spawnarea_spawners.cfg` file.

```toml
[TutorialGreydwarfnest]
IdentifyByName = Spawner_GreydwarfNest
ConditionMaxCloseCreatures = 10
SpawnInterval = 1
```

The name in `[]` is just a name for this config, and can be anything. We will use it later when modifying the spawns.

Having just the `IdentifyByName` means we will be configuring all greydwarf nests with that [prefab name](finding-prefab-names.md).

The two other lines are for increasing how many creatures are needed in the area, before the spawner stops spawning more in, and the interval is reduced so that it attempts to spawn another in every second.

The file should now look like this:

![image](https://user-images.githubusercontent.com/16554392/189481869-0117a46a-e46e-4ef0-b982-2bc72dd43816.png)

(If you don't have the green text above, don't worry. Its just auto-generated comments that gets added when the file is first generated in newer versions of Spawn That, it does nothing)

### Adding more spawns to spawner

What are greydwarfs without their troll buddies? Lets add a chance for the greydwarf nests to also spawn in a troll friend!

Below the spawner configs we just added, add the following:

```toml
[TutorialGreydwarfnest.101]
PrefabName=Troll
SpawnWeight=0.5
```

We reuse the spawner config name (`TutorialGreydwarfnest`), to indicate that this spawn belong with our previous spawner settings.

The ID `101` is simply a number not used by the existing spawns for the `Spawner_GreydwarfNest` spawner, this ensures we are *adding* to the spawners spawnlist, and not *modifying*.

The `SpawnWeight` is used by the SpawnArea spawner. When it picks which spawn to use next, it will use the weight in its randomization. The more weight, the greater the chance of it picked. We set it to something low, to make the Troll more rare.

The file should now look like this:

![image](https://user-images.githubusercontent.com/16554392/189485910-27e28bba-cdbe-47e9-8c4d-e8ceb1080d75.png)

### Modifying existing spawns

Say we want to have more control over the greydwarf brutes spawning from the greydwarf nests. We can change the existing values, and add a few more conditions for them spawning in the first place.

We start by adding this to the file we are working in:

```toml
[TutorialGreydwarfnest.1]
```

Since we know our config (`TutorialGreydwarfnest`) is greydwarf spawners only, and that those have a greydwarf brute spawn with ID `1`, this is how we start changing it.

Lets increase its spawn weight, so that we see more of them, but also restrict it to night-time only.

```toml
[TutorialGreydwarfnest.1]
SpawnWeight = 5
ConditionDaytime = Night
```

The file should now look like this:

![image](https://user-images.githubusercontent.com/16554392/189486259-835da10a-7e39-403c-af34-f1c0ac0c696b.png)

