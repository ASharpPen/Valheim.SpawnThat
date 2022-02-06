﻿using System;
using System.Collections.Generic;
using SpawnThat.Core;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners.WorldSpawner.Services;

namespace SpawnThat.Spawners.WorldSpawner.Managers;

internal static class WorldSpawnerManager
{
    private static bool HasInstantiatedSpawnLists;
    private static List<SpawnSystemList> SpawnLists { get; set; } = new();

    private static Dictionary<SpawnSystem.SpawnData, WorldSpawnTemplate> TemplatesBySpawnEntry { get; set; } = new();

    private static List<SpawnSystemList> PrefabSpawnSystemLists { get; set; } = new(0);

    /// <summary>
    /// Disables world spawner updates while true.
    /// Indended to delay updates until configs have been loaded.
    /// </summary>
    public static bool WaitingForConfigs { get; set; } = true;

    static WorldSpawnerManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            WaitingForConfigs = true;

            HasInstantiatedSpawnLists = false;
            SpawnLists.Clear();

            TemplatesBySpawnEntry = new();
        });
    }

    /// <summary>
    /// Grab references to spawnlists before anyone starts adding to them.
    /// This is to ensure that when later instantiating the default lists,
    /// only the prefab's will be instantiated. The ones added by other mods
    /// will be untouched, and thereby let them keep their references intact.
    /// </summary>
    public static void SetPrefabSpawnSystemLists()
    {
        // Initialize only once.
        if (PrefabSpawnSystemLists.Count == 0)
        {
            var prefabSpawnSystem = ZoneSystem.instance.m_zoneCtrlPrefab.GetComponent<SpawnSystem>();

            PrefabSpawnSystemLists = new(prefabSpawnSystem.m_spawnLists);
        }
    }

    public static void EnsureInstantiatedSpawnListAssigned(SpawnSystem spawner)
    {
        if (HasInstantiatedSpawnLists)
        {
            spawner.m_spawnLists = SpawnLists;
            return;
        }

        try
        {
            foreach (var spawnList in spawner.m_spawnLists)
            {
                if (PrefabSpawnSystemLists.Contains(spawnList))
                {
                    // spawnlist is a prefab
                    Log.LogTrace($"Instantiating spawn list: '{spawnList.name}'");
                    var instantiatedSpawnList = UnityEngine.Object.Instantiate(spawnList.gameObject);
                    SpawnLists.Add(instantiatedSpawnList.GetComponent<SpawnSystemList>());
                }
                else
                {
                    // Spawnlist is custom, add it normally.
                    SpawnLists.Add(spawnList);
                }
            }
        }
        catch (Exception e)
        {
            Log.LogWarning("Unable to instantiate SpawnSystemLists. Skipping step. Avoid entering multiple worlds without restarting, then everything will still be fine", e);
            SpawnLists = spawner.m_spawnLists;
            HasInstantiatedSpawnLists = true;
            return;
        }

        spawner.m_spawnLists = SpawnLists;
        HasInstantiatedSpawnLists = true;
    }

    public static bool ShouldDelaySpawnerUpdate(SpawnSystem spawner)
    {
        if (WaitingForConfigs)
        {
            Log.LogTrace("SpawnSysten waiting for configs. Skipping update.");
            return true;
        }

        return false;
    }

    public static void ConfigureSpawnList(SpawnSystem spawner)
    {
        if (WaitingForConfigs)
        {
            return;
        }

        WorldSpawnerConfigurationService.ConfigureSpawnLists(spawner.m_spawnLists);
    }

    public static void SetTemplate(SpawnSystem.SpawnData spawnEntry, WorldSpawnTemplate template)
    {
        TemplatesBySpawnEntry[spawnEntry] = template;
    }

    public static WorldSpawnTemplate GetTemplate(SpawnSystem.SpawnData spawnEntry)
    {
        if (TemplatesBySpawnEntry.TryGetValue(spawnEntry, out WorldSpawnTemplate template))
        {
            return template;
        }

        return null;
    }
}
