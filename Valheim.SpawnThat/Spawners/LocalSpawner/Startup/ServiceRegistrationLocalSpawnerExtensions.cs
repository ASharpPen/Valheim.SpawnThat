using Valheim.SpawnThat.Spawners;
using Valheim.SpawnThat.Spawners.LocalSpawner;
using Valheim.SpawnThat.Spawners.LocalSpawner.Configuration.BepInEx;

namespace Valheim.SpawnThat.Startup;

internal static class ServiceRegistrationLocalSpawnerExtensions
{
    public static void SetupLocalSpawners(this ServiceRegistrationManager _)
    {
        LifecycleManager.OnSinglePlayerInit += LoadBepInExConfigs;
        LifecycleManager.OnDedicatedServerInit += LoadBepInExConfigs;

        // TODO: This SHOULD be on late configure, but configs doesn't have a concept of null yet, so to avoid overriding everything always, they will be applied first.
        SpawnerConfigurationManager.OnEarlyConfigure += ApplyBepInExConfigs;
    }

    private static void LoadBepInExConfigs()
    {
        CreatureSpawnerConfigurationManager.LoadAllConfigurations();
        LocalSpawnerManager.WaitingForConfigs = false;
    }

    private static void ApplyBepInExConfigs(ISpawnerConfigurationCollection spawnerConfigs)
    {
        CreatureSpawnerConfigApplier.ApplyBepInExConfigs(spawnerConfigs);
    }
}
