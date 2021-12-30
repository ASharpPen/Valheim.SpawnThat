using System;
using System.Collections.Generic;

namespace Valheim.SpawnThat.Spawners.WorldSpawner.Configurations;

internal class WorldSpawnConfigurationCollection : ISpawnerConfiguration
{
    private Dictionary<uint, WorldSpawnBuilder> builders = new();

    public void Build()
    {
        throw new NotImplementedException();
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
