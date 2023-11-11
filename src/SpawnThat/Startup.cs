using SpawnThat.Configuration;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners;
using SpawnThat.Spawners.SpawnAreaSpawner.Startup;
using SpawnThat.Spawners.LocalSpawner.Startup;
using SpawnThat.Spawners.WorldSpawner.Startup;
using SpawnThat.World.Locations;
using SpawnThat.ConsoleCommands;

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

        RegisterCommands();
    }

    private static void InitConfiguration()
    {
        if (LifecycleManager.GameState == GameState.Singleplayer ||
            LifecycleManager.GameState == GameState.DedicatedServer)
        {
            SpawnerConfigurationManager.BuildConfigurations();
        }
    }

    private static void RegisterCommands()
    {
        AreaCommand.Register();
        AreaRollCommand.Register();
        AreaRollHeatmapCommand.Register();
        RoomCommand.Register();
        WhereDoesItSpawnCommand.Register();
    }
}
