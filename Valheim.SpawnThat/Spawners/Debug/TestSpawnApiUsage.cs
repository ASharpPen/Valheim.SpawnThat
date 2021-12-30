
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.Debug;

internal static class TestSpawnApiUsage
{
    public static void Setup()
    {
        SpawnerConfigurationManager.OnConfigure += AddNewSpawner;
        SpawnerConfigurationManager.OnConfigure += ModifySpawner;
        SpawnerConfigurationManager.OnConfigure += RemoveSpawner;
    }

    public static void AddNewSpawner()
    {
        SpawnerConfigurationManager.SubscribeConfiguration((spawnerConfigs) => 
        {
            // Configure local spawners
            spawnerConfigs.ConfigureLocalSpawnerByName("Spawner_Goblin")
                .SetMinLevel(1);

            spawnerConfigs.ConfigureLocalSpawnerByLocationAndCreature("GoblinCamp1", "Goblin")
                .SetMinLevel(3)
                .SetMaxLevel(5)
                .SetPrefabName("Skeleton");
        });
    }

    public static void ModifySpawner()
    {
        SpawnerConfigurationManager.SubscribeConfiguration((spawnerConfigs) =>
        {
            var location = "GoblinCamp1";
            var creaturePrefabName = "Goblin";

            spawnerConfigs.ConfigureLocalSpawnerByLocationAndCreature(location, creaturePrefabName)
                .SetPrefabName("Skeleton")
                .SetMinLevel(1)
                .AddPostConfiguration((template) =>
                {
                    Log.LogDebug($"Testing post configurations for local spawner configuration [{location}:{creaturePrefabName}]: {template.PrefabName}");

                    template.SpawnConditions.RemoveAll(x => !x.CanRunClientSide);
                });
        });
    }

    public static void RemoveSpawner()
    {

    }
}