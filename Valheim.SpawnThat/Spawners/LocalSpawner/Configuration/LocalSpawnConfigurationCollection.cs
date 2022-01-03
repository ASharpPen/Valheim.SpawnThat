using System.Collections.Generic;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.LocalSpawner.Configuration;

public record struct SpawnerNameIdentifier(string SpawnerPrefabName);

public record struct LocationIdentifier(string Location, string PrefabName);

public record struct RoomIdentifier(string Room, string PrefabName);

internal class LocalSpawnConfigurationCollection : ISpawnerConfiguration
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
            var template = spawnerBuilder.Value.Build();
            LocalSpawnTemplateManager.SetTemplate(spawnerBuilder.Key, template);
        }

        foreach (var spawnerBuilder in locationBuilders)
        {
            var template = spawnerBuilder.Value.Build();
            LocalSpawnTemplateManager.SetTemplate(spawnerBuilder.Key, template);
        }

        foreach (var spawnerBuilder in roomBuilders)
        {
            var template = spawnerBuilder.Value.Build();
            LocalSpawnTemplateManager.SetTemplate(spawnerBuilder.Key, template);
        }

        LocalSpawnerManager.WaitingForConfigs = false;
    }

    public ILocalSpawnBuilder GetBuilder(SpawnerNameIdentifier identifier)
    {
        if (spawnerBuilders.TryGetValue(identifier, out var existing))
        {
            return existing;
        }

        return spawnerBuilders[identifier] = new LocalSpawnBuilder();
    }

    public ILocalSpawnBuilder GetBuilder(LocationIdentifier identifier)
    {
        if (locationBuilders.TryGetValue(identifier, out var existing))
        {
            return existing;
        }

        return locationBuilders[identifier] = new LocalSpawnBuilder();
    }

    public ILocalSpawnBuilder GetBuilder(RoomIdentifier identifier)
    {
        if (roomBuilders.TryGetValue(identifier, out var existing))
        {
            return existing;
        }

        return roomBuilders[identifier] = new LocalSpawnBuilder();
    }
}
