# Tutorial - Making a hierarchy of configs for a SpawnArea spawner

This tutorial will describe how to have multiple configs for SpawnArea spawners, to handle general changes, as well as specific ones at the same time.

First, make sure you have read [Modifying a SpawnArea spawner](modifying-vanilla-spawnarea-spawner.md).

Then, lets create a general change to all greydwarf spawners:

```toml
[GlobalGreydwarfNests]
IdentifyByName = Spawner_GreydwarfNest
ConditionMaxCloseCreatures = 10
SpawnInterval = 1
```

This should modify every single greydwarf nest in the game as is.

But what if we want it to be a bit more relaxed when we are in meadows, and only then?

We can add another config like this:

```toml
[FriendlierGreydwarfNests]
IdentifyByName = Spawner_GreydwarfNest
IdentifyByBiome = Meadows
ConditionMaxCloseCreatures = 2
SpawnInterval = 20
```

Now this is a bit more calm.

When Spawn That notices the greydwarf spawner in game, it will look up all SpawnArea configs that could match that spawner. It then selects the one with most matching identifiers.

In our case, IdentifyByName and IdentifyByBiome of `FriendlierGreydwarfNests` counts as more matches than the IdentifyByName `GlobalGreydwarfNests`.

But what if we want to change every single spawner in a biome to spawn in trolls? Well, we can do:

```toml
[JustTrollsPlease]
IdentifyByBiome = BlackForest
RemoveNotConfiguredSpawns = true

[JustTrollsPlease.0]
PrefabName = Troll
```

However, we now have two configs that could apply to a Spawner_GreydwarfNest spawner. `GlobalGreydwarfNests` identifies it by name, and `JustTrollsPlease` by biome.
To select which config to apply when both have the same number of matching identifiers, the identifiers get an internal "weight" assigned. The combined weight of identifiers is then used to pick the most relevant config.

Identifiers are ranked as:

- IdentifyByBiome: 50
- IdentifyByName: 100
- IdentifyByLocation: 100
- IdentifyByRoom: 200

That means, in our case, `GlobalGreydwarfNests` will get picked, as the weight of `IdentifyByName` is greater than `IdentifyByBiome`.

If we look at a Spawner_GreydwarfNest spawner in meadows, then `FriendlierGreydwarfNests` still gets picked, because it simply has more identifiers than the other configs.

Our final file looks like this:

![image](https://user-images.githubusercontent.com/16554392/190868602-37b427f1-38a0-4729-9db5-6ba13a430edc.png)

Finally, what happens if we have multiple configs, with identical identifiers?

Lets say a mod added a set of changes like this:

```toml
[SomeOtherMod]
IdentifyByName = Spawner_GreydwarfNest

[SomeOtherMod.0]
PrefabName = Boar
SpawnWeight = 5

[SomeOtherMod.1]
PrefabName = Boar
SpawnWeight = 1
LevelMin = 3
LevelMax = 3
```

When the same set of identifiers are detected, Spawn That merges the configs together. In our case, the result would become like this internally, with `GlobalGreydwarfNests` merging with `SomeOtherMod`:

```toml
[GlobalGreydwarfNests]
IdentifyByName = Spawner_GreydwarfNest
ConditionMaxCloseCreatures = 10
SpawnInterval = 1

[GlobalGreydwarfNests.0]
PrefabName = Boar
SpawnWeight = 5

[GlobalGreydwarfNests.1]
PrefabName = Boar
SpawnWeight = 1
LevelMin = 3
LevelMax = 3
```

This is also how to override modded configs (or ones added through code). Use the same identifiers, and then when Spawn That merges the configs the last loaded settings overrides the earlier.