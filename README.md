
# Spawn That! 

This is an advanced tool for configuring all world spawners.

With this, it is possible to change almost all of the default settings for the way spawners work in Valheim.
Want to have a world without trolls? Possible! (probably)
Want to have a world with ONLY trolls? Possible! (almost)
Want to have a world where greydwarves only spawn at night? Possible!
Just want to have more/less of a mob type? Simple modifiers exist!

# Features

- Change spawning rates of specific mobs
- Replace existing spawn configurations throughout the world
- Set almost any of the default parameters the game uses
- Add your own spawn configuration to the world
- Modify the localized spawners by mob type and location
- Dump existing game templates as files using the same format as the mod configs. 
	- Easy to copy-paste and change the parts you want.
	- Investigate what the world throws at you.
- Server-side configs
- Modify the spawners in camps, villages and dungeons
- Conditions and settings specific to integrated mods:
	- [Creature Level and Loot Control](https://valheim.thunderstore.io/package/Smoothbrain/CreatureLevelAndLootControl/)
	- [MobAILib](https://www.nexusmods.com/valheim/mods/1188)
	- [Epic Loot](https://valheim.thunderstore.io/package/RandyKnapp/EpicLoot/)

# Documentation

Documentation can be found on the [Spawn That! wiki](https://github.com/ASharpPen/Valheim.SpawnThat/wiki).


# Example 

```INI
[WorldSpawner.321]
Name = Angry Test Boars
PrefabName = Boar
Biomes = Meadows
Enabled = true
HuntPlayer=true
MaxSpawned = 30
SpawnInterval = 1
SpawnChance = 100
SpawnDuringDay = true
SpawnDuringNight = true
ConditionLocation = Runestone_Boars
ConditionDistanceToCenterMin = 500
ConditionAreaSpawnChance = 50

[WorldSpawner.321.CreatureLevelAndLootControl]
SetInfusion=Fire

[WorldSpawner.321.EpicLoot]
ConditionNearbyPlayerCarryLegendaryItem = HeimdallLegs
```

# Changelog: 
- v0.11.3:
	- Fixes for Valheim v0.202.14
- v0.11.2: 
	- Fixed the setting SpawnInPlayerBase not being assigned to local spawners.
- v0.11.1: 
	- Fixed issue with local spawners being disabled due to missing location info.
	- Made local spawners less demanding of config application. Should revert to default spawn if Spawn That fails to apply its changes.
- v0.11.0: 
	- Added region labelling for map biomes. Will now scan for connected biome zones, and assign an id for that whole area.
	- Added condition for spawning only in specified areas. Intended as a world specific setting. For those who have been waiting, this is the option to use for designated monster islands.
	- Added condition for spawning in an area, chance is pr area and only rolled once, to allow for variety in spawning across the world.
	- Added console commands for areas.
	- Added conditions for epic loot based items on nearby players.
	- Added support for spawning non-ai entities. MaxSpawned and SpawnDistance should now work properly for any prefab. SpawnDistance should work for any distance now, be aware this may cause performance issues if set too high.
	- Documentation moved to mod wiki.
- v0.10.1: 
	- Fixed issue with world-spawner mobs not spawning in mountains.
- v0.10.0: 
	- Optimized network package sizes.
	- Added initial support for [MobAILib](https://www.nexusmods.com/valheim/mods/1188).
	- Added setting "SetTamed" for local- and world spawners.
	- Added setting "SetTamedCommandable" for local- and world spawners.
	- Added world spawner condition "" for nearby players having status effect.
- v0.9.1: 
	- Fixed issue with too early access of location info. Should resolve issue with local spawners not spawning creatures.
- v0.9.0: 
	- Added setting "UseDefaultLevels" to CLLC integraiton, to let Spawn That set levels.
	- Added setting "SetRelentless".
	- Added setting "SetTryDespawnOnConditionsInvalid".
	- Added setting "SetTryDespawnOnAlert".
	- Added setting "TemplateId", for assigning a specific identifier to mobs spawned by a template, for other mods to use (intended for future Drop That setting).
	- Added condition "ConditionNearbyPlayersNoiseThreshold".
- v0.8.2: 
	- Added more helpful warning- and error messages for when configurations are incorrectly set up.
	- Changed StopTouchingMyConfigs to be set to true by default when spawn_that.cfg is created. Due to the massive loading time impact of large config files.
- v0.8.1: 
	- Additional error handling for conditions. Should help fix potential errors coming out of the newly added player conditions.
- v0.8.0: 
	- Added world spawner condition for nearby players carried items.
	- Added world spawner condition for nearby players carried valuables.
	- Added option for assigning faction for world- and local spawner entries.
	- Added faction to pre-change debug files.
	- Changed when configs are applied to spawners, for increased compatibility with mods adding prefabs.
	- Additional error checks.
- v0.7.1: 
	- Fixed simple config being generated with wrong prefab-names.
	- Additional error handling.
	- Changed location info from server to client slightly, to hopefully stop issues with local spawners.
- v0.7.0: 
	- Added support for Creature Level and Loot Control (CLLC)
	- Added CLLC creature effect options for world- and local spawners.
	- Added CLLC world level condition to world spawners.
- v0.6.0: 
	- Added world spawner condition - RequiredNotGlobalKey
	- Added support for supplemental world- and local spawner config files.
	- Local spawner configs now work in multiplayer.
- v0.5.1: 
	- Added a (probably temporary) config to not run local spawner configs, due to issues with multiplayer.
	- Fixed error message from local spawners.
	- Removed a couple of pointless warnings.
- v0.5.0: 
	- Added new local spawner defaults to file dumps.
	- Added condition for world spawners. World age in days.
	- Added console command for getting current room in which player is standing.
	- Added configuration for Dungeons, Camps and Villages. All are considered local spawners.
	- Lots of bug fixes. Spawners should have an easier time having configuration "stick" now.
- v0.4.0: 
	- New condition for world spawners. Distance to center min/max.
	- Simple config initialized with creatures on file creation by default.
	- Various attempts at stabilizing and guarding code.
- v0.3.0: 
	- Server-to-client config sync added
- v0.2.0: 
	- Initial release
