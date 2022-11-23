using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpawnThat.Spawners.SpawnAreaSpawner.Configuration;
using SpawnThat.Spawners.SpawnAreaSpawner.Managers;
using SpawnThat.Spawners.WorldSpawner.Configurations;
using SpawnThat.Spawners.WorldSpawner.Managers;
using SpawnThat.Spawners.WorldSpawner.Sync;
using SpawnThat.Utilities.Enums;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Sync;

[TestClass]
public class SpawnAreaSpawnerConfigPackageTests
{
    [TestInitialize]
    public void Init()
    {
        SpawnAreaSpawnerManager.Templates.Clear();
    }

    [TestCleanup]
    public void Cleanup()
    {
        SpawnAreaSpawnerManager.Templates.Clear();
    }

    [TestMethod]
    public void CanSync()
    {
        // Setup
        var config = new SpawnAreaSpawnerConfiguration();

        var spawner = config.CreateBuilder();

        spawner
            .SetTemplateName("Meadow Spawners")
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

        config.Build();

        // Sync

        FakeSpawnAreaSpawnerConfigPackage package = new();

        var serialized = package.FakePack();

        SpawnAreaSpawnerManager.Templates.Clear();

        package.FakeUnpack(serialized);

        // Verify

        var template = SpawnAreaSpawnerManager.Templates.FirstOrDefault();

        Assert.IsNotNull(template);
        Assert.AreEqual("Meadow Spawners", template.TemplateName);
        Assert.IsNotNull(template.Spawns);
        Assert.AreEqual(2, template.Spawns.Count);
        Assert.AreEqual("Troll", template.Spawns[0].PrefabName);
        Assert.AreEqual("Wolf", template.Spawns[1].PrefabName);
    }


    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void CanSyncEnabled(bool enabled)
    {
        // Setup
        SpawnAreaSpawnerConfiguration configuration = new();
        configuration
            .CreateBuilder()
            .GetSpawnBuilder(0)
                .SetEnabled(enabled);

        configuration.Build();

        // Sync
        FakeSpawnAreaSpawnerConfigPackage package = new();

        var serialized = package.FakePack();

        SpawnAreaSpawnerManager.Templates.Clear();

        package.FakeUnpack(serialized);

        // Verify
        var template = SpawnAreaSpawnerManager.Templates.First();
        var spawn = template.Spawns[0];

        Assert.AreEqual(enabled, spawn.Enabled);
    }
}
