using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
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

        int existingSpawns = spawner.m_prefabs?.Count ?? 0;

        var unmodifiedIndexes = spawnerTemplate.RemoveNotConfiguredSpawns
            ? Enumerable.Range(0, existingSpawns).ToList()
            : null;

        var pos = spawner.transform.position;

        foreach (var spawn in spawnerTemplate.Spawns)
        {
            var spawnTemplate = spawn.Value;

            if (!spawnTemplate.Enabled)
            {
                continue;
            }

            if (spawn.Key < existingSpawns)
            {
                // Configure existing
                var original = spawner.m_prefabs[(int)spawn.Key];

                if (spawnTemplate.TemplateEnabled)
                {
                    Log.LogTrace($"[Destructible Spawner] Modifying spawn '{spawnTemplate.Id}' of '{spawner.GetCleanedName()}:{pos}'");

                    Modify(original, spawnTemplate);
                    DestructibleSpawnTemplateManager.SetTemplate(original, spawnTemplate);

                    if (spawnerTemplate.RemoveNotConfiguredSpawns)
                    {
                        unmodifiedIndexes.RemoveAt((int)spawn.Key);
                    }
                }

                spawns.Add(original);
            }
            else if (spawnTemplate.TemplateEnabled)
            {
                // Create new
                var newSpawn = Create(spawnTemplate);

                if (newSpawn is null)
                {
                    continue;
                }

                Log.LogTrace($"[Destructible Spawner] Creating spawn '{spawnTemplate.Id}' for '{spawner.GetCleanedName()}:{pos}'");

                DestructibleSpawnTemplateManager.SetTemplate(newSpawn, spawnTemplate);

                spawns.Add(newSpawn);
            }
        }

        if (spawnerTemplate.RemoveNotConfiguredSpawns)
        {
#if DEBUG
            Log.LogTrace("Remove not configured from spawns: " + spawns.Join(x => x.m_prefab.name));
            Log.LogTrace("Removing unmodified indexes: " + unmodifiedIndexes?.Join() ?? "");
#endif
            foreach (var index in unmodifiedIndexes)
            {
                spawns.RemoveAt(index);
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
            Log.LogWarning($"[Destructible Spawner] Unable to find prefab '{template.PrefabName}'. Skipping adding spawn.");
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
                Log.LogWarning($"[Destructible Spawner] Unable to find prefab '{template.PrefabName}'. Skipping modifying spawn.");
                return;
            }

            original.m_prefab = prefab;
        }

        original.m_weight = template.SpawnWeight ?? original.m_weight;
        original.m_minLevel = template.LevelMin ?? original.m_minLevel;
        original.m_maxLevel = template.LevelMax ?? original.m_maxLevel;
    }
}