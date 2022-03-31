
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

Documentation can be found on the [Spawn That github page](https://asharppen.github.io/Valheim.SpawnThat/).

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

# v1.0.0 Details

## Config changes

`Enabled` now toggles the actual spawner on/off. This can be used to both disable local spawners and world spawner entries.

`TemplateEnabled` added, which behaves like the old `Enabled` by disabling the configuration entry itself.

## API support

Spawn That now supports configurations by code. Configurations are merged with configs from file. File configurations will be applied last, ensuring that users can still override the settings made by mods.

A nuget release has also been made available.

Configurations are applied once pr world entered, and synced from server-side.

API is more feature-rich than the config files though.

Example:
```cs
public class Plugin : BaseUnityPlugin
{
	public void Awake()
	{
		// Register for configuration event.
		SpawnThat.Spawners.SpawnerConfigurationManager.OnConfigure += MySpawnerConfigurations;
	}

	public void MySpawnerConfigurations(ISpawnerConfigurationCollection spawnerConfig)
	{
		// Add a new world spawner
		spawnerConfig
			.ConfigureWorldSpawner(123)
			.SetPrefabName("Skeleton")
			.SetMinLevel(2)
			.SetMaxLevel(3);

		// Modify an existing vanilla spawner
		spawnerConfig
			.ConfigureWorldSpawner(1)
			.SetSpawnInterval(TimeSpan.FromSeconds(30))
			.SetPackSizeMin(3)
			.SetPackSizeMax(10);

		// Modify a local spawner
		spawnerConfig
			.ConfigureLocalSpawnerByLocationAndCreature("Runestone_Boars", "Boar")
			.SetPrefabName("Skeleton");
	}
}
```

# Support

If you feel like it

<a href="https://www.buymeacoffee.com/asharppen"><img src="https://img.buymeacoffee.com/button-api/?text=Buy me a coffee&emoji=&slug=asharppen&button_colour=FFDD00&font_colour=000000&font_family=Cookie&outline_colour=000000&coffee_colour=ffffff" /></a>

# Changelog: 
- v1.0.5:
	- Improved world spawner debug file output. Now tries to output not just the default settings, but the custom too. Including integrations.
	- Fixed world spawner post change debug file not showing the configuration ID's, but instead the internal index they were added at.
	- Fixed world spawner cfg load adding unnecessary option.
	- Fixed faction, biome and cllc setting parsing being case-sensitive. Eg., setting biome as Blackforest would fail to parse as biome BlackForest.
 	- Added installation check for YamlDotNet dll, to ensure it is present.
v1.0.4:
	- Fixed dungeon room names not being cleaned before registration, causing issues with local spawners matching.
- v1.0.3:
	- Fixed local spawner file-configs not being properly matched with spawners.
	- Fixed error when printing world spawners with missing prefab to debug file.
	- Fixed error when having file-configurations with integrations not installed.
	- Added robustness to API when setting up configurations for uninstalled integrations.
	- Added robustness for v0.207.15
- v1.0.2:
	- Fixed error spawn from world spawners, when joining a server with no world spawner configurations.
- v1.0.1:
	- Fixed broken config sync.
- v1.0.0:
	- WorldSpawner config `Enabled` now toggles the actual spawn entry on/off.
	- Added WorldSpawner option `TemplateEnabled` to replace `Enabled`.
	- LocalSpawner config `Enabled` now toggles the spawner itself on/off.
	- Added LocalSpawner option `TemplateEnabled` to replace `Enabled`.
	- Added API for using Spawn That by code.
	- Debug files are now printed to BepInEx/Debug by default. Output folder is configurable.
	- A ton of internal work and improvements.
	- Moved documentation to https://asharppen.github.io/Valheim.SpawnThat/. Documentation will be updated here from now on.
- v0.11.6:
	- It's the season of bugs! World spawner templates are now instantiated on entering world, meaning changes applied are no longer carried between worlds / re-entering. This is hopefully getting changed by IG in the future.
	- Fixed local spawners not honouring "Enabled=false". Configs were still attempted applied.
	- Fixed leftover optimizations causing spawners to get disabled in biomes outside the one player logged into.
- v0.11.5:
	- More v0.205.5 fixes. World spawners were changed from no longer being per zone, but properly global, meaning Spawn That was reapplying its changes more than once.
- v0.11.4:
	- Fixes for Valheim v0.205.5
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
