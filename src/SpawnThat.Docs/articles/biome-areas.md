# Biome Areas

Introduced in v0.11.0, the world map is now scanned on world start and connected biomes grouped up.

The world is separated into squares of 64x64 meters, and then each is checked for its biome to generate an internal map like this.

![](https://user-images.githubusercontent.com/16554392/122655785-775de680-d155-11eb-99f4-4a1b49fdc8e8.png)

This map does not take into account sea-levels or other identifiers such as altitude that you might see colouring a normal map, so it can be a bit hard to read.

The map is then scanned for connected colours, in a 4-way check. IE. diagonal biomes will not count as connected.

### Example 
An area of squares such as:

| | | | |
| --- | --- | --- | --- | 
| Meadows | Meadows | Meadows | BlackForest |
| BlackForest | BlackForest | Meadows | BlackForest |
| BlackForest | BlackForest | Meadows | Ocean |
| Meadows | Meadows | Meadows | Ocean |

Will get id's assigned something along the lines of this:

| | | | |
| --- | --- | --- | --- | 
| 1 | 1 | 1 | 3 |
| 2 | 2 | 1 | 3 |
| 2 | 2 | 1 | 4 |
| 1 | 1 | 1 | 4 |

The id's are a bit and important part of why the map is scanned.

## Features

That all sounds very nice and number'y I hear you say, but what is it good for?

Well, before the scan we could not really tell much about where we were on the map, apart from position and current biome of the zone we were in.
With the assignment of grouped id's for each zone, we can for example now identify exactly which forest on the map we are in.

This allows for features / conditions based on whole areas. Instead of just a bit of randomness, or very rough distance/biome control, we can now base it on whole regions of the map, and ONLY that region.

For Spawn That, this means:

- Spawn chance can be bound to an area, making exploration more rewarding.
- Spawn conditions can be bound to specific area id's, allowing for very specific control on known maps.

## Spawn chance by area

Since we have unique id's for each area, and a unique id for the map itself, Spawn That has introduced a new spawn condition, `ConditionAreaSpawnChance`.

Basically, for each SpawnThat template loaded, the game can/will roll chance once, for each **area** of the map. The roll always be the same for each area, as it is based on `AreaId + WorldSeed + WorldSpawner.Index`.

Eg. for a template
```INI
[Boar.123]
PrefabName = Boar
```
WorldSpawner.Index is the 123.

To get an idea of where your template will spawn, maps will be printed for each template, with an estimation of which areas the configuration will be allowed to spawn in.

This could look something like this:

![](https://user-images.githubusercontent.com/16554392/122656556-e0485d00-d15b-11eb-951f-1b8b2d44222d.png)

or a more restricted version with a lower `ConditionAreaSpawnChance` like this:

![](https://user-images.githubusercontent.com/16554392/122656558-e3434d80-d15b-11eb-872c-264ef86f3f64.png)

This is not the limit at all. During testing, it was possible to bring down the map to a single zone in which a template was allowed to spawn.

As of writing, the conditions taking into account during the map printing is:
- Biomes
- ConditionLocation
- ConditionDistanceToCenterMin
- ConditionDistanceToCenterMax
- ConditionAreaIds
- ConditionAreaSpawnChance

## Spawn area id's
Since we have unique id's for each area, we can now look them up, and check if the spawner is actually inside one of the whitelisted areas with the condition `ConditionAreaIds`.

Identifying the features can be done by using the console command `spawnthat area`, which will print the area id of the zone the player is currently in.

A more advanced way of retrieving the id's can be done by looking at a full map of id's. The map will automatically be printed on entering the world, if the debug option `PrintAreaMap` is enabled in the [general config](../file-configs/1.1.0/config-types/general-config.md).

The map looks like this:

![](https://user-images.githubusercontent.com/16554392/122656308-a413fd00-d159-11eb-84ed-f6c76969fd76.png)

Now, this is where the tricky part comes in. Each individual pixel is a representation of the matching area id. An image tool such as paint.net can be used to pinpoint the exact values here.

Image of the biome map.

![Biome Map View](https://user-images.githubusercontent.com/16554392/122656466-1507e480-d15b-11eb-8f94-70779afebd33.png)

Image of the biome id map.

![Id Map View](https://user-images.githubusercontent.com/16554392/122656470-2fda5900-d15b-11eb-95bd-aa51df0d1ac8.png)

Take note of the hex field in the biome id map. That '0006CD' is the hexidecimal representation of the area id. Using whatever tool fits you, you can convert that number into the corresponding decimal value as this:

![](https://user-images.githubusercontent.com/16554392/122656494-644e1500-d15b-11eb-9fd8-bbcad30a0197.png)

And finally, we can insert the desired id '1741' into our configuration.

