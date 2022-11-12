using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpawnThat.Spawners.WorldSpawner;
using SpawnThat.Spawners.WorldSpawner.Configurations;
using SpawnThat.Spawners.WorldSpawner.Managers;
using SpawnThat.Spawners.WorldSpawner.Sync;
using SpawnThat.Utilities.Enums;
using static Heightmap;

namespace SpawnThat.Tests.Spawners.WorldSpawner.Sync;

[TestClass]
public class WorldSpawnerConfigPackageTests
{
    [TestInitialize]
    public void Init() => Cleanup();

    [TestCleanup]
    public void Cleanup()
    {
        WorldSpawnTemplateManager.TemplatesById.Clear();
    }

    [TestMethod]
    public void CanSync()
    {
        WorldSpawnerConfiguration configuration = new();
        configuration.GetBuilder(1)
            .SetSpawnDuringDay(true)
            .SetSpawnDuringNight(false)
            .SetMaxLevel(3);
        configuration.GetBuilder(2)
            .SetConditionAreaIds(123)
            .SetPrefabName("Boar")
            .SetTemplateEnabled(true);
        configuration.GetBuilder(3)
            .SetConditionAreaIds(125)
            .SetConditionBiomes(Biome.Meadows, Biome.Ocean);

        configuration.Build();

        FakeWorldSpawnerConfigPackage package = new();

        var serialized = package.FakePack();

        Cleanup();

        package.FakeUnpack(serialized);

        Assert.IsTrue(WorldSpawnTemplateManager.TemplatesById.Count > 0);
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void CanSyncEnabled(bool enabled)
    {
        // Setup
        WorldSpawnerConfiguration configuration = new();
        configuration.GetBuilder(1)
            .SetEnabled(enabled);

        configuration.Build();

        // Sync
        FakeWorldSpawnerConfigPackage package = new();

        var serialized = package.FakePack();

        Cleanup();

        package.FakeUnpack(serialized);

        // Verify
        var template = WorldSpawnTemplateManager.TemplatesById[1];

        Assert.AreEqual(template.Enabled, enabled);
    }

    [TestMethod]
    public void TestSize()
    {
        WorldSpawnerConfiguration configuration = new();

        for(uint i = 1; i < 2; ++i)
        {
            configuration.GetBuilder(i)
                .SetPrefabName("MySpawner" + i)
                .SetMaxLevel(3)
                .SetConditionEnvironments(EnvironmentName.Ashrain, EnvironmentName.Clear)
                .SetConditionBiomesAll()
                .SetSpawnDuringDay(true)
                .SetSpawnDuringNight(true)
                .SetConditionDistanceToCenter(500 * (int)i, 2500 * (int)i)
                .SetConditionWorldAge(1 * (int)i, 200 * (int)i)
                .SetGlobalKeysRequiredMissing("Test1", "Test2")
                .SetCllcConditionWorldLevel(1, 3)
                .SetConditionLocation("TestLoc1", "TestLoc2")
                .SetConditionAltitude(5, 100)
                .SetConditionNearbyPlayersNoise(25, 100)
                .SetConditionNearbyPlayersCarryValue(100, 500)
                .SetPositionConditionLocation("Loc123", "Loc6432")
                .SetMaxSpawned(10)
                .SetModifierFaction(Character.Faction.Undead)
                .SetSpawnChance(50)
                .SetSpawnInterval(TimeSpan.FromHours(1))
                .SetTemplateEnabled(true)
                .SetTemplateName("MyTemplate" + i)
                .SetPackSizeMin(2 * i)
                .SetPackSizeMax(4 * i)
                .SetPackSpawnCircleRadius(5);
        }

        configuration.Build();

        var package = new FakeWorldSpawnerConfigPackage();

        var serialized = package.FakePack();

        Cleanup();

        package.FakeUnpack(serialized);

        var template = WorldSpawnTemplateManager.TemplatesById[1];

        Assert.IsTrue(WorldSpawnTemplateManager.TemplatesById.Count > 0);
    }
}
