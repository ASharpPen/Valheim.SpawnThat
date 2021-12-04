using System;
using HarmonyLib;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Services;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Simulators;

namespace Valheim.SpawnThat.ServerSide.Debug;

[HarmonyPatch]
public static class InitializeDefaultServerSideSpawner
{
    private static DefaultSpawnSystemSimulator defaultSimulator = new();

    private static bool Initialized = false;

    static InitializeDefaultServerSideSpawner()
    {
        StateResetter.Subscribe(() =>
        {
            defaultSimulator = new();
            Initialized = false;
        });

        TickService.SubscribeToUpdate(defaultSimulator.Update);
    }

    [HarmonyPatch(typeof(SpawnSystem), nameof(SpawnSystem.Awake))]
    [HarmonyPriority(Priority.Last)]
    [HarmonyPostfix]
    public static void InitSpawner(SpawnSystem __instance)
    {
        if (Initialized)
        { 
            return;
        }

        Initialized = true;

        int index = 0;

        foreach(var spawnList in __instance.m_spawnLists)
        {
            foreach(var spawner in spawnList.m_spawners)
            {
                if(spawner is null)
                {
                    continue;
                }

                var template = ConvertToTemplate(spawner);

                template.Index = index;

                defaultSimulator.Templates.Add(template);

                index++;
            }
        }
    }

    private static DefaultSpawnSystemTemplate ConvertToTemplate(SpawnSystem.SpawnData spawnData)
    {
        var template = new DefaultSpawnSystemTemplate();

        template.Enabled = spawnData.m_enabled;
        template.PrefabName = spawnData.m_prefab.name;
        template.MinSpawned = spawnData.m_groupSizeMin;
        template.MaxSpawned = spawnData.m_groupSizeMax;
        template.Radius = spawnData.m_groupRadius;
        template.SpawnChance = spawnData.m_spawnChance;
        template.SpawnInterval = TimeSpan.FromSeconds(spawnData.m_spawnInterval);

        // Conditions
        template.SpawnConditions.Add(new ConditionBiome(spawnData.m_biome));

        if (!spawnData.m_spawnAtDay)
        {
            template.SpawnConditions.Add(new ConditionIsDay());
        }

        if (!spawnData.m_spawnAtNight)
        {
            template.SpawnConditions.Add(new ConditionIsNight());
        }

        if (spawnData.m_requiredEnvironments?.Count > 0)
        {
            template.SpawnConditions.Add(new ConditionEnvironments(spawnData.m_requiredEnvironments));
        }

        if (!string.IsNullOrWhiteSpace(spawnData.m_requiredGlobalKey))
        {
            template.SpawnConditions.Add(new ConditionGlobalKeysRequireOneOf(spawnData.m_requiredGlobalKey));
        }

        template.SpawnConditions.Add(new ConditionMaxSpawned(spawnData.m_maxSpawned));

        // Positions
        template.PositionConditions.Add(new PositionConditionForest(spawnData.m_inForest, spawnData.m_outsideForest));
        template.PositionConditions.Add(new PositionConditionAltitude(spawnData.m_minAltitude, spawnData.m_maxAltitude));

        if (spawnData.m_minOceanDepth != spawnData.m_maxOceanDepth)
        {
            template.PositionConditions.Add(new PositionConditionOceanDepth(spawnData.m_minOceanDepth, spawnData.m_maxOceanDepth));
        }

        template.PositionConditions.Add(new PositionConditionSurfaceTilt(spawnData.m_minTilt, spawnData.m_maxTilt));
        template.PositionConditions.Add(new PositionConditionDistanceToPlayer(spawnData.m_spawnDistance));

        // Positions - Simulated
        template.PositionConditions.Add(new PositionConditionNotBlocked());
        template.PositionConditions.Add(new PositionConditionPlayerBase());
        template.PositionConditions.Add(new PositionConditionSpawnerBiome());

        // Modifiers
        if (spawnData.m_huntPlayer)
        {
            template.Modifiers.Add(new SpawnModifierSetHuntPlayer());
        }

        template.Modifiers.Add(new SpawnModifierDefaultRollLevel(
            spawnData.m_minLevel, 
            spawnData.m_maxLevel, 
            spawnData.m_levelUpMinCenterDistance, 
            10));

        return template;
    }
}
