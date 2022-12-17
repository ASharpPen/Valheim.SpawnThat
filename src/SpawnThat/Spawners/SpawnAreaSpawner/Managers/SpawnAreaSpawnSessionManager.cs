using System;
using System.Collections.Generic;
using System.Linq;
using SpawnThat.Caches;
using SpawnThat.Core;
using SpawnThat.Core.Cache;
using SpawnThat.Spawners.SpawnAreaSpawner.Models;
using SpawnThat.Utilities.Extensions;
using UnityEngine;
using static SpawnArea;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Managers;

internal class SpawnAreaSpawnSessionManager
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
        if (spawn is null)
        {
            return;
        }

        if (SpawnSessions.TryGet(spawner, out var context))
        {
            context.CurrentSpawn = spawn;

            if (SpawnAreaSpawnTemplateManager.TryGetTemplate(spawn, out var spawnTemplate))
            {
                context.CurrentTemplate = spawnTemplate;
            }
        }
    }

    public static void FilterSpawnData(SpawnArea spawner)
    {
        var template = SpawnAreaSpawnerManager.GetTemplate(spawner);

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

        for (int i = 0; i < spawner.m_prefabs.Count; ++i)
        {
            var spawn = spawner.m_prefabs[i];

            if (SpawnAreaSpawnTemplateManager.TryGetTemplate(spawn, out var spawnTemplate))
            {
                if (!spawnTemplate.Enabled)
                {
                    continue;
                }

                bool isValid = spawnTemplate.Conditions.All(condition =>
                {
                    try
                    {
#if DEBUG
                        var isValid = condition.IsValid(context);
                        if (!isValid)
                        {
                            Log.LogTrace($"[SpawnArea Spawner] Invalid condition '{condition.GetType().Name}' for spawn '{i}'.");
                        }
                        return isValid;
#else
                        return condition.IsValid(context);
#endif
                    }
                    catch (Exception e)
                    {
                        Log.LogWarning($"Error while evaluating condition '{condition.GetType().Name}' for spawn '{spawnTemplate.Id}' of SpawnArea spawner '{spawner.GetCleanedName()}'. Ignoring condition.", e);
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
                    Log.LogWarning($"Error while evaluating position condition '{condition.GetType().Name}' for spawn '{context.CurrentTemplate.Id}' of SpawnArea spawner '{spawner.GetCleanedName()}'. Ignoring condition.", e);
                    return true;
                }
            });
        }
        catch(Exception e)
        {
            Log.LogError($"Error while attempting to evaluate valid position '{position}' of spawn for SpawnArea spawner '{spawner.GetCleanedName()}'.", e);
            return true;
        }
    }

    private static GameObject _spawn;

    public static void GetSpawnReference(GameObject spawn)
    {
        _spawn = spawn;
    }

    public static void ModifySpawn(SpawnArea spawner)
    {
        try
        {
            if (_spawn.IsNull())
            {
                return;
            }

            SpawnSession context;
            if (!SpawnSessions.TryGet(spawner, out context) || context is null)
            {
                return;
            }

            if (context.CurrentTemplate?.Modifiers is null)
            {
                return;
            }

            var zdo = ComponentCache.GetZdo(_spawn);

            foreach (var modifier in context.CurrentTemplate.Modifiers)
            {
                try
                {
                    modifier?.Modify(_spawn, zdo);
                }
                catch (Exception e)
                {
                    Log.LogWarning("Error while attempting to apply modifier '{}' to spawn '{}' of SpawnArea spawner '{}'.", e);
                }
            }
        }
        catch (Exception e)
        {
            Log.LogError($"Error while modifying spawn '{_spawn.GetCleanedName()}' of SpawnArea spawner '{spawner.GetCleanedName()}'.", e);
        }

        _spawn = null;
    }
}
