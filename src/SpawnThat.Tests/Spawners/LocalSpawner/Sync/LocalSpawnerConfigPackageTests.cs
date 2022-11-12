using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpawnThat.Spawners.LocalSpawner;
using SpawnThat.Spawners.LocalSpawner.Configuration;
using SpawnThat.Spawners.LocalSpawner.Managers;
using SpawnThat.Spawners.LocalSpawner.Models;
using SpawnThat.Spawners.LocalSpawner.Sync;
using static Heightmap;

namespace SpawnThat.Tests.Spawners.LocalSpawner.Sync;

[TestClass]
public class LocalSpawnerConfigPackageTests
{
    [TestInitialize]
    public void Init() => Cleanup();

    [TestCleanup]
    public void Cleanup()
    {
        LocalSpawnTemplateManager.TemplatesByLocation.Clear();
        LocalSpawnTemplateManager.TemplatesByRoom.Clear();
        LocalSpawnTemplateManager.TemplatesBySpawnerName.Clear();
    }

    [TestMethod]
    public void CanSync()
    {
        try
        {
            // Setup

            LocalSpawnerConfiguration configuration = new();
            configuration.GetBuilder(new LocationIdentifier("Runestone_Boars", "Boar"))
                .SetPrefabName("Boar")
                .SetEnabled(true)
                .SetSpawnInterval(TimeSpan.FromSeconds(3))
                .SetPatrolSpawn(true)
                .SetSpawnInPlayerBase(true)
                .SetMinLevel(1)
                .SetMaxLevel(3)
                .SetLevelUpChance(10)
                .SetSpawnDuringNight(true)
                .SetSpawnDuringDay(true)
                .SetConditionPlayerWithinDistance(60)
                .SetConditionPlayerNoise(0)
                .SetModifierFaction(Character.Faction.Boss)
                .SetModifierTamed(true)
                .SetModifierTamedCommandable(true);
            configuration.GetBuilder(new RoomIdentifier("Runestone_Boars", "Boar"))
                .SetPrefabName("Boar")
                .SetEnabled(true)
                .SetSpawnInterval(TimeSpan.FromSeconds(3))
                .SetPatrolSpawn(true)
                .SetSpawnInPlayerBase(true)
                .SetMinLevel(1)
                .SetMaxLevel(3)
                .SetLevelUpChance(10)
                .SetSpawnDuringNight(true)
                .SetSpawnDuringDay(true)
                .SetConditionPlayerWithinDistance(60)
                .SetConditionPlayerNoise(0)
                .SetModifierFaction(Character.Faction.Boss)
                .SetModifierTamed(true)
                .SetModifierTamedCommandable(true);

            configuration.Build();

            // Sync

            var package = new FakeLocalSpawnerConfigPackage();

            var serialized = package.FakePack();

            Cleanup();

            package.FakeUnpack(serialized);

            // Verify

            var template1 = LocalSpawnTemplateManager.GetTemplate(new LocationIdentifier("Runestone_Boars", "Boar"));
            Assert.IsNotNull(template1);

            var template2 = LocalSpawnTemplateManager.GetTemplate(new RoomIdentifier("Runestone_Boars", "Boar"));
            Assert.IsNotNull(template2);
        }
        finally
        {
            LocalSpawnTemplateManager.TemplatesByLocation.Clear();
        }
    }

    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void CanSyncEnabled(bool enabled)
    {
        // Setup
        var identifier = new LocationIdentifier("Runestone_Boars", "Boar");

        LocalSpawnerConfiguration configuration = new();
        configuration
            .GetBuilder(identifier)
            .SetEnabled(enabled);

        configuration.Build();

        // Sync
        FakeLocalSpawnerConfigPackage package = new();

        var serialized = package.FakePack();

        Cleanup();

        package.FakeUnpack(serialized);

        // Verify
        var template = LocalSpawnTemplateManager.GetTemplate(identifier);

        Assert.IsNotNull(template);
        Assert.AreEqual(enabled, template.Enabled);
    }
}
