using System.Collections;
using UnityEngine;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;
using SpawnThat.Spawners.WorldSpawner.Sync;
using SpawnThat.Spawners.WorldSpawner.Managers;
using SpawnThat.Configuration;
using SpawnThat.Spawners.WorldSpawner.Debug;

namespace SpawnThat.Spawners.WorldSpawner.Startup;

internal static class WorldSpawnerSetup
{
    public static void SetupWorldSpawners()
    {
        LifecycleManager.OnSinglePlayerInit += LoadBepInExConfigs;
        LifecycleManager.OnDedicatedServerInit += LoadBepInExConfigs;
        LifecycleManager.OnFindSpawnPointFirstTime += DelayedConfigRelease;

        SpawnerConfigurationManager.OnLateConfigure += ApplyBepInExConfigs;

        SpawnerConfigurationManager.OnConfigureFinished += WriteConfigsToDisk;

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

    internal static void WriteConfigsToDisk()
    {
        if (ConfigurationManager.GeneralConfig?.WriteWorldSpawnerConfigsToFile.Value == true)
        {
            TemplateWriter.WriteToDiskAsToml();
        }
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
