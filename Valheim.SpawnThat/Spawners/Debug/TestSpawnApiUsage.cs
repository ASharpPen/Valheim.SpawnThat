
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Integrations.EpicLoot.Models;

namespace Valheim.SpawnThat.Spawners.Debug;

internal static class TestSpawnApiUsage
{
    public static void Setup()
    {
        SpawnerConfigurationManager.OnConfigure += AddNewSpawner;
    }

    public static void AddNewSpawner(ISpawnerConfigurationCollection spawnerConfigs)
    {
        // Configure local spawners
        spawnerConfigs
            .ConfigureLocalSpawnerByName("Spawner_Goblin")
            .SetMinLevel(1);

        spawnerConfigs
            .ConfigureLocalSpawnerByLocationAndCreature("GoblinCamp1", "Goblin")
            .SetMinLevel(3)
            .SetMaxLevel(5)
            .SetPrefabName("Skeleton");
    }

    public static void AddNewSpawner()
    {
        SpawnerConfigurationManager.SubscribeConfiguration((spawnerConfigs) => 
        {
            // Configure local spawners
            spawnerConfigs
                .ConfigureLocalSpawnerByName("Spawner_Goblin")
                .SetMinLevel(1);

            spawnerConfigs
                .ConfigureLocalSpawnerByLocationAndCreature("GoblinCamp1", "Goblin")
                .SetMinLevel(3)
                .SetMaxLevel(5)
                .SetPrefabName("Skeleton");
        });

        SpawnerConfigurationManager.SubscribeConfiguration(spawnerConfigs =>
        {
            // Configure world spawners
            spawnerConfigs
                .ConfigureWorldSpawner(1001)
                .SetPrefabName("Skeleton")
                .SetMaxSpawned(100)
                .SetMinLevel(1)
                .SetMaxSpawned(5);
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
                });
        });

        SpawnerConfigurationManager.SubscribeConfiguration(spawnerConfigs =>
        {
            spawnerConfigs
                .ConfigureWorldSpawner(1)
                .SetMinLevel(2)
                .SetMaxSpawned(20)
                .SetPackSizeMin(5)
                .SetPackSizeMax(10);
        });
    }

    public static void RemoveSpawner()
    {
        SpawnerConfigurationManager.SubscribeConfiguration(spawnerConfigs =>
        {
            spawnerConfigs
                .ConfigureWorldSpawner(2)
                .SetEnabled(false);
                /*
                .AddPostConfiguration(template =>
                {
                    // Try to force it
                    template.Enabled = false;
                });
                */
        });
    }

    public static void ConfigureUsingIntegration()
    {
        SpawnerConfigurationManager.SubscribeConfiguration(spawnerConfigs =>
        {
            // Epic Loot for World Spawner
            spawnerConfigs
                .ConfigureWorldSpawner(1)
                .AddEpicLootConditionNearbyPlayersCarryItemWithRarity(30, new[] { EpicLootRarity.Rare });
        });
    }
}