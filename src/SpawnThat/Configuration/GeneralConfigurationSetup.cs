using SpawnThat.Configuration.Sync;
using SpawnThat.Lifecycle;

namespace SpawnThat.Configuration;

internal static class GeneralConfigurationSetup
{
    public static void SetupMainConfiguration()
    {
        LifecycleManager.OnWorldInit += LoadBepInExConfigs;

        GeneralConfigSyncSetup.Configure();
    }

    private static void LoadBepInExConfigs()
    {
        ConfigurationManager.LoadAllConfigurations();
    }
}
