#define VERBOSE

using System;
using UnityEngine;
using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.Contexts;

namespace Valheim.SpawnThat.Spawners.LocalSpawner;

internal static class LocalSpawnSessionManager
{
    internal static float GetChanceToLevelUp(float defaultChance, CreatureSpawner spawner) 
        => LocalSpawnerManager.GetTemplate(spawner)?.LevelUpChance ?? defaultChance;

    internal static bool CheckConditionsValid(CreatureSpawner spawner)
    {
        var template = LocalSpawnerManager.GetTemplate(spawner);

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

        foreach(var condition in template.SpawnConditions)
        {
            try
            {
                var validCondition = condition?.IsValid(context) ?? true;

                if (!validCondition)
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                Log.LogError($"Error while attempting to check spawn condition '{condition?.GetType()?.Name}' for local spawner '{spawner.name}'. Ignoring condition", e);
            }
        }

        return true;
    }

    internal static void ModifySpawn(CreatureSpawner spawner, GameObject spawn)
    {
        var template = LocalSpawnerManager.GetTemplate(spawner);

        if (template is null)
        {
#if DEBUG
            Log.LogTrace($"Found no config for {spawn}.");
#endif
            return;
        }

        Log.LogTrace($"Applying modifiers to spawn {spawn.name}");

        var spawnZdo = ComponentCache.GetZdo(spawn);

        if (template.Modifiers is not null)
        {
            foreach (var modifier in template.Modifiers)
            {
                try
                {
                    modifier?.Modify(spawn, spawnZdo);
                }
                catch(Exception e)
                {
                    Log.LogError($"Error while attempting to apply modifier '{modifier?.GetType()?.Name}' to local spawner '{spawner.name}'", e);
                }
            }
        }
    }
}
