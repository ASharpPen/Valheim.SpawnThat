using System;
using System.Collections.Generic;
using SpawnThat.Core;
using SpawnThat.Lifecycle;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Spawners;

public static class SpawnerConfigurationManager
{
    private static List<Action<ISpawnerConfigurationCollection>> _configurations = new();
    private static List<Action<ISpawnerConfigurationCollection>> _lateConfigurations = new();

    public static ISpawnerConfigurationCollection ConfigurationCollection { get; private set; }

    static SpawnerConfigurationManager()
    {
        // Reset on new world entered.
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            ConfigurationCollection = null;
        });
    }

    internal delegate void SpawnerEarlyConfigurationEvent(ISpawnerConfigurationCollection spawnerConfigs);
    internal static event SpawnerEarlyConfigurationEvent OnEarlyConfigure;

    public delegate void SpawnerConfigurationEvent(ISpawnerConfigurationCollection spawnerConfigs);
    public static event SpawnerConfigurationEvent OnConfigure;

    internal delegate void SpawnerLateConfigurationEvent(ISpawnerConfigurationCollection spawnerConfigs);
    internal static event SpawnerLateConfigurationEvent OnLateConfigure;

    /// <summary>
    /// Subscribe configuration action.
    /// </summary>
    /// <remarks>Subscriptions are never reset. Use OnConfigure to manage subscription yourself.</remarks>
    public static void SubscribeConfiguration(Action<ISpawnerConfigurationCollection> configure)
    {
        _configurations.Add(configure);
    }

    internal static void SubscribeLateConfiguration(Action<ISpawnerConfigurationCollection> configure)
    {
        _lateConfigurations.Add(configure);
    }

    internal static void BuildConfigurations()
    {
        Log.LogDebug("Building configurations.");

        SpawnerConfigurationCollection configuration = new();

        OnEarlyConfigure.RaiseSafely(
            "Error during early configuration",
            configuration);

        OnConfigure.RaiseSafely(
            "Error during configure event",
            configuration);

        foreach (var configure in _configurations)
        {
            try
            {
                if (configure is not null)
                {
                    configure(configuration);
                }
            }
            catch (Exception e)
            {
                Log.LogError("Error while attempting to apply spawner configuration", e);
            }
        }

        OnLateConfigure.RaiseSafely(
            "Error during late configure event",
            configuration);

        foreach (var configure in _lateConfigurations)
        {
            try
            {
                if (configure is not null)
                {
                    configure(configuration);
                }
            }
            catch (Exception e)
            {
                Log.LogError("Error while attempting to apply spawner configuration", e);
            }
        }

        foreach (var spawnerConfig in configuration.SpawnerConfigurations)
        {
            try 
            {
                spawnerConfig.Build();
            }
            catch (Exception e)
            {
                Log.LogError($"Error during build of spawner config {spawnerConfig?.GetType()?.Name}", e);
            }
        }
        
        ConfigurationCollection = configuration;
    }
}
