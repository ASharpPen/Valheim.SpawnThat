using SpawnThat.Configuration;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners;
using SpawnThat.Spawners.SpawnAreaSpawner.Startup;
using SpawnThat.Spawners.LocalSpawner.Startup;
using SpawnThat.Spawners.WorldSpawner.Startup;
using SpawnThat.World.Locations;

namespace SpawnThat;

internal static class Startup
{
    public static void SetupServices()
    {
        GeneralConfigurationSetup.SetupMainConfiguration();
        LocalSpawnerSetup.SetupLocalSpawners();
        WorldSpawnerSetup.SetupWorldSpawners();
        SpawnAreaSpawnerSetup.SetupSpawnAreaSpawners();

        LifecycleManager.OnLateInit += InitConfiguration;

        ZoneSystemSyncSetup.Configure();
    }

    private static void InitConfiguration()
    {
        if (LifecycleManager.GameState == GameState.Singleplayer ||
            LifecycleManager.GameState == GameState.DedicatedServer)
        {
            SpawnerConfigurationManager.BuildConfigurations();
        }
    }
}
