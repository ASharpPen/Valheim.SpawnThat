using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Debugging;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;

namespace SpawnThat.Spawners.WorldSpawner.Services;

internal static class WorldSpawnerConfigurationService
{
    private static bool IsConfigured;
    private static bool FirstRun = true;

    static WorldSpawnerConfigurationService()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            IsConfigured = false;
            FirstRun = true;
        });
    }

    public static void ConfigureSpawnLists(List<SpawnSystemList> spawnLists)
    {
        if (IsConfigured)
        {
            return;
        }

        Log.LogTrace($"Configuring world spawner entries");

        if (FirstRun && 
            ConfigurationManager.GeneralConfig?.WriteSpawnTablesToFileBeforeChanges?.Value == true)
        {
            var preChangeSpawners = spawnLists.SelectMany(x => x.m_spawners).ToList();
            SpawnDataFileDumper.WriteToFile(preChangeSpawners, "world_spawners_pre_changes.txt");
        }

        if (ConfigurationManager.GeneralConfig?.ClearAllExisting?.Value == true)
        {
            Log.LogTrace($"Clearing all existing world spawner entries");
            spawnLists.ForEach(x => x.m_spawners.Clear());
        }

        ApplyWorldSpawnerTemplates(spawnLists);

        // TODO: Depends on whether simple templates are merged into world spawn templates or not.
        ApplySimpleTemplates(spawnLists);

        if (ConfigurationManager.GeneralConfig?.WriteSpawnTablesToFileAfterChanges?.Value == true)
        {
            SpawnDataFileDumper.WriteToFile(spawnLists.SelectMany(x => x.m_spawners).ToList(), "world_spawners_post_changes", true);
        }

        IsConfigured = true;
    }

    private static void ApplyWorldSpawnerTemplates(List<SpawnSystemList> spawnLists)
    {
        var templates = WorldSpawnTemplateManager
            .GetTemplates()
            .Where(x => x.template.TemplateEnabled)
            .OrderBy(x => x.id)
            .ToList();

        Log.LogTrace($"Found {templates.Count} world spawner templates to apply.");

        if (templates.Count == 0)
        {
            return;
        }

        var spawners = spawnLists
            .SelectMany(x => x.m_spawners)
            .ToList();

        var mainSpawnList = spawnLists.FirstOrDefault() ?? new();

        foreach((int id, WorldSpawnTemplate template) in templates)
        {
            // Validate
            if (!template.TemplateEnabled)
            {
                continue;
            }

            SpawnSystem.SpawnData entry;

            if (ConfigurationManager.GeneralConfig?.AlwaysAppend?.Value == true ||
                id >= spawners.Count)
            {
                entry = new SpawnSystem.SpawnData();
                mainSpawnList.m_spawners.Add(entry);

                Log.LogTrace($"Creating spawner entry for template [{id}:{template.PrefabName}]");
            }
            else
            {
                Log.LogTrace($"Overriding spawner entry [{id}:{spawners[id].m_prefab.name}] with template [{id}:{template.PrefabName}]");
                entry = spawners[id];
            }

            ConfigureEntry(entry, template);
            WorldSpawnerManager.SetTemplate(entry, template);
        }
    }

    private static void ApplySimpleTemplates(List<SpawnSystemList> spawnLists)
    {
        var simpleConfigs = SpawnSystemConfigurationManager.SimpleConfig?.Subsections;

        if (simpleConfigs is not null && simpleConfigs.Count > 0)
        {
            foreach (var spawnList in spawnLists)
            {
                foreach (var spawner in spawnList.m_spawners)
                {
                    if (string.IsNullOrWhiteSpace(spawner.m_prefab?.name))
                    {
                        continue;
                    }

                    var name = spawner.m_prefab.name;
                    var cleanedName = name.Trim().ToUpper();

                    if (simpleConfigs.TryGetValue(cleanedName, out SimpleConfig simpleConfig))
                    {
                        spawner.m_maxSpawned = (int)Math.Round(spawner.m_maxSpawned * simpleConfig.SpawnMaxMultiplier.Value);
                        spawner.m_groupSizeMin = (int)Math.Round(spawner.m_groupSizeMin * simpleConfig.GroupSizeMinMultiplier.Value);
                        spawner.m_groupSizeMax = (int)Math.Round(spawner.m_groupSizeMax * simpleConfig.GroupSizeMaxMultiplier.Value);
                        spawner.m_spawnInterval = simpleConfig.SpawnFrequencyMultiplier.Value != 0
                            ? spawner.m_spawnInterval / simpleConfig.SpawnFrequencyMultiplier.Value
                            : 0;
                    }
                }
            }
        }
    }

    private static void ConfigureEntry(SpawnSystem.SpawnData entry, WorldSpawnTemplate template)
    {
        GameObject prefab = null;

        if (!string.IsNullOrWhiteSpace(template.PrefabName))
        {
            prefab = ZNetScene.instance.GetPrefab(template.PrefabHash);
        }

        if (!prefab || prefab is null)
        {
            Log.LogWarning($"Unable to find prefab for {template.PrefabName}. Skipping world spawn template {template.TemplateName}");
            return;
        }

        float? spawnInterval = template.SpawnInterval is null
            ? null
            : (float)template.SpawnInterval.Value.TotalSeconds;

        Configure(ref entry.m_name, template.TemplateName);
        Configure(ref entry.m_enabled, template.Enabled);
        Configure(ref entry.m_biome, template.BiomeMask);
        Configure(ref entry.m_prefab, prefab);
        Configure(ref entry.m_huntPlayer, template.ModifierHuntPlayer);
        Configure(ref entry.m_maxSpawned, template.MaxSpawned);
        Configure(ref entry.m_spawnInterval, spawnInterval);
        Configure(ref entry.m_spawnChance, template.SpawnChance);
        Configure(ref entry.m_minLevel, template.MinLevel);
        Configure(ref entry.m_maxLevel, template.MaxLevel);
        Configure(ref entry.m_levelUpMinCenterDistance, template.LevelUpDistance);
        Configure(ref entry.m_spawnDistance, template.MinDistanceToOther);
        Configure(ref entry.m_spawnRadiusMin, template.SpawnAtDistanceToPlayerMin);
        Configure(ref entry.m_spawnRadiusMax, template.SpawnAtDistanceToPlayerMax);
        Configure(ref entry.m_requiredGlobalKey, template.ConditionRequiredGlobalKey);
        Configure(ref entry.m_requiredEnvironments, template.ConditionEnvironments);
        Configure(ref entry.m_groupSizeMin, template.PackSizeMin);
        Configure(ref entry.m_groupSizeMax, template.PackSizeMax);
        Configure(ref entry.m_groupRadius, template.PackSpawnCircleRadius);
        Configure(ref entry.m_groundOffset, template.SpawnAtDistanceToGround);
        Configure(ref entry.m_spawnAtDay, template.ConditionAllowDuringDay);
        Configure(ref entry.m_spawnAtNight, template.ConditionAllowDuringNight);
        Configure(ref entry.m_minAltitude, template.ConditionMinAltitude);
        Configure(ref entry.m_maxAltitude, template.ConditionMaxAltitude);
        Configure(ref entry.m_minTilt, template.ConditionMinTilt);
        Configure(ref entry.m_maxTilt, template.ConditionMaxTilt);
        Configure(ref entry.m_inForest, template.ConditionAllowInForest);
        Configure(ref entry.m_outsideForest, template.ConditionAllowOutsideForest);
        Configure(ref entry.m_minOceanDepth, template.ConditionMinOceanDepth);
        Configure(ref entry.m_maxOceanDepth, template.ConditionMaxOceanDepth);
    }

    private static void Configure<T>(ref T entry, T newValue)
    {
        if (newValue is null)
        {
            return;
        }

        entry = newValue;
    }

    private static void Configure<T>(ref T entry, T? newValue)
        where T : struct
    {
        if (!newValue.HasValue)
        {
            return;
        }

        entry = newValue.Value;
    }
}
