using System.Collections;
using UnityEngine;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;
using SpawnThat.Spawners.WorldSpawner.Sync;

namespace SpawnThat.Spawners.WorldSpawner.Startup;

internal static class WorldSpawnerSetup
{
    public static void SetupWorldSpawners()
    {
        LifecycleManager.OnSinglePlayerInit += LoadBepInExConfigs;
        LifecycleManager.OnDedicatedServerInit += LoadBepInExConfigs;
        LifecycleManager.OnFindSpawnPointFirstTime += DelayedConfigRelease;

        SpawnerConfigurationManager.OnLateConfigure += ApplyBepInExConfigs;

        WorldSpawnerSyncSetup.Configure();
    }

    internal static void LoadBepInExConfigs()
    {
        SpawnSystemConfigurationManager.LoadAllConfigurations();
        WorldSpawnerManager.WaitingForConfigs = false;
    }

    internal static void ApplyBepInExConfigs(ISpawnerConfigurationCollection spawnerConfigs)
    {
        SpawnSystemConfigApplier.ApplyBepInExConfigs(spawnerConfigs);
    }

    internal static void DelayedConfigRelease()
    {
        _ = Game.instance.StartCoroutine(StopWaiting());
    }

    public static IEnumerator StopWaiting()
    {
        yield return new WaitForSeconds(8);

        WorldSpawnerManager.WaitingForConfigs = false;
    }
}
