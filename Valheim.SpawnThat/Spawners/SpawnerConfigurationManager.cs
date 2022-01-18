﻿using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Startup;

namespace Valheim.SpawnThat.Spawners;

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
            _configurations = new();
            _lateConfigurations = new();
            ConfigurationCollection = null;
        });
    }

    internal delegate void SpawnerEarlyConfigurationEvent(ISpawnerConfigurationCollection spawnerConfigs);
    internal static event SpawnerEarlyConfigurationEvent OnEarlyConfigure;

    public delegate void SpawnerConfigurationEvent(ISpawnerConfigurationCollection spawnerConfigs);
    public static event SpawnerConfigurationEvent OnConfigure;

    internal delegate void SpawnerLateConfigurationEvent(ISpawnerConfigurationCollection spawnerConfigs);
    internal static event SpawnerLateConfigurationEvent OnLateConfigure;

    // TODO: Consider removing this if it gets reset anyway?
    // TODO: Consider not resetting this, so that it can be configured once instead, without having to worry about it. OnConfigure should be easier to manage with regards to subscribing / unsubscribing..
    /// <summary>
    /// Subscribe configuration action.
    /// </summary>
    /// <remarks>Subscriptions are reset by OnWorldInit. Use OnConfigure to manage subscription yourself.</remarks>
    public static void SubscribeConfiguration(Action<ISpawnerConfigurationCollection> configure)
    {
        _configurations.Add(configure);
    }

    // TODO: Consider removing this if it gets reset anyway?
    // TODO: Consider not resetting this, so that it can be configured once instead, without having to worry about it. OnConfigure should be easier to manage with regards to subscribing / unsubscribing..
    internal static void SubscribeLateConfiguration(Action<ISpawnerConfigurationCollection> configure)
    {
        _lateConfigurations.Add(configure);
    }

    internal static void BuildConfigurations()
    {
        Log.LogDebug("Building configurations.");

        SpawnerConfigurationCollection configuration = new();

        if (OnEarlyConfigure is not null)
        {
            foreach (var configure in OnEarlyConfigure.GetInvocationList())
            {
                try
                {
                    configure.DynamicInvoke(configuration);
                }
                catch (Exception e)
                {
                    Log.LogError("Error during early configuration.", e);
                }
            }
        }

        if (OnConfigure is not null)
        {
            try
            {
                foreach (var configure in OnConfigure.GetInvocationList())
                {
                    configure.DynamicInvoke(configuration);
                }
            }
            catch (Exception e)
            {
                Log.LogError("Error during configure event", e);
            }
        }

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

        if (OnLateConfigure is not null)
        {
            try
            {
                foreach (var lateConfigure in OnLateConfigure.GetInvocationList())
                {
                    lateConfigure.DynamicInvoke(configuration);
                }
            }
            catch (Exception e)
            {
                Log.LogError("Error during late configure event", e);
            }
        }

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
