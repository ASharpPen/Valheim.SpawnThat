using System;
using System.Collections.Generic;
using System.Linq;
using SpawnThat.Core;

namespace SpawnThat.Spawners;

internal class SpawnerConfigurationCollection : ISpawnerConfigurationCollection
{
    private Dictionary<Type, ISpawnerConfiguration> spawnerCollections = new();

    public List<ISpawnerConfiguration> SpawnerConfigurations
    {
        get
        {
            return spawnerCollections.Values.ToList();
        }
    }

    public TSpawnerConfig GetOrAddSpawnerConfiguration<TSpawnerConfig>(TSpawnerConfig spawnerConfig)
        where TSpawnerConfig : class, ISpawnerConfiguration
    {
        var spawnerType = typeof(TSpawnerConfig);

        if (spawnerCollections.TryGetValue(spawnerType, out var existing))
        {
            return (TSpawnerConfig)existing;
        }
        else
        {
#if DEBUG
            Log.LogTrace($"Adding new spawner type {spawnerType.Name} to collection.");
#endif            
            spawnerCollections[spawnerType] = spawnerConfig;
            return spawnerConfig;
        }
    }
}
