﻿using System;
using System.Collections.Generic;
using System.Linq;
using SpawnThat.Configuration;
using SpawnThat.Core.Toml;
using SpawnThat.Integrations.CLLC.Conditions;
using SpawnThat.Integrations.CLLC.Modifiers;
using SpawnThat.Integrations.EpicLoot.Conditions;
using SpawnThat.Integrations.MobAi.Modifiers;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Identifiers;
using SpawnThat.Options.Modifiers;
using SpawnThat.Options.PositionConditions;
using SpawnThat.Spawners.SpawnAreaSpawner.Configuration.BepInEx;
using SpawnThat.Spawners.SpawnAreaSpawner.Managers;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Debug;

internal static class TemplateWriter
{
    public static void WriteToDiskAsToml()
    {
        var tomlFile = PrepareTomlFile();

        TomlWriter.WriteToDisk(tomlFile, new()
        {
            FileName = "spawn_that.spawnarea_spawners_loaded_configs.cfg",
            FileDescription = "loaded SpawnArea spawner configs",
            AddComments = ConfigurationManager.GeneralConfig?.SpawnAreaAddCommentsToFile.Value ?? false,
            Header =
                $"# This file was auto-generated by Spawn That {SpawnThatPlugin.Version} at {DateTimeOffset.UtcNow.ToString("u")}, with Valheim '{Version.CurrentVersion.m_major}.{Version.CurrentVersion.m_minor}.{Version.CurrentVersion.m_patch}'.\n" +
                $"# The entries listed here were generated from the internally loaded SpawnArea spawner configurations.\n" +
                $"# This is intended to reveal the state of Spawn That, after loading configs from all sources, and before applying them to in-game spawners.\n" +
                $"# This file is not scanned by Spawn That, and any changes done will therefore have no effect. Copy sections into a SpawnArea spawner config in the configs folder, if you want to change things.\n" +
                $"# Be aware, for entries added by mods using the Spawn That API, it might not be showing the full truth but an approximation, since the API can do a lot more than the config file allows.\n"
        });

    }

