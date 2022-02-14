using System;
using System.Collections.Generic;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration;

internal class DestructibleSpawnerConfiguration : ISpawnerConfiguration
{
    private List<DestructibleSpawnerBuilder> Builders { get; } = new();

    public void Build()
    {
        throw new NotImplementedException();
    }

    public IDestructibleSpawnerBuilder CreateBuilder()
    {
        DestructibleSpawnerBuilder builder = new();

        Builders.Add(new DestructibleSpawnerBuilder());

        return builder;
    }
}
