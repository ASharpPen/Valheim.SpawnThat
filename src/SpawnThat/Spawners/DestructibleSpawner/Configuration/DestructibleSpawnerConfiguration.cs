using System;
using System.Collections.Generic;
using SpawnThat.Core;
using SpawnThat.Spawners.DestructibleSpawner.Managers;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration;

internal class DestructibleSpawnerConfiguration : ISpawnerConfiguration
{
    private List<DestructibleSpawnerBuilder> Builders { get; } = new();

    private bool finalized = false;

    public void Build()
    {
        if (finalized)
        {
            Log.LogWarning("Attempting to build world spawner configs that have already been finalized. Ignoring request.");
            return;
        }

        finalized = true;

        foreach (var builder in Builders)
        {
            DestructibleSpawnerManager.AddTemplate(builder.Build());
        }
    }

    public IDestructibleSpawnerBuilder CreateBuilder()
    {
        if (finalized)
        {
            throw new InvalidOperationException("Collection is finalized. Builders cannot be retrieved or modified after build.");
        }

        DestructibleSpawnerBuilder builder = new();

        Builders.Add(builder);

        return builder;
    }
}
