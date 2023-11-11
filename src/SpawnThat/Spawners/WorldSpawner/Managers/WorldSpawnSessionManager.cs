//#define VERBOSE

using System;
using System.Linq;
using UnityEngine;
using SpawnThat.Caches;
using SpawnThat.Core;
using SpawnThat.Spawners.Contexts;
using SpawnThat.World.Queries;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Spawners.WorldSpawner.Managers;

internal static class WorldSpawnSessionManager
{
    private static SpawnSystem Spawner { get; set; }
    private static SpawnSystem.SpawnData SpawnDataEntry { get; set; }
    private static WorldSpawnTemplate SpawnTemplate { get; set; }
    private static SpawnSessionContext Context { get; set; }

    public static void StartSession(SpawnSystem spawner)
    {
        try
        {
#if FALSE && DEBUG && VERBOSE
            Log.LogDebug("WorldSpawnSessionManager.StartSession");
#endif

            Spawner = spawner;
            Context = new(ComponentCache.GetZdo(Spawner));
        }
        catch (Exception e)
        {
            Log.LogError("Error during initialization of new SpawnSystem session", e);
        }
    }

    public static void StartSpawnSession(SpawnSystem.SpawnData currentEntry)
    {
        try
        {
#if DEBUG && VERBOSE
            Log.LogDebug("WorldSpawnSessionManager.StartSpawnSession");
#endif
            SpawnDataEntry = currentEntry;
            SpawnTemplate = WorldSpawnerManager.GetTemplate(SpawnDataEntry);
        }
        catch (Exception e)
        {
            Log.LogError("Error during initialization of spawner entry session.", e);
        }
    }

    public static bool ValidSpawnEntry(bool eventSpawner)
    {
#if DEBUG && VERBOSE
        Log.LogDebug("WorldSpawnSessionManager.ValidSpawnEntry");
#endif
        if (eventSpawner)
        {
            return true;
        }

        if (Context is null)
        {
            return true;
        }

        if (SpawnTemplate?.SpawnConditions is null)
        {
            return true;
        }

        return SpawnTemplate.SpawnConditions.All(x =>
        {
            try
            {
                var isValid = x?.IsValid(Context) ?? true;
#if DEBUG && VERBOSE
                if (!isValid)
                {
                    Log.LogDebug($"[{SpawnTemplate.Index}:{SpawnTemplate.PrefabName}] condition {x.GetType().Name} is invalid.");
                }
#endif
                return isValid;
            }
            catch (Exception e)
            {
                Log.LogError($"Error while attempting to check spawn condition {x.GetType().Name} for world spawner template '{SpawnTemplate?.TemplateName}'. Skipping spawn", e);
                return false;
            }
        });
    }

    public static int GetSpawnEntryId(int originalId)
    {
        return SpawnTemplate?.Index ?? originalId;
    }

    public static int GetCustomHash(int original, string type, int index, GameObject prefab)
    {
        if (SpawnTemplate is null)
        {
            return original;
        }
        return (type + SpawnTemplate.Index + prefab.name).GetStableHashCode();
    }

    public static bool AnyInRange(GameObject prefab, Vector3 center, int radius)
    {
#if DEBUG && VERBOSE
        Log.LogDebug("WorldSpawnSessionManager.AnyInRange");
#endif
        if (prefab.IsNull())
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(prefab.name))
        {
            Log.LogDebug($"Unable to count '{prefab}' during world spawner checks, due to object.name being empty.");
            return false;
        }

        try
        {
            var query = new ZdoPrefabQuery(center, radius);
            var prefabHash = prefab.name.GetStableHashCode();

            return query.HasAny(prefabHash);
        }
        catch (Exception e)
        {
            Log.LogError($"Error while attempting to find any entity {prefab} nearby", e);
            return false;
        }
    }

    public static int CountEntitiesInArea(GameObject entityPrefab, Vector3 center, float range)
    {
#if DEBUG && VERBOSE
        Log.LogDebug("WorldSpawnSessionManager.CountEntitiesInArea");
#endif
        if (entityPrefab.IsNull())
        {
            return 0;
        }

        if (string.IsNullOrWhiteSpace(entityPrefab.name))
        {
            Log.LogDebug($"Unable to count '{entityPrefab}' during world spawner checks, due to object.name being empty.");
            return 0;
        }

        try
        {
            ZdoPrefabQuery query;
            if (center == Vector3.zero && range == 0)
            {
                // TODO: Consider lowering the range, or making it configurable.
                query = new ZdoPrefabQuery(Context.SpawnerZdo.m_position, 250);
            }
            else
            {
                query = new ZdoPrefabQuery(center, (int)range);
            }

            var prefabHash = entityPrefab.name.GetStableHashCode();

            return query.CountEntities(prefabHash);
        }
        catch (Exception e)
        {
            Log.LogError($"Error while attempting to count nearby entities {entityPrefab}", e);
            return 0;
        }
    }

    public static bool ValidSpawnPosition(Vector3 pos)
    {
#if DEBUG && VERBOSE
        Log.LogDebug("WorldSpawnSessionManager.ValidSpawnPosition");
#endif
        if (SpawnTemplate?.SpawnPositionConditions is null)
        {
            return true;
        }

        return SpawnTemplate.SpawnPositionConditions.All(x =>
        {
            try
            {
                return x?.IsValid(Context, pos) ?? true;
            }
            catch (Exception e)
            {
                Log.LogError($"Error while attempting to check spawn position condition {x.GetType().Name} for world spawner template '{SpawnTemplate?.TemplateName}'. Ignoring condition", e);
                return true;
            }
        });
    }

    private static GameObject _spawn;
    private static bool _isEventCreature;

    public static void GetSpawnReference(GameObject spawn, bool isEventCreature)
    {
        _spawn = spawn;
        _isEventCreature = isEventCreature;
    }

    public static void ModifySpawn()
    {
        if (_spawn.IsNotNull())
        {
#if DEBUG && VERBOSE
        Log.LogDebug("WorldSpawnSessionManager.ModifySpawn");
#endif
            if (_isEventCreature)
            {
                return;
            }

            if (SpawnTemplate?.SpawnModifiers is null)
            {
                return;
            }

            var zdo = ComponentCache.GetZdo(_spawn);

            if (zdo is null)
            {
                return;
            }

            foreach (var modifier in SpawnTemplate.SpawnModifiers)
            {
                try
                {
                    modifier?.Modify(_spawn, zdo);
                }
                catch (Exception e)
                {
                    string spawnName = _spawn.IsNotNull()
                        ? _spawn.name
                        : "";

                    Log.LogError($"Error while attempting to apply modifier {modifier.GetType().Name} to entity '{spawnName}' from world spawn template {SpawnTemplate.TemplateName}", e);
                }
            }
        }

        _spawn = null;
    }
}
