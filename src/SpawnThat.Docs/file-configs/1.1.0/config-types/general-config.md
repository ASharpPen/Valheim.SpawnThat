# General Config

General configuration options.
Belongs to the file `spawn_that.cfg`

```toml
[Datamining]

## Writes all locations loaded to a file, sectioned by the biome in which they can appear.
# Setting type: Boolean
# Default value: false
WriteLocationsToFile = false

[Debug]

## Enable debug logging.
# Setting type: Boolean
# Default value: false
DebugLoggingOn = false

## Enables in-depth logging. Note, this might generate a LOT of log entries.
# Setting type: Boolean
# Default value: false
TraceLoggingOn = false

## Prints a set of pngs showing the area id's assigned by Spawn That to each biome 'area'. Each pixel's hex value corresponds to an area id, when converted into a decimal.
# Setting type: Boolean
# Default value: false
PrintAreaMap = false

## Prints a map of the biome of each zone.
# Setting type: Boolean
# Default value: false
PrintBiomeMap = false

## Prints maps marking where each configured world spawn template can spawn. This will be done for every config entry.
# Setting type: Boolean
# Default value: false
PrintFantasticBeastsAndWhereToKillThem = false

## Folder path to write to. Root folder is BepInEx.
# Setting type: String
# Default value: Debug
DebugFileFolder = Debug

[LocalSpawner]

## Writes local spawners to a file before applying configuration changes.
# Setting type: Boolean
# Default value: false
WriteSpawnTablesToFileBeforeChanges = false

## If true, locations with multiple spawners with duplicate creatures will be listed individually, instead of being only one of each creature.
# Setting type: Boolean
# Default value: false
DontCollapseFile = false

## Toggles if Spawn That changes to local spawners will be run or not.
# Setting type: Boolean
# Default value: true
Enable = true

[Simple]

## If true, fills in simple cfg with a list of creatures when file is created.
# Setting type: Boolean
# Default value: true
InitializeWithCreatures = true

[SpawnAreaSpawner]

## Writes SpawnArea spawners to a file before applying configuration changes.
# Setting type: Boolean
# Default value: false
WriteSpawnTablesToFileBeforeChanges = false

## Writes SpawnArea spawner configs loaded in Spawn That to file, before applying to spawners in game.
## These are the configs loaded from files and set by other mods, after being merged and prepared internally in Spawn That.
# Setting type: Boolean
# Default value: false
WriteConfigsToFile = false

[WorldSpawner]

## If true, removes all existing world spawner templates.
# Setting type: Boolean
# Default value: false
ClearAllExisting = false

## If true, will never override existing spawners, but add all custom configurations to the list.
# Setting type: Boolean
# Default value: false
AlwaysAppend = false

## Writes world spawner templates to a file, before applying custom changes.
# Setting type: Boolean
# Default value: false
WriteSpawnTablesToFileBeforeChanges = false

## Writes world spawner templates to a file after applying configuration changes.
# Setting type: Boolean
# Default value: false
WriteSpawnTablesToFileAfterChanges = false

## Writes world spawner configs loaded in Spawn That to file, before applying to spawners in game.
## These are the configs loaded from files and set by other mods, after being merged and prepared internally in Spawn That.
# Setting type: Boolean
# Default value: false
WriteConfigsToFile = false
```
