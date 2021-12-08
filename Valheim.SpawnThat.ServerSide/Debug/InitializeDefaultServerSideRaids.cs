using System;
using System.Collections.Generic;
using HarmonyLib;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Simulators;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Services;

namespace Valheim.SpawnThat.ServerSide.Debug;

[HarmonyPatch]
internal class InitializeDefaultServerSideRaids
{
    private static DefaultRaidSimulator RaidSimulator { get; set; } = new();

    private static bool Initialized { get; set; }

    static InitializeDefaultServerSideRaids()
    {
        StateResetter.Subscribe(() =>
        {
            RaidSimulator = new();
            Initialized = false;
        });
    }

    [HarmonyPatch(typeof(RandEventSystem), nameof(RandEventSystem.Awake))]
    [HarmonyPriority(Priority.Last)]
    [HarmonyPostfix]
    public static void InitSpawner(RandEventSystem __instance)
    {
        if (Initialized)
        {
            return;
        }

        Initialized = true;

        TickService.SubscribeToUpdate(RaidSimulator.Update, TimeSpan.FromSeconds(1));

#if DEBUG
        // Get them raids rolling real quick
        __instance.m_eventIntervalMin = 0.5f;
        __instance.m_eventChance = 100f;
#endif

        foreach (var raid in __instance.m_events)
        {
            Log.LogTrace("Registering raid " + raid.m_name + " for server-side simulation.");

            List<DefaultSpawnTemplate> spawners = new(raid.m_spawn.Count);

            int index = 0;

            foreach (var spawner in raid.m_spawn)
            {
                if (spawner is null)
                {
                    continue;
                }

                Log.LogTrace($"[{raid.m_name}:{index}] {spawner.m_prefab.name}");

                var template = ConvertToTemplate(spawner);

                template.Index = index;

                spawners.Add(template);

                index++;
            }

            RaidSimulator.Raids[raid.m_name] = spawners;
        }
    }

    private static DefaultSpawnTemplate ConvertToTemplate(SpawnSystem.SpawnData spawnData)
    {
        var template = new DefaultSpawnTemplate();

        template.Enabled = spawnData.m_enabled;
        template.PrefabName = spawnData.m_prefab.name;
        template.IsRaidMob = true;
        template.MinPackSize = spawnData.m_groupSizeMin;
        template.MaxPackSize = spawnData.m_groupSizeMax;
        template.PackRadius = spawnData.m_groupRadius;
        template.MaxSpawned = spawnData.m_maxSpawned;
        template.SpawnChance = spawnData.m_spawnChance;
        template.SpawnInterval = TimeSpan.FromSeconds(spawnData.m_spawnInterval);

        // Conditions
        template.SpawnConditions.Add(new ConditionBiome(spawnData.m_biome));

        if (!spawnData.m_spawnAtDay)
        {
            template.SpawnConditions.Add(new ConditionSpawnDuringDay(false));
        }

        if (!spawnData.m_spawnAtNight)
        {
            template.SpawnConditions.Add(new ConditionSpawnDuringNight(false));
        }

        if (spawnData.m_requiredEnvironments?.Count > 0)
        {
            template.SpawnConditions.Add(new ConditionEnvironments(spawnData.m_requiredEnvironments));
        }

        if (!string.IsNullOrWhiteSpace(spawnData.m_requiredGlobalKey))
        {
            template.SpawnConditions.Add(new ConditionGlobalKeysRequireOneOf(spawnData.m_requiredGlobalKey));
        }

        template.SpawnConditions.Add(new ConditionMaxSpawnedEventMobs(spawnData.m_maxSpawned));

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
        template.Modifiers.Add(new SpawnModifierSetEventCreature());
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
