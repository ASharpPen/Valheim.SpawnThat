# API - Getting Started

Quick'n'dirty explanation of how to get Spawn That set up for your own code.

## Reference SpawnThat:

Reference the SpawnThat dll in your preferred manner.

Spawn That can be added by:
- Install nuget package
- Download from github release https://github.com/ASharpPen/Valheim.SpawnThat/releases

### (Optional) Add dependency:

Add a bepinex dependency to plugin, that ensures your mod is loaded after Spawn That, and also tells BepInEx that Spawn That is expected installed.

```cs
[BepInDependency("asharppen.valheim.spawn_that", BepInDependency.DependencyFlags.HardDependency)]
public class YourModsPlugin : BaseUnityPlugin
```

## Register to configuration event.

Spawn That calls all registered functions with the main configuration collection, when it starts to gather its configurations.

There are currently two ways of registering for this callback:
`SpawnerConfigurationManager.OnConfigure += YourFunction;`
or
`SpawnerConfigurationManager.SubscribeConfiguration(YourFunction);`

The call to registered actions is made every time a singleplayer world is entered or a server is started. So registrations only need to be made once.

```cs
using SpawnThat.Spawners;
...
public class YourModsPlugin : BaseUnityPlugin
{
  void Awake()
  {
    SpawnerConfigurationManager.OnConfigure += MySpawnerConfigurations;
  }
}

public void MySpawnerConfigurations(ISpawnerConfigurationCollection spawnerConfig)
{
}
```

## Add configurations

Configurations are made by retrieving a builder for a desired spawner type.

Usually this has the form of `ISpawnerConfigurationCollection.ConfigureSpawnerType`.

```cs
public void MySpawnerConfigurations(ISpawnerConfigurationCollection spawnerConfig)
{
  // Configure a new world spawner
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
    .ConfigureLocalSpawnerByLocationAndCreature("Runestone_boars", "Boar")
    .SetPrefabName("Skeleton");
}
```

And done.

- Spawn That will make sure the configurations are merged with file ones. 
- The resulting templates after configuration building is done will be synced to players joining a server. 
- Any un-assigned setting will use a default value if this is a new spawner, or use existing settings when overriding an existing spawner (eg., if you want to just increase vanilla boar spawn frequency, only that setting will be touched).