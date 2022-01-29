using System;
using System.Collections.Generic;
using SpawnThat.Core;
using SpawnThat.Spawners.WorldSpawner.Managers;

namespace SpawnThat.Spawners.WorldSpawner.Configurations;

internal class WorldSpawnerConfiguration : ISpawnerConfiguration
{
    private Dictionary<uint, WorldSpawnBuilder> builders = new();

    private bool finalized = false;

    public IWorldSpawnBuilder GetBuilder(uint id)
    {
        if (finalized)
        {
            throw new InvalidOperationException("Collection is finalized. Builders cannot be retrieved or modified after build.");
        }

        if (builders.TryGetValue(id, out WorldSpawnBuilder existing))
        {
            return existing;
        }

        return builders[id] = new WorldSpawnBuilder((int)id);
    }

    public void Build()
    {
        if (finalized)
        {
            Log.LogWarning("Attempting to build world spawner configs that have already been finalized. Ignoring request.");
            return;
        }

        finalized = true;

        foreach (var builder in builders)
        {
            var template = builder.Value.Build();
            WorldSpawnTemplateManager.SetTemplate((int)builder.Key, template);
        }
    } 
}
