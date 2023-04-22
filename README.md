
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