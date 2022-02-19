using System;
using System.Collections.Generic;
using System.Linq;
using SpawnThat.Caches;
using SpawnThat.Core;
using SpawnThat.Core.Cache;
using SpawnThat.Spawners.DestructibleSpawner.Models;
using SpawnThat.Utilities.Extensions;
using UnityEngine;
using static SpawnArea;

namespace SpawnThat.Spawners.DestructibleSpawner.Managers;

internal class DestructibleSpawnSessionManager
{
    private static ManagedCache<SpawnSession> SpawnSessions { get; } = new();

    public static void StartSession(SpawnArea spawner)
    {
        var context = new SpawnSession(spawner.m_nview.GetZDO())
        {
            OriginalSpawnData = spawner.m_prefabs
        };

        SpawnSessions.Set(spawner, context);
    }

    public static void EndSession(SpawnArea spawner)
    {
        if (SpawnSessions.TryGet(spawner, out var context))
        {
            // Reset spawn data if they have been filtered during session.
            if (spawner.m_prefabs != context.OriginalSpawnData)
            {
                spawner.m_prefabs = context.OriginalSpawnData;
            }
        }
    }

    public static void SetCurrentSpawn(SpawnArea spawner, SpawnData spawn)
    {
        if (SpawnSessions.TryGet(spawner, out var context))
        {
            context.CurrentSpawn = spawn;

            if (DestructibleSpawnTemplateManager.TryGetTemplate(spawn, out var spawnTemplate))
            {
                context.CurrentTemplate = spawnTemplate;
            }
        }
    }

    public static void FilterSpawnData(SpawnArea spawner)
    {
        var template = DestructibleSpawnerManager.GetTemplate(spawner);

        if (template is null)
        {
            return;
        }

        SpawnSession context;
        if (!SpawnSessions.TryGet(spawner, out context))
        {
            return;
        }

        List<SpawnData> spawnData = new(spawner.m_prefabs.Count);

        foreach (var spawn in spawner.m_prefabs)
        {
            if (DestructibleSpawnTemplateManager.TryGetTemplate(spawn, out var spawnTemplate))
            {
                if (!spawnTemplate.Enabled)
                {
                    continue;
                }

                bool isValid = spawnTemplate.Conditions.All(condition =>
                {
                    try
                    {
                        return condition.IsValid(context);
                    }
                    catch (Exception e)
                    {
                        Log.LogWarning($"Error while evaluating condition '{condition.GetType().Name}' for spawn '{spawnTemplate.Id}' of destructible spawner '{spawner.GetCleanedName()}'. Ignoring condition.", e);
                        return true;
                    }
                });

                if (isValid)
                {
                    spawnData.Add(spawn);
                }
            }
            else
            {
                spawnData.Add(spawn);
            }
        }

        spawner.m_prefabs = spawnData;
    }

    public static bool CheckValidPosition(SpawnArea spawner, Vector3 position)
    {
        try
        {
            SpawnSession context;
            if (!SpawnSessions.TryGet(spawner, out context) || context is null)
            {
                return true;
            }

            if (context.CurrentTemplate is null)
            {
                return true;
            }

            return context.CurrentTemplate.PositionConditions.All(condition =>
            {
                try
                {
                    return condition.IsValid(context, position);
                }
                catch (Exception e)
                {
                    Log.LogWarning($"Error while evaluating position condition '{condition.GetType().Name}' for spawn '{context.CurrentTemplate.Id}' of destructible spawner '{spawner.GetCleanedName()}'. Ignoring condition.", e);
                    return true;
                }
            });
        }
        catch(Exception e)
        {
            Log.LogError($"Error while attempting to evaluate valid position '{position}' of spawn for destructible spawner '{spawner.GetCleanedName()}'.", e);
            return true;
        }
    }

    public static void ModifySpawn(GameObject spawn, SpawnArea spawner)
    {
        try
        {
            SpawnSession context;
            if (!SpawnSessions.TryGet(spawner, out context) || context is null)
            {
                return;
            }

            if (context.CurrentTemplate?.Modifiers is null)
            {
                return;
            }

            var zdo = ComponentCache.GetZdo(spawn);

            foreach (var modifier in context.CurrentTemplate.Modifiers)
            {
                try
                {
                    modifier?.Modify(spawn, zdo);
                }
                catch (Exception e)
                {
                    Log.LogWarning("Error while attempting to apply modifier '{}' to spawn '{}' of destructible spawner '{}'.", e);
                }
            }
        }
        catch (Exception e)
        {
            Log.LogError($"Error while modifying spawn '{spawn.GetCleanedName()}' of destructible spawner '{spawner.GetCleanedName()}'.", e);
        }
    }
}
