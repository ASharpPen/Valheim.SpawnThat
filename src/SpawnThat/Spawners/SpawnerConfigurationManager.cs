using System;
using System.Collections.Generic;
using SpawnThat.Core;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Spawners;

/// <summary>
/// Handles configuration creation workflow.
/// 
/// When <c>Lifecycle.OnLateInit</c> and GameState is SinglePlayer or Dedicated,
/// the manager initalizes a new <c>ISpawnerConfigurationCollection</c>, and runs
/// subscribed configuration actions in the order:
/// 1. Actions for OnConfigure
/// 2. Actions for SubscribeConfiguration
/// 3. Configurations loaded from files
/// 
/// The collection is then finalized, and results are stored.
/// If in Dedicated, and a player joins, the finalized templates are synced to player,
/// to later be applied to the various spawners.
/// 
/// The way the configurations are applied to spawners depends on the spawner type.
/// </summary>
public static class SpawnerConfigurationManager
{
    private static List<Action<ISpawnerConfigurationCollection>> _configurations = new();
    private static List<Action<ISpawnerConfigurationCollection>> _lateConfigurations = new();

    internal static event Action<ISpawnerConfigurationCollection> OnEarlyConfigure;

    /// <summary>
    /// OnConfigure is called once on <c>Lifecycle.OnLateInit</c> if GameState is SinglePlayer or Dedicated.
    /// </summary>
    public static event Action<ISpawnerConfigurationCollection> OnConfigure;

    internal static event Action<ISpawnerConfigurationCollection> OnLateConfigure;

    /// <summary>
    /// Subscribe configuration action.
    /// 
    /// Actions are run after <c>OnConfigure</c>.
    /// </summary>
    /// <remarks>Subscribed actions are never reset. Use OnConfigure to manage subscription yourself.</remarks>
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
    }
}
