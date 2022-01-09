using Valheim.SpawnThat.Configuration;

namespace Valheim.SpawnThat.Startup;

internal static class ServiceRegistrationConfigurationExtensions
{
    public static void SetupMainConfiguration(this ServiceRegistrationManager _)
    {
        LifecycleManager.OnWorldInit += LoadBepInExConfigs;
    }

    private static void LoadBepInExConfigs()
    {
        ConfigurationManager.LoadAllConfigurations();
    }
}
