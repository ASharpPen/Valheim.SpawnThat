using System.Collections.Generic;
using SpawnThat.Core;
using SpawnThat.Spawners.DestructibleSpawner.Managers;
using SpawnThat.Utilities.Extensions;
using static SpawnArea;

namespace SpawnThat.Spawners.DestructibleSpawner.Services;

internal static class ConfigApplicationService
{
    public static void ConfigureSpawner(SpawnArea spawner, DestructibleSpawnerTemplate spawnerTemplate)
    {
        List<SpawnData> spawns = new();

        spawner.m_levelupChance = spawnerTemplate.LevelUpChance ?? spawner.m_levelupChance;
        spawner.m_spawnIntervalSec = spawnerTemplate.SpawnInterval?.Seconds ?? spawner.m_spawnIntervalSec;
        spawner.m_triggerDistance = spawnerTemplate.ConditionPlayerWithinDistance ?? spawner.m_triggerDistance;
        spawner.m_setPatrolSpawnPoint = spawnerTemplate.SetPatrol ?? spawner.m_setPatrolSpawnPoint;
        spawner.m_spawnRadius = spawnerTemplate.ConditionPlayerWithinDistance ?? spawner.m_spawnRadius;
        spawner.m_maxNear = spawnerTemplate.ConditionMaxCloseCreatures ?? spawner.m_maxNear;
        spawner.m_maxTotal = spawnerTemplate.ConditionMaxCreatures ?? spawner.m_maxTotal;
        spawner.m_nearRadius = spawnerTemplate.DistanceConsideredClose ?? spawner.m_nearRadius;
        spawner.m_farRadius = spawnerTemplate.DistanceConsideredFar ?? spawner.m_farRadius;
        spawner.m_onGroundOnly = spawnerTemplate.OnGroundOnly ?? spawner.m_onGroundOnly;

        foreach (var spawn in spawnerTemplate.Spawns)
        {
            var template = spawn.Value;

            if (!template.Enabled)
            {
                continue;
            }

            if (spawner.m_prefabs.Count < spawn.Key)
            {
                // Configure existing
                var original = spawner.m_prefabs[(int)spawn.Key];

                if (template.TemplateEnabled)
                {
                    Log.LogTrace($"Modifying spawn '{template.Id}' of destructible spawner '{spawner.GetCleanedName()}'");

                    Modify(original, template);
                    DestructibleSpawnTemplateManager.SetTemplate(original, template);
                }

                spawns.Add(original);
            }
            else if (template.TemplateEnabled)
            {
                // Create new
                var newSpawn = Create(template);

                if (newSpawn is null)
                {
                    continue;
                }

                Log.LogTrace($"Creating spawn '{template.Id}' for destructible spawner '{spawner.GetCleanedName()}'");

                DestructibleSpawnTemplateManager.SetTemplate(newSpawn, template);

                spawns.Add(newSpawn);
            }
        }

        spawner.m_prefabs = spawns;
    }

    private static SpawnData Create(DestructibleSpawnTemplate template)
    {
        if (string.IsNullOrWhiteSpace(template.PrefabName))
        {
            return null;
        }

        var prefab = ZNetScene.instance.IsNotNull()
            ? ZNetScene.instance.GetPrefab(template.PrefabName)
            : null;

        if (prefab.IsNull())
        {
            Log.LogWarning($"Unable to find prefab '{template.PrefabName}' for destructible spawner. Skipping adding spawn.");
            return null;
        }

        return new SpawnData()
        {
            m_weight = template.SpawnWeight ?? 1,
            m_minLevel = template.LevelMin ?? 1,
            m_maxLevel = template.LevelMax ?? 1,
            m_prefab = prefab,
        };
    }

    private static void Modify(SpawnData original, DestructibleSpawnTemplate template)
    {
        if (!string.IsNullOrWhiteSpace(template.PrefabName))
        {
            var prefab = ZNetScene.instance.IsNotNull()
                ? ZNetScene.instance.GetPrefab(template.PrefabName)
                : null;
            
            if (prefab.IsNull())
            {
                Log.LogWarning($"Unable to find prefab '{template.PrefabName}' for destructible spawner. Skipping adding spawn.");
                return;
            }
        }

        original.m_weight = template.SpawnWeight ?? original.m_weight;
        original.m_minLevel = template.LevelMin ?? original.m_minLevel;
        original.m_maxLevel = template.LevelMax ?? original.m_maxLevel;
    }
}