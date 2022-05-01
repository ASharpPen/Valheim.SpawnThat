using System.Collections;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners.DestructibleSpawner.Configuration.BepInEx;
using SpawnThat.Spawners.DestructibleSpawner.Debug;
using SpawnThat.Spawners.DestructibleSpawner.Managers;
using SpawnThat.Spawners.DestructibleSpawner.Sync;
using UnityEngine;

namespace SpawnThat.Spawners.DestructibleSpawner.Startup;

internal static class DestructibleSpawnerSetup
{
    public static void SetupDestructibleSpawners()
    {
        LifecycleManager.OnSinglePlayerInit += LoadFileConfigs;
        LifecycleManager.OnDedicatedServerInit += LoadFileConfigs;
        LifecycleManager.OnFindSpawnPointFirstTime += DelayedConfigRelease;

        SpawnerConfigurationManager.OnLateConfigure += ApplyFileConfigs;

        LifecycleManager.OnFindSpawnPointFirstTime += SpawnAreaDataGatherer.ScanAndPrint;

        DestructibleSpawnerSyncSetup.Configure();
    }

    internal static void LoadFileConfigs()
    {
        DestructibleSpawnerBepInExCfgManager.Load();
    }

    internal static void ApplyFileConfigs(ISpawnerConfigurationCollection spawnerConfigs)
    {
        DestructibleSpawnerConfigApplier.ApplyBepInExConfigs(spawnerConfigs);
    }

    internal static void DelayedConfigRelease()
    {
        _ = Game.instance.StartCoroutine(StopWaiting());
    }

    public static IEnumerator StopWaiting()
    {
        yield return new WaitForSeconds(8);

        DestructibleSpawnerManager.DelaySpawners = false;
    }
}