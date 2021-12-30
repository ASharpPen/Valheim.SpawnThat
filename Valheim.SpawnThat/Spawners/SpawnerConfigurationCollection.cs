using System;
using System.Collections.Generic;
using System.Linq;

namespace Valheim.SpawnThat.Spawners;

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
            spawnerCollections[spawnerType] = spawnerConfig;
            return spawnerConfig;
        }
    }
}
