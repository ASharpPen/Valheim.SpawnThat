# Overview

Spawn That works off 5 different types of configuration files.

- `spawn_that.cfg`
	- General Configurations. Options for debugging and global controls can be found in there.
- `spawn_that.world_spawners_advanced.cfg`
	- Configurations for world spawners will be loaded from here.
- `spawn_that.local_spawners_advanced.cfg`
	- Configurations for local spawners will be loaded from here.
- `spawn_that.simple.cfg`
	- Simplified configurations for world spawners will be loaded here.
- `spawn_that.spawnarea_spawners.cfg`
	- Configurations for SpawnArea spawners will be loaded from here.

Additionally, both world and local spawner configs can be placed in separate "supplemental" files, to allow for easier separation and management.
The supplemental files must be named following the pattern:
- World Spawners: 
	- `spawn_that.world_spawners.*.cfg`
	- `spawn_that.world_spawners.my_configuration_file.cfg`
- Local Spawners: 
	- `spawn_that.local_spawners.*.cfg`
	- `spawn_that.local_spawners.my_configuration_file.cfg`
- SpawnArea Spawners: 
	- `spawn_that.spawnarea_spawners.*.cfg`
	- `spawn_that.spawnarea_spawners.my_configuration_file.cfg`

These "supplemental" configuration files can be placed in any subfolder, as long as they are in the bepinex/config folder somewhere.