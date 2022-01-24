using Valheim.SpawnThat.Configuration.Sync;
using Valheim.SpawnThat.Lifecycle;

namespace Valheim.SpawnThat.Configuration;

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
