using System;
using System.Collections.Generic;
using SpawnThat.Integrations;
using SpawnThat.Options.Modifiers;
using SpawnThat.Utilities;
using SpawnThat.Core;
using SpawnThat.Core.Toml;
using SpawnThat.Spawners.WorldSpawner;
using BepInEx;
using System.IO;
using SpawnThat.Spawners.LocalSpawner.Configuration.BepInEx;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Configuration.BepInEx;

internal static class SpawnAreaSpawnerConfigApplier
{
    public static void ApplyBepInExConfigs(ISpawnerConfigurationCollection configs)
    {
        if (SpawnAreaSpawnerTomlCfgManager.Config is null)
        {
            return;
        }

        foreach (var spawnerConfig in SpawnAreaSpawnerTomlCfgManager.Config.Subsections)
        {
            var config = spawnerConfig.Value;
            
            // Skip if all identifiers are empty.
            if (!config.IdentifyByName.IsSet &&
                !config.IdentifyByBiome.IsSet &&
                !config.IdentifyByLocation.IsSet &&
                !config.IdentifyByRoom.IsSet)
            {
                Log.LogWarning($"[SpawnArea Spawner] Ignoring config '{config.SectionPath}' due to having no identifiers listed. At least one identifier must be specified, for config to be valid.");
                continue;
            }
            if ((config.IdentifyByName.Value?.Count ?? 0) == 0 &&
                (config.IdentifyByBiome.Value?.Count ?? 0) == 0 &&
                (config.IdentifyByLocation.Value?.Count ?? 0) == 0 &&
                (config.IdentifyByRoom.Value?.Count ?? 0) == 0)
            {
                Log.LogWarning($"[SpawnArea Spawner] Ignoring config '{config.SectionPath}' due to all identifiers being empty. At least one identifier must have a value, for config to be valid.");
                continue;
            }

            var builder = configs.ConfigureSpawnAreaSpawner();

            ConfigureSpawner(config, builder);

            foreach (var spawnConfig in config.Subsections)
            {
                ConfigureSpawn(config, spawnConfig.Value, builder.GetSpawnBuilder((uint)spawnConfig.Value.Index));
            }
        }
    }

    private static void ConfigureSpawner(SpawnAreaSpawnerConfig config, ISpawnAreaSpawnerBuilder builder)
    {
        builder.SetTemplateName(config.SectionPath);

        // Set identifiers
        config.IdentifyByName.SetIfHasValue(builder.SetIdentifierName);
        config.IdentifyByBiome.SetIfHasValue(builder.SetIdentifierBiome);
        config.IdentifyByLocation.SetIfHasValue(builder.SetIdentifierLocation);
        config.IdentifyByRoom.SetIfHasValue(builder.SetIdentifierRoom);

        // Set default settings
        config.LevelUpChance.SetIfLoaded(x => builder.SetLevelUpChance(x));
        config.SpawnInterval.SetIfLoaded(x => x is null 
            ? builder.SetSpawnInterval(null)
            : builder.SetSpawnInterval(TimeSpan.FromSeconds(x.Value)));
        config.SetPatrol.SetIfLoaded(x => builder.SetPatrol(x));
        config.ConditionPlayerWithinDistance.SetIfLoaded(x => builder.SetConditionPlayerWithinDistance(x));
        config.SpawnRadius.SetIfLoaded(x => builder.SetSpawnRadius(x));
        config.ConditionMaxCloseCreatures.SetIfLoaded(x => builder.SetConditionMaxCloseCreatures(x));
        config.ConditionMaxCreatures.SetIfLoaded(x => builder.SetConditionMaxCreatures(x));
        config.DistanceConsideredClose.SetIfLoaded(x => builder.SetDistanceConsideredClose(x));
        config.DistanceConsideredFar.SetIfLoaded(x => builder.SetDistanceConsideredFar(x));
        config.OnGroundOnly.SetIfLoaded(x => builder.SetOnGroundOnly(x));

        // Set custom settings
        config.RemoveNotConfiguredSpawns.SetValueOrDefaultIfLoaded(x => builder.SetRemoveNotConfiguredSpawns(x.Value));
    }

