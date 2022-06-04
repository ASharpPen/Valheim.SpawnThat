using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpawnThat.Options.Modifiers;
using SpawnThat.Utilities.Enums;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration;

[TestClass]
public class DestructibleSpawnerConfigurationTests
{
    [TestMethod]
    public void CanBuild()
    {
        var config = new DestructibleSpawnerConfiguration();

        var spawner = config.CreateBuilder();

        spawner.SetTemplateName("Meadow Spawners")
                .SetIdentifierBiome(Heightmap.Biome.Meadows)
                .SetSpawnInterval(TimeSpan.FromSeconds(5))
                ;

        spawner.GetSpawnBuilder(0)
            .SetPrefabName("Troll")
            .SetSpawnWeight(5)
            .SetConditionAllOfGlobalKeys(GlobalKey.Bat, GlobalKey.Troll)
            ;

        spawner.GetSpawnBuilder(1)
            .SetPrefabName("Wolf")
            .SetSpawnWeight(1)
            ;

        var spawnerSettings = new DestructibleSpawnerSettings()
        {
            SpawnInterval = TimeSpan.FromSeconds(5),
            SetPatrol = true,

        };

        var spawnSettings = new DestructibleSpawnSettings
        {
            PrefabName = "Boar",
            Modifiers = new[]
            {
                    new ModifierSetTamed(true)
                },
        };

        var spawner2 = config.CreateBuilder()
            .WithSettings(spawnerSettings)
            .SetIdentifierName("Spawner_GreydwarfNest")
            ;

        spawner2.GetSpawnBuilder(0)
            .WithSettings(spawnSettings)
            .SetLevelMin(1)
            .SetLevelMax(1)
            .SetSpawnWeight(5)
            ;

        spawner2.GetSpawnBuilder(1)
            .WithSettings(spawnSettings)
            .SetLevelMin(3)
            .SetLevelMax(3)
            .SetSpawnWeight(1)
            ;

        config.Build();
    }
}
