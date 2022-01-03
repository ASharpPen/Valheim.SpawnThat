using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.WorldSpawner.Configurations;

internal class WorldSpawnConfigurationCollection : ISpawnerConfiguration
{
    private Dictionary<uint, WorldSpawnBuilder> builders = new();

    private bool finalized = false;

    public void Build()
    {
        if (finalized)
        {
            Log.LogWarning("Attempting to finalize world spawner configs that have already been finalized. Ignoring.");
            return;
        }

        finalized = true;

        foreach(var builder in builders)
        {
            var template = builder.Value.Build();
            WorldSpawnTemplateManager.SetTemplate((int)builder.Key, template);
        }
    }

    public IWorldSpawnBuilder GetBuilder(uint id)
    {
        if (builders.TryGetValue(id, out WorldSpawnBuilder existing))
        {
            return existing;
        }

        return builders[id] = new WorldSpawnBuilder();
    }
}
