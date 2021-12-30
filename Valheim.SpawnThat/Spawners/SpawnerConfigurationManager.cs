using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Startup;

namespace Valheim.SpawnThat.Spawners;

public static class SpawnerConfigurationManager
{
    private static List<Action<ISpawnerConfigurationCollection>> _configurations = new();

    static SpawnerConfigurationManager()
    {
        StateResetter.Subscribe(() =>
        {
            _configurations = new();
        });
    }

    public delegate void SpawnerConfigurationEvent();

    public static event SpawnerConfigurationEvent OnConfigure;

    internal static void ApplyConfigurations(ISpawnerConfigurationCollection configuration)
    {
        OnConfigure();

        foreach (var configure in _configurations)
        {
            try 
            {
                if (configure is not null)
                {
                    configure(configuration);
                }
            }
            catch(Exception e)
            {
                Log.LogError("Error while attempting to apply spawner configuration", e);
            }
        }
    }

    public static void SubscribeConfiguration(Action<ISpawnerConfigurationCollection> configure)
    {
        _configurations.Add(configure);
    }
}
