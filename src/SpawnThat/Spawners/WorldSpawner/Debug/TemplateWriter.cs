﻿using System;
using System.Linq;
using SpawnThat.Core.Toml;
using SpawnThat.Integrations.CLLC.Conditions;
using SpawnThat.Integrations.CLLC.Modifiers;
using SpawnThat.Integrations.EpicLoot.Conditions;
using SpawnThat.Integrations.MobAi.Modifiers;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Utilities.Extensions;
using SpawnThat.Spawners.WorldSpawner.Managers;
using SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;
using SpawnThat.Options.PositionConditions;
using SpawnThat.Configuration;

namespace SpawnThat.Spawners.WorldSpawner.Debug;
internal static class TemplateWriter
{
    public static void WriteToDiskAsToml()
    {
        var tomlFile = PrepareTomlFile();

        TomlWriter.WriteToDisk(tomlFile, new()
        {
            FileName = "spawn_that.world_spawners_loaded_configs.cfg",
            FileDescription = "loaded world spawner configs",
            AddComments = ConfigurationManager.GeneralConfig?.WorldSpawnerAddCommentsToFile.Value ?? false,
            Header =
                $"# This file was auto-generated by Spawn That {SpawnThatPlugin.Version} at {DateTimeOffset.UtcNow.ToString("u")}, with Valheim '{Version.m_major}.{Version.m_minor}.{Version.m_patch}'.\n" +
                $"# The entries listed here were generated from the internally loaded world spawner configurations.\n" +
                $"# This is intended to reveal the state of Spawn That, after loading configs from all sources, and before applying them to in-game spawners.\n" +
                $"# This file is not scanned by Spawn That, and any changes done will therefore have no effect. Copy sections into a world spawner config in the configs folder, if you want to change things.\n" +
                $"# Be aware, for entries added by mods using the Spawn That API, it might not be showing the full truth but an approximation, since the API can do a lot more than the config file allows.\n"
        });
    }

    internal static SpawnSystemConfigurationFile PrepareTomlFile()
    {
        var spawnerTemplates = WorldSpawnTemplateManager.GetTemplates();

        var tomlFile = new SpawnSystemConfigurationFile();

        foreach (var spawnerTemplate in spawnerTemplates)
        {
            var config = tomlFile
                .GetGenericSubsection("WorldSpawner")
                .GetGenericSubsection(spawnerTemplate.id.ToString());

            ConfigureSpawnerConfig(config, spawnerTemplate.template);
        }

        return tomlFile;

        static void ConfigureSpawnerConfig(SpawnConfiguration config, WorldSpawnTemplate template)
        {
            config.Name.SetConditionally(template.TemplateName);
            config.Enabled.SetConditionally(template.Enabled);
            config.TemplateEnabled.SetConditionally(template.TemplateEnabled);

            // Default Settings
            config.PrefabName.SetConditionally(template.PrefabName);
            config.Biomes.SetConditionally(template.BiomeMask?.Split());
            config.HuntPlayer.SetConditionally(template.ModifierHuntPlayer);
            config.MaxSpawned.SetConditionally(template.MaxSpawned);
            config.SpawnInterval.SetConditionally((float?)template.SpawnInterval?.TotalSeconds);
            config.SpawnChance.SetConditionally(template.SpawnChance);
            config.LevelMin.SetConditionally(template.MinLevel);
            config.LevelMax.SetConditionally(template.MaxLevel);
            config.LevelUpMinCenterDistance.SetConditionally(template.DistanceToCenterForLevelUp);
            config.SpawnDistance.SetConditionally(template.MinDistanceToOther);
            config.SpawnRadiusMin.SetConditionally(template.SpawnAtDistanceToPlayerMin);
            config.SpawnRadiusMax.SetConditionally(template.SpawnAtDistanceToPlayerMax);
            config.RequiredGlobalKey.SetConditionally(template.ConditionRequiredGlobalKey);
            config.RequiredEnvironments.SetConditionally(template.ConditionEnvironments);
            config.GroupSizeMin.SetConditionally(template.PackSizeMin);
            config.GroupSizeMax.SetConditionally(template.PackSizeMax);
            config.GroupRadius.SetConditionally(template.PackSpawnCircleRadius);
            config.GroundOffset.SetConditionally(template.SpawnAtDistanceToGround);
            config.SpawnDuringDay.SetConditionally(template.ConditionAllowDuringDay);
            config.SpawnDuringNight.SetConditionally(template.ConditionAllowDuringNight);
            config.ConditionAltitudeMin.SetConditionally(template.ConditionMinAltitude);
            config.ConditionAltitudeMax.SetConditionally(template.ConditionMaxAltitude);
            config.ConditionTiltMin.SetConditionally(template.ConditionMinTilt);
            config.ConditionTiltMax.SetConditionally(template.ConditionMaxTilt);
            config.SpawnInForest.SetConditionally(template.ConditionAllowInForest);
            config.SpawnOutsideForest.SetConditionally(template.ConditionAllowOutsideForest);
            config.OceanDepthMin.SetConditionally(template.ConditionMinOceanDepth);
            config.OceanDepthMax.SetConditionally(template.ConditionMaxOceanDepth);

            // Conditions
            foreach (var condition in template.SpawnConditions)
            {
                switch (condition)
                {
                    case ConditionBiome biome:
                        config.Biomes.Set(biome.BiomeMask.Split());
                        break;
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
                    case ConditionGlobalKeysRequiredMissing globalKeysRequiredMissing:
                        config.RequiredNotGlobalKey.Set(globalKeysRequiredMissing.RequiredMissing?.ToList());
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

            foreach (var positionCondition in template.SpawnPositionConditions)
            {
                switch (positionCondition)
                {
                    case PositionConditionLocation location:
                        config.ConditionLocation.Set(location.LocationNames?.ToList());
                        break;
                }
            }

            // Modifiers
            foreach (var modifier in template.SpawnModifiers)
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
                    case ModifierSetTemplateId templateId:
                        config.TemplateId.Set(templateId.TemplateId);
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