    private static void ConfigureSpawn(SpawnAreaSpawnerConfig spawnerConfig, SpawnAreaSpawnConfig config, ISpawnAreaSpawnBuilder builder)
    {
        if (config.TemplateEnabled.Value == false)
        {
            return;
        }

        config.Enabled.SetValueOrDefaultIfLoaded(x => builder.SetEnabled(x.Value));
        config.PrefabName.SetIfHasValue(builder.SetPrefabName);
        config.SpawnWeight.SetIfLoaded(x => builder.SetSpawnWeight(x));
        config.LevelMin.SetIfLoaded(x => builder.SetLevelMin(x));
        config.LevelMax.SetIfLoaded(x => builder.SetLevelMax(x));

        // Conditions
        if (config.ConditionDistanceToCenterMin.IsSet ||
            config.ConditionDistanceToCenterMax.IsSet)
        {
            builder.SetConditionDistanceToCenter(
                config.ConditionDistanceToCenterMin.IsSet ? config.ConditionDistanceToCenterMin.Value : null,
                config.ConditionDistanceToCenterMax.IsSet ? config.ConditionDistanceToCenterMax.Value : null);
        }

        if (config.ConditionWorldAgeDaysMin.IsSet ||
            config.ConditionWorldAgeDaysMax.IsSet)
        {
            builder.SetConditionWorldAge(
                config.ConditionWorldAgeDaysMin.IsSet ? config.ConditionWorldAgeDaysMin.Value : null,
                config.ConditionWorldAgeDaysMax.IsSet ? config.ConditionWorldAgeDaysMax.Value : null);
        }

        int playerConditionsDistance = config.DistanceToTriggerPlayerConditions.IsSet && config.DistanceToTriggerPlayerConditions.Value is not null
            ? (int)config.DistanceToTriggerPlayerConditions.Value
            : (int)config.DistanceToTriggerPlayerConditions.DefaultValue;

        config.ConditionNearbyPlayersCarryValue.SetValueOrDefaultIfLoaded(x => builder.SetConditionNearbyPlayersCarryValue(playerConditionsDistance, x.Value));
        config.ConditionNearbyPlayerCarriesItem.SetIfLoaded(x => builder.SetConditionNearbyPlayersCarryItem(playerConditionsDistance, x));
        config.ConditionNearbyPlayersNoiseThreshold.SetValueOrDefaultIfLoaded(x => builder.SetConditionNearbyPlayersNoise(playerConditionsDistance, x.Value));
        config.ConditionNearbyPlayersStatus.SetIfLoaded(x => builder.SetConditionNearbyPlayersStatus(playerConditionsDistance, x));

        config.ConditionAreaSpawnChance.SetIfLoaded(x => builder.SetConditionAreaSpawnChance(x ?? config.ConditionAreaSpawnChance.DefaultValue.Value));
        config.ConditionLocation.SetIfLoaded(builder.SetConditionLocation);
        config.ConditionAreaIds.SetIfLoaded(builder.SetConditionAreaIds);
        config.ConditionBiome.SetIfLoaded(builder.SetConditionBiome);
        config.ConditionAllOfGlobalKeys.SetIfLoaded(builder.SetConditionAllOfGlobalKeys);
        config.ConditionAnyOfGlobalKeys.SetIfLoaded(builder.SetConditionAnyOfGlobalKeys);
        config.ConditionNoneOfGlobalKeys.SetIfLoaded(builder.SetConditionNoneOfGlobalkeys);
        config.ConditionEnvironment.SetIfLoaded(x => builder.SetConditionEnvironment(x));
        config.ConditionDaytime.SetIfLoaded(builder.SetConditionDaytime);
        config.ConditionForestState.SetValueOrDefaultIfLoaded(x => builder.SetConditionForest(x.Value));

        if (config.ConditionAltitudeMin.IsSet ||
            config.ConditionAltitudeMax.IsSet)
        {
            builder.SetConditionAltitude(
                config.ConditionAltitudeMin.IsSet ? config.ConditionAltitudeMin.Value : null,
                config.ConditionAltitudeMax.IsSet ? config.ConditionAltitudeMax.Value : null);
        }

        if (config.ConditionOceanDepthMin.IsSet ||
            config.ConditionOceanDepthMax.IsSet)
        {
            builder.SetConditionOceanDepth(
                config.ConditionOceanDepthMin.IsSet ? config.ConditionOceanDepthMin.Value : null,
                config.ConditionOceanDepthMax.IsSet ? config.ConditionOceanDepthMax.Value : null);
        }

        // Conditions - Integrations
        TomlConfig cfg;
        {
            if (IntegrationManager.InstalledCLLC)
            {
                if (config.TryGet(SpawnAreaSpawnConfigCLLC.ModName, out cfg) &&
                    cfg is SpawnAreaSpawnConfigCLLC cllcConfig)
                {
                    if (cllcConfig.ConditionWorldLevelMin.IsSet || 
                        cllcConfig.ConditionWorldLevelMax.IsSet)
                    {
                        builder.SetCllcConditionWorldLevel(
                            cllcConfig.ConditionWorldLevelMin.IsSet ? cllcConfig.ConditionWorldLevelMin.Value : null,
                            cllcConfig.ConditionWorldLevelMax.IsSet ? cllcConfig.ConditionWorldLevelMax.Value : null);
                    }
                }
            }

            if (IntegrationManager.InstalledEpicLoot)
            {
                if (config.TryGet(SpawnAreaSpawnConfigEpicLoot.ModName, out cfg) &&
                    cfg is SpawnAreaSpawnConfigEpicLoot elConfig)
                {
                    var dist = config.DistanceToTriggerPlayerConditions.IsSet && config.DistanceToTriggerPlayerConditions.Value is not null
                        ? (int)config.DistanceToTriggerPlayerConditions.Value
                        : (int)config.DistanceToTriggerPlayerConditions.DefaultValue;

                    elConfig.ConditionNearbyPlayerCarryItemWithRarity.SetIfLoaded(x => builder.SetEpicLootConditionNearbyPlayersCarryItemWithRarity(dist, x));
                    elConfig.ConditionNearbyPlayerCarryLegendaryItem.SetIfHasValue(x => builder.SetEpicLootConditionNearbyPlayerCarryLegendaryItem(dist, x));
                }
            }
        }

        // Position conditions

        if (config.ConditionPositionMustBeNearAllPrefabs.IsSet || config.ConditionPositionMustBeNearAllPrefabsDistance.IsSet)
        {
            builder.ConditionPositionMustBeNearAllPrefabs(
                config.ConditionPositionMustBeNearAllPrefabs.IsSet
                    ? config.ConditionPositionMustBeNearAllPrefabs.Value
                    : config.ConditionPositionMustBeNearAllPrefabs.DefaultValue,
                config.ConditionPositionMustBeNearAllPrefabsDistance.IsSet
                    ? config.ConditionPositionMustBeNearAllPrefabsDistance.Value
                    : config.ConditionPositionMustBeNearAllPrefabsDistance.DefaultValue
                );
        }

        if (config.ConditionPositionMustBeNearPrefabs.IsSet || config.ConditionPositionMustBeNearPrefabsDistance.IsSet)
        {
            builder.ConditionPositionMustBeNearPrefab(
                config.ConditionPositionMustBeNearPrefabs.IsSet
                    ? config.ConditionPositionMustBeNearPrefabs.Value
                    : config.ConditionPositionMustBeNearPrefabs.DefaultValue,
                config.ConditionPositionMustBeNearPrefabsDistance.IsSet
                    ? config.ConditionPositionMustBeNearPrefabsDistance.Value
                    : config.ConditionPositionMustBeNearPrefabsDistance.DefaultValue
                );
        }

        if (config.ConditionPositionMustNotBeNearPrefabs.IsSet || config.ConditionPositionMustNotBeNearPrefabsDistance.IsSet)
        {
            builder.ConditionPositionMustNotBeNearPrefab(
                config.ConditionPositionMustNotBeNearPrefabs.IsSet
                    ? config.ConditionPositionMustNotBeNearPrefabs.Value
                    : config.ConditionPositionMustNotBeNearPrefabs.DefaultValue,
                config.ConditionPositionMustNotBeNearPrefabsDistance.IsSet
                    ? config.ConditionPositionMustNotBeNearPrefabsDistance.Value
                    : config.ConditionPositionMustNotBeNearPrefabsDistance.DefaultValue
                );
        }

        // Modifiers
        config.SetFaction.SetIfLoaded(builder.SetModifierFaction);
        config.SetRelentless.SetValueOrDefaultIfLoaded(x => builder.SetModifierRelentless(x.Value));
        config.SetTryDespawnOnAlert.SetValueOrDefaultIfLoaded(x => builder.SetModifierDespawnOnAlert(x.Value));
        config.SetTamed.SetValueOrDefaultIfLoaded(x => builder.SetModifierTamed(x.Value));
        config.SetTamedCommandable.SetValueOrDefaultIfLoaded(x => builder.SetModifierTamedCommandable(x.Value));
        config.SetHuntPlayer.SetValueOrDefaultIfLoaded(x => builder.SetModifierHuntPlayer(x.Value));

        // Modifiers - Integrations
        {
            if (IntegrationManager.InstalledCLLC)
            {
                if (config.TryGet(SpawnAreaSpawnConfigCLLC.ModName, out cfg) &&
                    cfg is SpawnAreaSpawnConfigCLLC cllcConfig)
                {
                    cllcConfig.SetBossAffix.SetIfLoaded(builder.SetCllcModifierBossAffix);
                    cllcConfig.SetExtraEffect.SetIfLoaded(builder.SetCllcModifierExtraEffect);
                    cllcConfig.SetInfusion.SetIfLoaded(builder.SetCllcModifierInfusion);

                    if (cllcConfig.UseDefaultLevels.IsSet &&
                        (cllcConfig.UseDefaultLevels.Value ?? cllcConfig.UseDefaultLevels.DefaultValue.Value))
                    {
                        builder.SetModifier(
                            new ModifierDefaultRollLevel(
                                config.LevelMin.IsSet 
                                    ? config.LevelMin.Value ?? config.LevelMin.DefaultValue.Value
                                    : config.LevelMin.DefaultValue.Value,
                                config.LevelMax.IsSet 
                                    ? config.LevelMax.Value ?? config.LevelMax.DefaultValue.Value
                                    : config.LevelMax.DefaultValue.Value,
                                0,
                                (double)(spawnerConfig.LevelUpChance.IsSet
                                    ? spawnerConfig.LevelUpChance.Value ?? spawnerConfig.LevelUpChance.DefaultValue
                                    : spawnerConfig.LevelUpChance.DefaultValue)
                                ));
                    }
                    else if (cllcConfig.UseDefaultLevels.IsSet)
                    {
                        builder.SetModifier(new ModifierDefaultRollLevel(-1, -1, 0, 0));
                    }
                }
            }

            if (IntegrationManager.InstalledMobAI)
            {
                if (config.TryGet(CreatureSpawnerConfigMobAI.ModName, out cfg) &&
                    cfg is CreatureSpawnerConfigMobAI mobAIConfig)
                {
                    ApplyMobAI();
                }

                void ApplyMobAI()
                {
                    if (mobAIConfig.SetAI.IsSet)
                    {
                        var ai = mobAIConfig.SetAI.Value;

                        if (string.IsNullOrWhiteSpace(ai))
                        {
                            builder.SetMobAiModifier(null, null);
                        }

                        try
                        {
                            if (!mobAIConfig.AIConfigFile.IsSet ||
                                string.IsNullOrWhiteSpace(mobAIConfig.AIConfigFile.Value))
                            {
                                builder.SetMobAiModifier(ai, "{}");
                            }
                            else if (mobAIConfig.AIConfigFile.IsSet && !string.IsNullOrWhiteSpace(mobAIConfig.AIConfigFile.Value))
                            {
                                if (Paths.ConfigPath is null) // This generally means we are in a test environment.
                                {
                                    return;
                                }

                                string filePath = Path.Combine(Paths.ConfigPath ?? @".\", mobAIConfig.AIConfigFile.Value);

                                if (File.Exists(filePath))
                                {
                                    string content = File.ReadAllText(filePath);

                                    builder.SetMobAiModifier(ai, content);
                                }
                                else
                                {
                                    Log.LogWarning($"Unable to find MobAI json config file at '{filePath}'");
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Log.LogError("Error while attempting to read MobAI config.", e);
                        }
                    }
                }
            }
        }
    }

    private static void SetIfLoaded<T>(this TomlConfigEntry<T> value, Func<T, ISpawnAreaSpawnBuilder> apply)
    {
        if (value is not null &&
            value.IsSet)
        {
            apply(value.Value);
        }
    }

    private static void SetIfLoaded<T>(this TomlConfigEntry<T> value, Func<T, ISpawnAreaSpawnerBuilder> apply)
    {
        if (value is not null &&
            value.IsSet)
        {
            apply(value.Value);
        }
    }

    private static void SetValueOrDefaultIfLoaded<T>(this ITomlConfigEntry<T> value, Func<T, ISpawnAreaSpawnBuilder> apply)
    {
        if (value is not null &&
            value.IsSet)
        {
            apply(value.Value ?? value.DefaultValue);
        }
    }

    private static void SetValueOrDefaultIfLoaded<T>(this ITomlConfigEntry<T> value, Func<T, ISpawnAreaSpawnerBuilder> apply)
    {
        if (value is not null &&
            value.IsSet)
        {
            apply(value.Value ?? value.DefaultValue);
        }
    }

    private static void SetIfHasValue<T>(this TomlConfigEntry<List<T>> value, Func<List<T>, ISpawnAreaSpawnerBuilder> apply)
    {
        if (value is not null &&
            value.IsSet &&
            value.Value?.Count > 0)
        {
            apply(value.Value);
        }
    }

    private static void SetIfHasValue<T>(this TomlConfigEntry<List<T>> value, Func<List<T>, ISpawnAreaSpawnBuilder> apply)
    {
        if (value is not null &&
            value.IsSet &&
            value.Value?.Count > 0)
        {
            apply(value.Value);
        }
    }

    private static void SetIfHasValue(this TomlConfigEntry<string> value, Func<string, ISpawnAreaSpawnBuilder> apply)
    {
        if (value is not null &&
            value.IsSet &&
            value.Value.IsNotEmpty())
        {
            apply(value.Value);
        }
    }
}
