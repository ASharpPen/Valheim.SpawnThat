using System;
using System.Collections.Generic;
using SpawnThat.Core;
using SpawnThat.Spawners.LocalSpawner.Managers;
using SpawnThat.Spawners.LocalSpawner.Models;

namespace SpawnThat.Spawners.LocalSpawner.Configuration;

internal class LocalSpawnerConfiguration : ISpawnerConfiguration
{
    private Dictionary<SpawnerNameIdentifier, LocalSpawnBuilder> spawnerBuilders { get; set; } = new();
    private Dictionary<LocationIdentifier, LocalSpawnBuilder> locationBuilders { get; set; } = new();
    private Dictionary<RoomIdentifier, LocalSpawnBuilder> roomBuilders { get; set; } = new();

    private bool finalized = false;

    public void Build()
    {
        if (finalized)
        {
            Log.LogWarning("Attempting to finalize local spawner configs that have already been finalized. Ignoring.");
            return;
        }

        finalized = true;

        foreach (var spawnerBuilder in spawnerBuilders)
        {
            try
            {
                var template = spawnerBuilder.Value.Build();
                LocalSpawnTemplateManager.SetTemplate(spawnerBuilder.Key, template);
            }
            catch (Exception e)
            {
                Log.LogWarning($"Error while attempting to build configuration for named local spawner '{spawnerBuilder.Key.SpawnerPrefabName}'.", e);
            }
        }

        foreach (var spawnerBuilder in locationBuilders)
        {
            try
            {
                var template = spawnerBuilder.Value.Build();
                LocalSpawnTemplateManager.SetTemplate(spawnerBuilder.Key, template);
            }
            catch (Exception e)
            {
                Log.LogWarning($"Error while attempting to build configuration for location local spawner '{spawnerBuilder.Key.Location}.{spawnerBuilder.Key.PrefabName}'.", e);
            }
        }

        foreach (var spawnerBuilder in roomBuilders)
        {
            try
            {
                var template = spawnerBuilder.Value.Build();
                LocalSpawnTemplateManager.SetTemplate(spawnerBuilder.Key, template);
            }
            catch (Exception e)
            {
                Log.LogWarning($"Error while attempting to build configuration for room local spawner '{spawnerBuilder.Key.Room}.{spawnerBuilder.Key.PrefabName}'.", e);
            }
        }

        LocalSpawnerManager.WaitingForConfigs = false;
    }

    public ILocalSpawnBuilder GetBuilder(SpawnerNameIdentifier identifier)
    {
        if (spawnerBuilders.TryGetValue(identifier, out var existing))
        {
#if DEBUG
            Log.LogWarning($"Potentially conflicting configurations for local spawner with name '{identifier.SpawnerPrefabName}' detected.");
#endif

            return existing;
        }

        return spawnerBuilders[identifier] = new LocalSpawnBuilder();
    }

    public ILocalSpawnBuilder GetBuilder(LocationIdentifier identifier)
    {
        if (locationBuilders.TryGetValue(identifier, out var existing))
        {
#if DEBUG
            Log.LogWarning($"Potentially conflicting configurations for local spawner '{identifier.Location}.{identifier.PrefabName}' detected.");
#endif

            return existing;
        }

        return locationBuilders[identifier] = new LocalSpawnBuilder();
    }

    public ILocalSpawnBuilder GetBuilder(RoomIdentifier identifier)
    {
        if (roomBuilders.TryGetValue(identifier, out var existing))
        {
#if DEBUG
            Log.LogWarning($"Potentially conflicting configurations for local spawner '{identifier.Room}.{identifier.PrefabName}' detected.");
#endif

            return existing;
        }

        return roomBuilders[identifier] = new LocalSpawnBuilder();
    }
}
