using System;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Cache;
using Valheim.SpawnThat.Spawners.LocalSpawner.Caches;
using Valheim.SpawnThat.Spawners.LocalSpawner.Services;
using Valheim.SpawnThat.Startup;

namespace Valheim.SpawnThat.Spawners.LocalSpawner;

internal static class LocalSpawnerManager
{
    private static ManagedCache<LocalSpawnTemplate> templateBySpawner = new();

    /// <summary>
    /// Disables local spawner updates while true.
    /// Indended to delay updates until configs have been loaded.
    /// </summary>
    public static bool WaitingForConfigs { get; set; } = true;

    static LocalSpawnerManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            WaitingForConfigs = true;
        });
    }

    public static void EnsureSpawnerConfigured(CreatureSpawner spawner)
    {
        // Check if already configured.
        if (spawner.IsInitialized())
        {
            return;
        }

        try
        {
            // Find or register template for spawner.
            var template = GetTemplate(spawner);

            // Try apply template to spawner.
            LocalSpawnerConfigurationService.ConfigureSpawner(spawner, template);
            spawner.SetSuccessfulInit();
        }
        catch(Exception e)
        {
            Log.LogError($"Error while attempting to apply template to local spawner '{spawner.name}'.", e);
            spawner.SetFailedInitialization();
        }

        if (spawner.IsFailedInitialization() && spawner.GetFailedInitCount() >= 2)
        {
            Log.LogTrace($"Too many failed initialization attempts for spawner '{spawner.name}', will stop retrying.");
            spawner.SetInitialized(true);
            spawner.SetShouldWait(false);
            return;
        }
    }

    public static LocalSpawnTemplate GetTemplate(CreatureSpawner spawner)
    {
        if (templateBySpawner.TryGet(spawner, out var cached))
        {
            return cached;
        }

        var template = TemplateMatcherService.MatchMostSpecificTemplate(spawner);

        templateBySpawner.Set(spawner, template);

        return template;
    }

    public static bool ShouldDelaySpawnerUpdate(CreatureSpawner spawner)
    {
        if (WaitingForConfigs)
        {
            Log.LogTrace("CreatureSpawner waiting for configs. Skipping update.");
            return true;
        }

        if (spawner.ShouldWait())
        {
            Log.LogTrace("CreatureSpawner is disabled. Skipping update.");
            return true;
        }

        return false;
    }
}
