using System;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.World.Queries;

namespace Valheim.SpawnThat.Spawners.WorldSpawner;

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
            Spawner = spawner;
            Context = new(ComponentCache.GetZdo(Spawner));
        }
        catch(Exception e)
        {
            Log.LogError("Error during initialization of new SpawnSystem session", e);
        }
    }

    public static void StartSpawnSession(SpawnSystem.SpawnData currentEntry)
    {
        SpawnDataEntry = currentEntry;
        SpawnTemplate = WorldSpawnerManager.GetTemplate(SpawnDataEntry);
    }

    public static bool ValidSpawnEntry(bool eventSpawner)
    {
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
                return x?.IsValid(Context) ?? true;
            }
            catch(Exception e)
            {
                Log.LogError($"Error while attempting to check spawn condition {x.GetType().Name} for world spawner template '{SpawnTemplate?.TemplateName}'. Skipping spawn", e);
                return false;
            }
        });
    }

    public static bool AnyInRange(GameObject prefab, Vector3 center, int radius)
    {
        var query = new ZdoPrefabQuery(center, radius);
        var prefabHash = prefab.name.GetStableHashCode();

        return query.HasAny(prefabHash);
    }

    public static int CountEntitiesInArea(GameObject entityPrefab, Vector3 center, float range)
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

    public static bool ValidSpawnPosition(Vector3 pos)
    {
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

    public static void ModifySpawn(GameObject spawn, bool isEventCreature)
    {
        if (isEventCreature)
        {
            return;
        }

        if (SpawnTemplate?.SpawnModifiers is null)
        {
            return;
        }

        var zdo = ComponentCache.GetZdo(spawn);

        foreach(var modifier in SpawnTemplate.SpawnModifiers)
        {
            try
            {
                modifier?.Modify(spawn, zdo);
            }
            catch (Exception e)
            {
                Log.LogError($"Error while attempting to apply modifier {modifier.GetType().Name} to entity '{spawn?.name}' from world spawn template {SpawnTemplate.TemplateName}", e);
            }
        }
    }
}