    internal static SpawnAreaSpawnerConfigurationFile PrepareTomlFile()
    {
        var spawnerTemplates = SpawnAreaSpawnerManager.GetTemplates();

        var tomlFile = new SpawnAreaSpawnerConfigurationFile();

        Dictionary<string, int> templateNamesUsed = new();

        foreach (var spawnerTemplate in spawnerTemplates)
        {
            string templateName = spawnerTemplate.TemplateName ?? "SpawnAreaSpawner";

            if (templateNamesUsed.TryGetValue(templateName, out var usageCounter))
            {
                ++usageCounter;
                templateName += "_" + usageCounter;

                templateNamesUsed[templateName] = usageCounter;
            }
            else
            {
                templateNamesUsed[templateName] = 1;
            }

            var config = tomlFile.GetGenericSubsection(templateName);

            ConfigureSpawnerConfig(config, spawnerTemplate);

            foreach (var spawnTemplate in spawnerTemplate.Spawns)
            {
                var spawnConfig = config.GetGenericSubsection(spawnTemplate.Key.ToString());

                ConfigureSpawnConfig(spawnConfig, spawnTemplate.Value);
            }
        }

        return tomlFile;

        static void ConfigureSpawnerConfig(SpawnAreaSpawnerConfig config, SpawnAreaSpawnerTemplate template)
        {
            // Identifiers
            foreach (var identifier in template.Identifiers)
            {
                switch (identifier)
                {
                    case IdentifierName identifierName:
                        config.IdentifyByName.Set(identifierName.Names?.ToList());
                        break;
                    case IdentifierBiome identifierBiome:
                        config.IdentifyByBiome.Set(identifierBiome.BitmaskedBiome.Split());
                        break;
                    case IdentifierLocation identifierLocation:
                        config.IdentifyByLocation.Set(identifierLocation.Locations?.ToList());
                        break;
                    case IdentifierRoom identifierRoom:
                        config.IdentifyByRoom.Set(identifierRoom.Rooms?.ToList());
                        break;
                    default:
                        break;
                }
            }

            // Default Settings
            config.LevelUpChance.SetConditionally(template.LevelUpChance);
            config.SpawnInterval.SetConditionally((float?)template.SpawnInterval?.TotalSeconds);
            config.SetPatrol.SetConditionally(template.SetPatrol);
            config.ConditionPlayerWithinDistance.SetConditionally(template.ConditionPlayerWithinDistance);
            config.SpawnRadius.SetConditionally(template.SpawnRadius);
            config.ConditionMaxCloseCreatures.SetConditionally(template.ConditionMaxCloseCreatures);
            config.ConditionMaxCreatures.SetConditionally(template.ConditionMaxCreatures);
            config.DistanceConsideredClose.SetConditionally(template.DistanceConsideredClose);
            config.DistanceConsideredFar.SetConditionally(template.DistanceConsideredFar);
            config.OnGroundOnly.SetConditionally(template.OnGroundOnly);

            // Custom settings
            config.RemoveNotConfiguredSpawns.SetConditionally(template.RemoveNotConfiguredSpawns);
        }

        static void ConfigureSpawnConfig(SpawnAreaSpawnConfig config, SpawnAreaSpawnTemplate template)
        {
            config.Enabled.Set(template.Enabled);
            config.TemplateEnabled.SetConditionally(template.TemplateEnabled);
            config.PrefabName.SetConditionally(template.PrefabName);
            config.SpawnWeight.SetConditionally(template.SpawnWeight);
            config.LevelMin.SetConditionally(template.LevelMin);
            config.LevelMax.SetConditionally(template.LevelMax);

            // Conditions
            foreach (var condition in template.Conditions)
            {
                switch(condition)
                {
                    case ConditionDistanceToCenter distanceToCenter:
                        config.ConditionDistanceToCenterMin.Set((float?)distanceToCenter.MinDistance);
                        config.ConditionDistanceToCenterMax.Set((float?)distanceToCenter.MaxDistance);
                        break;
                    case ConditionWorldAge worldAge:
                        config.ConditionWorldAgeDaysMin.Set(worldAge.MinDays);
                        config.ConditionWorldAgeDaysMax.Set(worldAge.MaxDays);
                        break;
                    case ConditionNearbyPlayersCarryValue nearbyPlayersCarryValue:
                        config.ConditionNearbyPlayersCarryValue.Set(nearbyPlayersCarryValue.RequiredValue);
                        break;
                    case ConditionNearbyPlayersCarryItem nearbyPlayersCarryItem:
                        config.ConditionNearbyPlayerCarriesItem.Set(nearbyPlayersCarryItem.ItemsSearchedFor?.ToList());
                        break;
                    case ConditionNearbyPlayersNoise nearbyPlayersNoise:
                        config.ConditionNearbyPlayersNoiseThreshold.Set(nearbyPlayersNoise.NoiseThreshold);
                        break;
                    case ConditionNearbyPlayersStatus nearbyPlayersStatus:
                        config.ConditionNearbyPlayersStatus.Set(nearbyPlayersStatus.RequiredStatusEffects?.ToList());
                        break;
                    case ConditionAreaSpawnChance areaSpawnChance:
                        config.ConditionAreaSpawnChance.Set(areaSpawnChance.AreaChance);
                        break;
                    case ConditionLocation location:
                        config.ConditionLocation.Set(location.Locations?.ToList());
                        break;
                    case ConditionAreaIds areaIds:
                        config.ConditionAreaIds.Set(areaIds.RequiredAreaIds?.ToList());
                        break;
                    case ConditionBiome biome:
                        config.ConditionBiome.Set(biome.BiomeMask.Split());
                        break;
                    case ConditionGlobalKeysRequired globalKeysRequired:
                        config.ConditionAllOfGlobalKeys.Set(globalKeysRequired.Required?.ToList());
                        break;
                    case ConditionGlobalKeysAny globalKeysAny:
                        config.ConditionAnyOfGlobalKeys.Set(globalKeysAny.Keys?.ToList());
                        break;
                    case ConditionGlobalKeysRequiredMissing globalKeysRequiredMissing:
                        config.ConditionNoneOfGlobalKeys.Set(globalKeysRequiredMissing.RequiredMissing?.ToList());
                        break;
                    case ConditionEnvironment environment:
                        config.ConditionEnvironment.Set(environment.Environments?.ToList());
                        break;
                    case ConditionDaytime daytime:
                        config.ConditionDaytime.Set(daytime.Required?.ToList());
                        break;
                    case ConditionAltitude altitude:
                        config.ConditionAltitudeMin.Set(altitude.Min);
                        config.ConditionAltitudeMax.Set(altitude.Max);
                        break;
                    case ConditionForest forest:
                        config.ConditionForestState.Set(forest.Required);
                        break;
                    case ConditionOceanDepth oceanDepth:
                        config.ConditionOceanDepthMin.Set(oceanDepth.Min);
                        config.ConditionOceanDepthMax.Set(oceanDepth.Max);
                        break;

                    // Integrations - CLLC
                    case ConditionWorldLevel worldLevel:
                        config.CllcConfig?.ConditionWorldLevelMin.Set(worldLevel.MinWorldLevel);
                        config.CllcConfig?.ConditionWorldLevelMax.Set(worldLevel.MaxWorldLevel);
                        break;

                    // Integrations - Epic Loot
                    case ConditionNearbyPlayerCarryItemWithRarity nearbyPlayerCarryItemWithRarity:
                        config.EpicLootConfig?.ConditionNearbyPlayerCarryItemWithRarity.Set(nearbyPlayerCarryItemWithRarity.RaritiesRequired?.ToList());
                        break;
                    case ConditionNearbyPlayerCarryLegendaryItem nearbyPlayerCarryLegendaryItem:
                        config.EpicLootConfig?.ConditionNearbyPlayerCarryLegendaryItem.Set(nearbyPlayerCarryLegendaryItem.LegendaryIds?.ToList());
                        break;
                    default:
                        break;
                }
            }

            // Position Conditions

            foreach (var positionCondition in template.PositionConditions)
            {
                switch (positionCondition)
                {
                    case PositionConditionLocation location:
                        config.ConditionLocation.Set(location.LocationNames?.ToList());
                        break;
                    case PositionConditionMustBeNearAllPrefabs cond:
                        config.ConditionPositionMustBeNearAllPrefabs.Set(cond.Prefabs.ToList());
                        config.ConditionPositionMustBeNearAllPrefabsDistance.Set(cond.Distance);
                        break;
                    case PositionConditionMustBeNearPrefabs cond:
                        config.ConditionPositionMustBeNearPrefabs.Set(cond.Prefabs.ToList());
                        config.ConditionPositionMustBeNearPrefabsDistance.Set(cond.Distance);
                        break;
                    case PositionConditionMustNotBeNearPrefabs cond:
                        config.ConditionPositionMustNotBeNearPrefabs.Set(cond.Prefabs.ToList());
                        config.ConditionPositionMustNotBeNearPrefabsDistance.Set(cond.Distance);
                        break;
                }
            }

            // Modifiers
            foreach (var modifier in template.Modifiers)
            {
                switch (modifier)
                {
                    case ModifierSetFaction setFaction:
                        config.SetFaction.Set(setFaction.Faction);
                        break;
                    case ModifierSetRelentless setRelentless:
                        config.SetRelentless.Set(setRelentless.Relentless);
                        break;
                    case ModifierDespawnOnAlert despawnOnAlert:
                        config.SetTryDespawnOnAlert.Set(despawnOnAlert.DespawnOnAlert);
                        break;
                    case ModifierSetTamed setTamed:
                        config.SetTamed.Set(setTamed.Tamed);
                        break;
                    case ModifierSetTamedCommandable tamedCommandable:
                        config.SetTamedCommandable.Set(tamedCommandable.Commandable);
                        break;
                    case ModifierSetHuntPlayer setHuntPlayer:
                        config.SetHuntPlayer.Set(setHuntPlayer.HuntPlayer);
                        break;

                    // Integrations - CLLC
                    case ModifierCllcInfusion infusion:
                        config.CllcConfig?.SetInfusion.Set(infusion.Infusion);
                        break;
                    case ModifierCllcExtraEffect extraEffect:
                        config.CllcConfig?.SetExtraEffect.Set(extraEffect.ExtraEffect);
                        break;
                    case ModifierCllcBossAffix bossAffix:
                        config.CllcConfig?.SetBossAffix.Set(bossAffix.Affix);
                        break;
                    case ModifierDefaultRollLevel defaultRollLevel:
                        config.CllcConfig?.UseDefaultLevels.Set(true);
                        break;

                    // Integrations - MobAI
                    case ModifierSetAI setAI:
                        config.MobAIConfig?.SetAI.Set(setAI.AiName);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private static void Set<T>(this TomlConfigEntry<T> config, T value)
    {
        config.Value = value;
        config.IsSet = true;
    }

    private static void SetConditionally<T>(this TomlConfigEntry<T> config, T value)
    {
        if (value is not null)
        {
            config.Value = value;
            config.IsSet = true;
        }
    }
}
