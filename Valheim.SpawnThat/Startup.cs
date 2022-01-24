using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Lifecycle;
using Valheim.SpawnThat.Spawners;
using Valheim.SpawnThat.Spawners.LocalSpawner.Startup;
using Valheim.SpawnThat.Spawners.WorldSpawner.Startup;
using Valheim.SpawnThat.World.Locations;

namespace Valheim.SpawnThat;

internal static class Startup
{
    public static void SetupServices()
    {
        GeneralConfigurationSetup.SetupMainConfiguration();
        LocalSpawnerSetup.SetupLocalSpawners();
        WorldSpawnerSetup.SetupWorldSpawners();

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
