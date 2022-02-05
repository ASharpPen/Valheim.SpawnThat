using System.Collections;
using UnityEngine;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners.LocalSpawner.Configuration.BepInEx;
using SpawnThat.Spawners.LocalSpawner.Sync;
using SpawnThat.Spawners.LocalSpawner.Managers;

namespace SpawnThat.Spawners.LocalSpawner.Startup;

internal static class LocalSpawnerSetup
{
    public static void SetupLocalSpawners()
    {
        LifecycleManager.OnSinglePlayerInit += LoadBepInExConfigs;
        LifecycleManager.OnDedicatedServerInit += LoadBepInExConfigs;
        LifecycleManager.OnFindSpawnPointFirstTime += DelayedConfigRelease;

        SpawnerConfigurationManager.OnLateConfigure += ApplyBepInExConfigs;

        LocalSpawnerSyncSetup.Configure();
    }

    private static void LoadBepInExConfigs()
    {
        CreatureSpawnerConfigurationManager.LoadAllConfigurations();
        LocalSpawnerManager.WaitingForConfigs = false;
    }

    private static void ApplyBepInExConfigs(ISpawnerConfigurationCollection spawnerConfigs)
    {
        CreatureSpawnerConfigApplier.ApplyBepInExConfigs(spawnerConfigs);
    }

    internal static void DelayedConfigRelease()
    {
        _ = Game.instance.StartCoroutine(StopWaiting());
    }

    public static IEnumerator StopWaiting()
    {
        yield return new WaitForSeconds(8);

        LocalSpawnerManager.WaitingForConfigs = false;
    }
}
