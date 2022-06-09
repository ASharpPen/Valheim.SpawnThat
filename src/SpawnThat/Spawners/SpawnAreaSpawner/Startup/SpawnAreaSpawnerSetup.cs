using System.Collections;
using SpawnThat.Configuration;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners.SpawnAreaSpawner.Configuration.BepInEx;
using SpawnThat.Spawners.SpawnAreaSpawner.Debug;
using SpawnThat.Spawners.SpawnAreaSpawner.Managers;
using SpawnThat.Spawners.SpawnAreaSpawner.Sync;
using UnityEngine;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Startup;

internal static class SpawnAreaSpawnerSetup
{
    public static void SetupSpawnAreaSpawners()
    {
        LifecycleManager.OnSinglePlayerInit += LoadFileConfigs;
        LifecycleManager.OnDedicatedServerInit += LoadFileConfigs;
        LifecycleManager.OnFindSpawnPointFirstTime += DelayedConfigRelease;

        SpawnerConfigurationManager.OnLateConfigure += ApplyFileConfigs;

        SpawnerConfigurationManager.OnConfigureFinished += WriteConfigsToDisk;

        LifecycleManager.OnFindSpawnPointFirstTime += SpawnAreaDataGatherer.ScanAndPrint;

        SpawnAreaSpawnerSyncSetup.Configure();
    }

    internal static void LoadFileConfigs()
    {
        SpawnAreaSpawnerTomlCfgManager.Load();
    }

    internal static void ApplyFileConfigs(ISpawnerConfigurationCollection spawnerConfigs)
    {
        SpawnAreaSpawnerConfigApplier.ApplyBepInExConfigs(spawnerConfigs);
    }

    internal static void WriteConfigsToDisk()
    {
        if (ConfigurationManager.GeneralConfig?.WriteSpawnAreaSpawnersToFile.Value == true)
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

        SpawnAreaSpawnerManager.DelaySpawners = false;
    }
}