using Valheim.SpawnThat.Spawners;
using Valheim.SpawnThat.Spawners.WorldSpawner;
using Valheim.SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;

namespace Valheim.SpawnThat.Startup;

internal static class ServiceRegistrationWorldSpawnerExtensions
{
    public static void SetupWorldSpawners(this ServiceRegistrationManager _)
    {
        LifecycleManager.OnSinglePlayerInit += LoadBepInExConfigs;
        LifecycleManager.OnDedicatedServerInit += LoadBepInExConfigs;

        // TODO: This SHOULD be on late configure, but configs doesn't have a concept of null yet, so to avoid overriding everything always, they will be applied first.
        SpawnerConfigurationManager.OnEarlyConfigure += ApplyBepInExConfigs;

        //SpawnerConfigurationManager.SubscribeConfiguration(ApplyBepInExConfigs);
    }

    internal static void LoadBepInExConfigs()
    {
        SpawnSystemConfigurationManager.LoadAllConfigurations();
        WorldSpawnerManager.WaitingForConfigs = false;
    }

    internal static void ApplyBepInExConfigs(ISpawnerConfigurationCollection spawnerConfigs)
    {
        SpawnSystemConfigApplier.ApplyBepInExConfigs(spawnerConfigs);
    }
}
