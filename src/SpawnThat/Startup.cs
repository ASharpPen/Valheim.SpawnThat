using System.Threading.Tasks;
using SpawnThat.Configuration;
using SpawnThat.ConsoleCommands;
using SpawnThat.Debugging;
using SpawnThat.Debugging.Datamining;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners;
using SpawnThat.Spawners.LocalSpawner.Startup;
using SpawnThat.Spawners.SpawnAreaSpawner.Startup;
using SpawnThat.Spawners.WorldSpawner.Debug;
using SpawnThat.Spawners.WorldSpawner.Managers;
using SpawnThat.Spawners.WorldSpawner.Startup;
using SpawnThat.World.Locations;
using SpawnThat.World.Maps.Sectors;
using SpawnThat.World.Maps.Zones;

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
        SetupMaps();
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

    private static void SetupMaps()
    {
        LifecycleManager.OnBiomesLoaded += () =>
        {
            AltBiomesFileGenerator.WriteToDiskAsToml();

            SectorManager.AssignIds();
            SectorManager.InitMap();
            ZoneMapManager.InitializeSectorMap(SectorManager.IdMap);

            SectorManager.PrintMaps();
            ZoneMapManager.PrintMaps();

            WorldSpawnerSpawnMapImageGenerator.OnBiomesLoaded();
        };

        WorldSpawnerManager.OnConfigsApplied += WorldSpawnerSpawnMapImageGenerator.OnConfigsReady;
    }
}
