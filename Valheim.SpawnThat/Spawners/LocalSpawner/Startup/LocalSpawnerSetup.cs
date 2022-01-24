using System.Collections;
using UnityEngine;
using Valheim.SpawnThat.Lifecycle;
using Valheim.SpawnThat.Spawners.LocalSpawner.Configuration.BepInEx;
using Valheim.SpawnThat.Spawners.LocalSpawner.Sync;
using Valheim.SpawnThat.Spawners.WorldSpawner;

namespace Valheim.SpawnThat.Spawners.LocalSpawner.Startup;

internal static class LocalSpawnerSetup
{
    public static void SetupLocalSpawners()
    {
        LifecycleManager.OnSinglePlayerInit += LoadBepInExConfigs;
        LifecycleManager.OnDedicatedServerInit += LoadBepInExConfigs;
        LifecycleManager.OnFindSpawnPointFirstTime += DelayedConfigRelease;

        // TODO: This SHOULD be on late configure, but configs doesn't have a concept of null yet, so to avoid overriding everything always, they will be applied first.
        SpawnerConfigurationManager.OnEarlyConfigure += ApplyBepInExConfigs;

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
