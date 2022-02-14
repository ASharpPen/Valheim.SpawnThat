using System;
using System.Collections.Generic;
using SpawnThat.Spawners.DestructibleSpawner.Identifiers;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration;

internal class DestructibleSpawnerBuilder : IDestructibleSpawnerBuilder
{
    private Dictionary<uint, IDestructibleSpawnBuilder> Spawns { get; } = new();

    private Dictionary<Type, ISpawnerIdentifier> Identifiers { get; } = new();

    public IDestructibleSpawnBuilder GetSpawnBuilder(uint id)
    {
        if (Spawns.TryGetValue(id, out var cached))
        {
            return cached;
        }

        return Spawns[id] = new DestructibleSpawnBuilder(id);
    }

    public IDestructibleSpawnerBuilder SetIdentifier<T>(T identifier)
         where T : class, ISpawnerIdentifier
    {
        Identifiers[identifier.GetType()] = identifier;
        return this;
    }
}
