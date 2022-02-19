using System.Collections;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners.DestructibleSpawner.Managers;
using SpawnThat.Spawners.DestructibleSpawner.Sync;
using SpawnThat.Spawners.WorldSpawner.Managers;
using UnityEngine;

namespace SpawnThat.Spawners.DestructibleSpawner.Startup;

internal static class DestructibleSpawnerSetup
{
    public static void SetupDestructibleSpawners()
    {
        LifecycleManager.OnFindSpawnPointFirstTime += DelayedConfigRelease;

        DestructibleSpawnerSyncSetup.Configure();
    }

    internal static void LoadFileConfigs()
    {

    }

    internal static void ApplyFileConfigs(ISpawnerConfigurationCollection spawnerConfigs)
    {
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