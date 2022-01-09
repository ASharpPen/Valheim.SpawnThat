using Valheim.SpawnThat.Spawners;

namespace Valheim.SpawnThat.Startup;

/// <summary>
/// Manager for Spawn That services to get properly initialized on plugin startup.
/// 
/// Set up as singleton, to make it simple to extend.
/// </summary>
internal class ServiceRegistrationManager
{
    private static ServiceRegistrationManager _instance;
    private static ServiceRegistrationManager Instance => _instance ??= new();

    public static void SetupServices()
    {
        Instance.SetupMainConfiguration();
        Instance.SetupLocalSpawners();
        Instance.SetupWorldSpawners();

        LifecycleManager.AfterInit += InitConfiguration;
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
