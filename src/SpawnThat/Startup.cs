using SpawnThat.Configuration;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners;
using SpawnThat.Spawners.DestructibleSpawner.Startup;
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
        DestructibleSpawnerSetup.SetupDestructibleSpawners();

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
