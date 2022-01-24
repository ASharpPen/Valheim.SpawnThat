using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Valheim.SpawnThat.Core.Network;
using Valheim.SpawnThat.Spawners;
using Valheim.SpawnThat.Spawners.LocalSpawner;
using Valheim.SpawnThat.Spawners.LocalSpawner.Configuration;
using Valheim.SpawnThat.Spawners.LocalSpawner.Models;
using Valheim.SpawnThat.Spawners.LocalSpawner.Sync;
using static Heightmap;

namespace Valheim.SpawnThat.Tests.Spawners.LocalSpawner.Sync;

[TestClass]
public class LocalSpawnerConfigPackageTests
{
    [TestMethod]
    public void CanSync()
    {
        try
        {
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
                .SetSpawnAtDay(true)
                .SetSpawnAtNight(true)
                .SetConditionPlayerDistance(60)
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
                .SetSpawnAtDay(true)
                .SetSpawnAtNight(true)
                .SetConditionPlayerDistance(60)
                .SetConditionPlayerNoise(0)
                .SetModifierFaction(Character.Faction.Boss)
                .SetModifierTamed(true)
                .SetModifierTamedCommandable(true);

            configuration.Build();

            LocalSpawnerConfigPackage package = new();

            var zpack = package.Pack();

            zpack.m_stream.Position = 0L;
            LocalSpawnTemplateManager.TemplatesByLocation.Clear();

            CompressedPackage.Unpack<LocalSpawnerConfigPackage>(zpack);

            Assert.IsTrue(LocalSpawnTemplateManager.TemplatesByLocation.Count > 0);
        }
        finally
        {
            LocalSpawnTemplateManager.TemplatesByLocation.Clear();
        }
    }
}
