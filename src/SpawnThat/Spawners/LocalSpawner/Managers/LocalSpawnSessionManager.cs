#define VERBOSE

using System;
using UnityEngine;
using SpawnThat.Caches;
using SpawnThat.Core;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Spawners.LocalSpawner.Managers;

internal static class LocalSpawnSessionManager
{
    internal static float GetChanceToLevelUp(float defaultChance, CreatureSpawner spawner)
        => LocalSpawnerManager.GetTemplate(spawner)?.LevelUpChance ?? defaultChance;

    internal static bool CheckConditionsValid(CreatureSpawner spawner)
    {
#if DEBUG && VERBOSE
        Log.LogTrace($"Testing conditions of spawner '{spawner.name}:{spawner.transform.position}'");
#endif

        var template = LocalSpawnerManager.GetTemplate(spawner);

        if (template is not null && !template.Enabled)
        {
            return false;
        }

        if (template?.SpawnConditions is null)
        {
            return true;
        }

        var spawnerZdo = ComponentCache.GetZdo(spawner);

        if (spawnerZdo is null)
        {
            return true;
        }

        SpawnSessionContext context = new(spawnerZdo);

        foreach (var condition in template.SpawnConditions)
        {
            try
            {
                var validCondition = condition?.IsValid(context) ?? true;

                if (!validCondition)
                {
#if DEBUG && VERBOSE
                    Log.LogTrace($"Local Spawner '{spawner.name}', Invalid condition '{condition.GetType().Name}'");
#endif
                    return false;
                }
            }
            catch (Exception e)
            {
                Log.LogError($"Error while attempting to check spawn condition '{condition?.GetType()?.Name}' for local spawner '{spawner.name}'. Ignoring condition", e);
            }
        }

        return true;
    }

    private static GameObject _spawn;

    public static void GetSpawnReference(GameObject spawn)
    {
        _spawn = spawn;
    }

    internal static void ModifySpawn(CreatureSpawner spawner)
    {
        if (_spawn.IsNull())
        {
            return;
        }

        var template = LocalSpawnerManager.GetTemplate(spawner);

        if (template is null)
        {
#if DEBUG
            Log.LogTrace($"Found no config for {_spawn}.");
#endif
            return;
        }

        Log.LogTrace($"Applying modifiers to spawn {_spawn.name}");

        var spawnZdo = ComponentCache.GetZdo(_spawn);

        if (template.Modifiers is not null)
        {
            foreach (var modifier in template.Modifiers)
            {
                try
                {
                    modifier?.Modify(_spawn, spawnZdo);
                }
                catch (Exception e)
                {
                    Log.LogError($"Error while attempting to apply modifier '{modifier?.GetType()?.Name}' to local spawner '{spawner.name}'", e);
                }
            }
        }
    }
}
