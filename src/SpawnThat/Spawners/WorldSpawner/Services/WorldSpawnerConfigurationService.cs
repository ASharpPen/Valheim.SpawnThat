using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;
using SpawnThat.Spawners.WorldSpawner.Managers;
using SpawnThat.Utilities.Extensions;
using SpawnThat.Spawners.WorldSpawner.Debug;

namespace SpawnThat.Spawners.WorldSpawner.Services;

internal static class WorldSpawnerConfigurationService
{
    private static bool IsConfigured;
    private static bool FirstRun = true;
    private static bool DetectedUnableToFindPrefab;

    static WorldSpawnerConfigurationService()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            IsConfigured = false;
            FirstRun = true;
            DetectedUnableToFindPrefab = false;
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
            SpawnDataFileGenerator.WriteToFile(preChangeSpawners, "world_spawners_pre_changes.txt");
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
            SpawnDataFileGenerator.WriteToFile(spawnLists.SelectMany(x => x.m_spawners).ToList(), "world_spawners_post_changes.txt", true);
        }

        IsConfigured = true;

        if (DetectedUnableToFindPrefab)
        {
            Log.LogWarning(
                "Issues with finding prefabs detected.\n" +
                "The message \"Unable to find prefab\" means that Spawn That is loaded correctly, and is now trying to configure your spawns.\n" +
                "However, the listed prefab was not registered in the game, and Spawn That is therefore unable to use it.\n" +
                "Verify spelling of prefab name in Spawn That configurations, or that the creature/item is correctly loaded.");
        }
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

        var mainSpawnList = spawnLists.FirstOrDefault();

        if (mainSpawnList.IsNull())
        {
            Log.LogWarning("Something is really wrong. No SpawnSystemLists found. Skipping configuration of world spawners.");
            return;
        }

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

                ConfigureNewEntry(entry, template);
            }
            else
            {
                Log.LogTrace($"Overriding spawner entry [{id}:{spawners[id].m_prefab.GetName()}] with template '{template.TemplateName}'");
                entry = spawners[id];

                ConfigureExistingEntry(entry, template);
            }

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
                    if (spawner.m_prefab.IsNull() || string.IsNullOrWhiteSpace(spawner.m_prefab.GetName()))
                    {
                        continue;
                    }

                    var name = spawner.m_prefab.GetName();
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

    private static void ConfigureNewEntry(SpawnSystem.SpawnData entry, WorldSpawnTemplate template)
    {
        if (!template.Enabled)
        {
            entry.m_enabled = template.Enabled;
        }

        GameObject prefab = entry.m_prefab;

        if (!string.IsNullOrWhiteSpace(template.PrefabName))
        {
            prefab = ZNetScene.instance.GetPrefab(template.PrefabHash);

            if (prefab.IsNull())
            {
                Log.LogWarning($"Unable to find prefab '{template.PrefabName}' for {template.PrefabName}. Skipping world spawn template {template.TemplateName}");
                DetectedUnableToFindPrefab = true;
                return;
            }
        }

        Configure(ref entry.m_prefab, prefab);
        Configure(ref entry.m_biome, template.BiomeMask, (Heightmap.Biome)int.MaxValue);
        Configure(ref entry.m_enabled, template.Enabled);
        Configure(ref entry.m_groundOffset, template.SpawnAtDistanceToGround, 0.5f);
        Configure(ref entry.m_groupRadius, template.PackSpawnCircleRadius, 3f);
        Configure(ref entry.m_groupSizeMin, template.PackSizeMin, 1);
        Configure(ref entry.m_groupSizeMax, template.PackSizeMax, 1);
        Configure(ref entry.m_huntPlayer, template.ModifierHuntPlayer, false);
        Configure(ref entry.m_inForest, template.ConditionAllowInForest, true);
        Configure(ref entry.m_outsideForest, template.ConditionAllowOutsideForest, true);
        Configure(ref entry.m_levelUpMinCenterDistance, template.DistanceToCenterForLevelUp, 0);
        Configure(ref entry.m_minAltitude, template.ConditionMinAltitude, -1000);
        Configure(ref entry.m_maxAltitude, template.ConditionMaxAltitude, 1000);
        Configure(ref entry.m_minLevel, template.MinLevel, 1);
        Configure(ref entry.m_maxLevel, template.MaxLevel, 1);
        Configure(ref entry.m_minTilt, template.ConditionMinTilt, 0);
        Configure(ref entry.m_maxTilt, template.ConditionMaxTilt, 35);
        Configure(ref entry.m_minOceanDepth, template.ConditionMinOceanDepth, 0);
        Configure(ref entry.m_maxOceanDepth, template.ConditionMaxOceanDepth, 0);
        Configure(ref entry.m_maxSpawned, template.MaxSpawned, 1);
        Configure(ref entry.m_name, template.TemplateName, "");
        Configure(ref entry.m_requiredEnvironments, template.ConditionEnvironments, new(0));
        Configure(ref entry.m_requiredGlobalKey, template.ConditionRequiredGlobalKey, "");
        Configure(ref entry.m_spawnAtDay, template.ConditionAllowDuringDay, true);
        Configure(ref entry.m_spawnAtNight, template.ConditionAllowDuringNight, true);
        Configure(ref entry.m_spawnChance, template.SpawnChance, 100);
        Configure(ref entry.m_spawnDistance, template.MinDistanceToOther, 0);
        Configure(ref entry.m_spawnInterval, (float?)template.SpawnInterval?.TotalSeconds, 90f);
        Configure(ref entry.m_spawnRadiusMin, template.SpawnAtDistanceToPlayerMin, 0);
        Configure(ref entry.m_spawnRadiusMax, template.SpawnAtDistanceToPlayerMax, 0);
    }

    private static void ConfigureExistingEntry(SpawnSystem.SpawnData entry, WorldSpawnTemplate template)
    {
        if (!template.Enabled)
        {
            entry.m_enabled = template.Enabled;
        }

        GameObject prefab = entry.m_prefab;

        if (!string.IsNullOrWhiteSpace(template.PrefabName))
        {
            prefab = ZNetScene.instance.GetPrefab(template.PrefabHash);

            if (prefab.IsNull())
            {
                Log.LogWarning($"Unable to find prefab '{template.PrefabName}' for {template.PrefabName}. Skipping world spawn template {template.TemplateName}");
                DetectedUnableToFindPrefab = true;
                return;
            }
        }

        Configure(ref entry.m_name, template.TemplateName);
        Configure(ref entry.m_enabled, template.Enabled);
        Configure(ref entry.m_biome, template.BiomeMask);
        Configure(ref entry.m_prefab, prefab);
        Configure(ref entry.m_huntPlayer, template.ModifierHuntPlayer);
        Configure(ref entry.m_maxSpawned, template.MaxSpawned);
        Configure(ref entry.m_spawnInterval, (float?)template.SpawnInterval?.TotalSeconds);
        Configure(ref entry.m_spawnChance, template.SpawnChance);
        Configure(ref entry.m_minLevel, template.MinLevel);
        Configure(ref entry.m_maxLevel, template.MaxLevel);
        Configure(ref entry.m_levelUpMinCenterDistance, template.DistanceToCenterForLevelUp);
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

    private static void Configure<T>(ref T entry, T newValue, T defaultValue)
        where T : class
    {
        entry = newValue is not null
            ? newValue
            : defaultValue;
    }

    private static void Configure<T>(ref T entry, T? newValue, T defaultValue)
    where T : struct
    {
        entry = newValue is not null
            ? newValue.Value
            : defaultValue;
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
