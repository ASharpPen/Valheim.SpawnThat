using System;
using System.Linq;
using SpawnThat.Core;
using SpawnThat.Utilities;
using SpawnThat.Options.Modifiers;
using SpawnThat.Integrations;
using SpawnThat.Core.Toml;
using BepInEx;
using System.IO;
using SpawnThat.Spawners.LocalSpawner.Configuration.BepInEx;

namespace SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;

internal static class SpawnSystemConfigApplier
{
    internal static void ApplyBepInExConfigs(ISpawnerConfigurationCollection spawnerConfigs)
    {
#if DEBUG
        Log.LogTrace("SpawnSystemConfigApplier.ApplyBepInExConfigs");
#endif
        var spawnSystemConfigs = SpawnSystemConfigurationManager
            .SpawnSystemConfig?
            .Subsections? //[*]
            .Values?
            .FirstOrDefault()? //SpawnSystem config is only expected to have a single first layer, namely "WorldSpawner", so we just grab the first entry.
            .Subsections?//[WorldSpawner.*]
            .Values;

        if ((spawnSystemConfigs?.Count ?? 0) == 0)
        {
            return;
        }

        var configs = spawnSystemConfigs
            .OrderBy(x => x.Index)
            .Where(x => x.TemplateEnabled?.Value ?? x.TemplateEnabled.DefaultValue.Value);

        foreach (var spawnConfig in configs)
        {
            if (spawnConfig.PrefabName.IsSet &&
                string.IsNullOrWhiteSpace(spawnConfig.PrefabName?.Value))
            {
                Log.LogWarning($"PrefabName of world spawner config {spawnConfig.SectionPath} is empty. Skipping config.");
                continue;
            }

            if (spawnConfig.Index < 0)
            {
                Log.LogWarning($"ID of world spawner config {spawnConfig.SectionPath} is less than 0. Skipping config.");
                continue;
            }

            var builder = spawnerConfigs.ConfigureWorldSpawner((uint)spawnConfig.Index);


            ApplyConfigToBuilder(spawnConfig, builder);
        }
    }

    private static void ApplyConfigToBuilder(SpawnConfiguration config, IWorldSpawnBuilder builder)
    {
        // Default
        config.PrefabName.SetIfHasValue(builder.SetPrefabName);
        config.Name.SetIfLoaded(builder.SetTemplateName);
        config.RequiredGlobalKey.SetIfLoaded(builder.SetConditionRequiredGlobalKey);
        config.RequiredEnvironments.SetIfLoaded(builder.SetConditionEnvironments);
        config.Enabled.SetValueOrDefaultIfLoaded(builder.SetEnabled);
        config.TemplateEnabled.SetValueOrDefaultIfLoaded(builder.SetTemplateEnabled);
        config.Biomes.SetIfLoaded(builder.SetConditionBiomes);
        config.HuntPlayer.SetIfLoaded(builder.SetModifierHuntPlayer);
        config.MaxSpawned.SetIfLoaded(x => builder.SetMaxSpawned((uint?)x));
        config.SpawnInterval.SetIfLoaded(x => x is null
            ? builder.SetSpawnInterval(null)
            : builder.SetSpawnInterval(TimeSpan.FromSeconds(x.Value)));
        config.SpawnChance.SetIfLoaded(builder.SetSpawnChance);
        config.LevelMin.SetIfLoaded(x => builder.SetMinLevel((uint?)x));
        config.LevelMax.SetIfLoaded(x => builder.SetMaxLevel((uint?)x));
        config.LevelUpChance.SetIfLoaded(builder.SetLevelUpChance);
        config.SpawnDistance.SetIfLoaded(builder.SetMinDistanceToOther);
        config.SpawnRadiusMin.SetIfLoaded(x => builder.SetSpawnAtDistanceToPlayerMin(x));
        config.SpawnRadiusMax.SetIfLoaded(x => builder.SetSpawnAtDistanceToPlayerMax(x));
        config.GroupSizeMin.SetIfLoaded(x => builder.SetPackSizeMin((uint?)x));
        config.GroupSizeMax.SetIfLoaded(x => builder.SetPackSizeMax((uint?)x));
        config.GroupRadius.SetIfLoaded(builder.SetPackSpawnCircleRadius);
        config.SpawnDuringDay.SetIfLoaded(builder.SetSpawnDuringDay);
        config.SpawnDuringNight.SetIfLoaded(builder.SetSpawnDuringNight);
        config.ConditionAltitudeMin.SetIfLoaded(x => builder.SetConditionAltitudeMin(x));
        config.ConditionAltitudeMax.SetIfLoaded(x => builder.SetConditionAltitudeMax(x));
        config.ConditionTiltMin.SetIfLoaded(x => builder.SetConditionTiltMin(x));
        config.ConditionTiltMax.SetIfLoaded(x => builder.SetConditionTiltMax(x));
        config.SpawnInForest.SetIfLoaded(builder.SetSpawnInForest);
        config.SpawnOutsideForest.SetIfLoaded(builder.SetSpawnOutsideForest);
        config.OceanDepthMin.SetIfLoaded(x => builder.SetConditionOceanDepthMin(x));
        config.OceanDepthMax.SetIfLoaded(x => builder.SetConditionOceanDepthMax(x));
        config.LevelUpMinCenterDistance.SetIfLoaded(builder.SetDistanceToCenterForLevelUp);
        config.GroundOffset.SetIfLoaded(builder.SetSpawnAtDistanceToGround);

        // Conditions
        var playerConditionsDistance = config.DistanceToTriggerPlayerConditions.IsSet && config.DistanceToTriggerPlayerConditions.Value is not null
            ? (int)config.DistanceToTriggerPlayerConditions.Value 
            : (int)config.DistanceToTriggerPlayerConditions.DefaultValue;

        config.ConditionLocation.SetIfLoaded(builder.SetConditionLocation);
        config.RequiredNotGlobalKey.SetIfLoaded(builder.SetGlobalKeysRequiredMissing);
        config.ConditionNearbyPlayersCarryValue.SetValueOrDefaultIfLoaded(x => builder.SetConditionNearbyPlayersCarryValue(playerConditionsDistance, x));
        config.ConditionNearbyPlayersNoiseThreshold.SetValueOrDefaultIfLoaded(x => builder.SetConditionNearbyPlayersNoise(playerConditionsDistance, x));
        config.ConditionAreaSpawnChance.SetValueOrDefaultIfLoaded(builder.SetConditionAreaSpawnChance);

        float? distanceToCenterMin = config.ConditionDistanceToCenterMin.IsSet ? config.ConditionDistanceToCenterMin.Value : null;
        float? distanceToCenterMax = config.ConditionDistanceToCenterMax.IsSet ? config.ConditionDistanceToCenterMax.Value : null;

        if (config.ConditionDistanceToCenterMin.IsSet || config.ConditionDistanceToCenterMax.IsSet)
        {
            builder.SetConditionDistanceToCenter(distanceToCenterMin, distanceToCenterMax);
        }

        int? worldAgeMin = config.ConditionWorldAgeDaysMin.IsSet ? (int?)config.ConditionWorldAgeDaysMin.Value : null;
        int? worldAgeMax = config.ConditionWorldAgeDaysMax.IsSet ? (int?)config.ConditionWorldAgeDaysMax.Value : null;

        if (config.ConditionWorldAgeDaysMin.IsSet || config.ConditionWorldAgeDaysMax.IsSet)
        {
            builder.SetConditionWorldAge(worldAgeMin, worldAgeMax);
        }

        config.ConditionNearbyPlayerCarriesItem.SetIfLoaded(x => builder.SetConditionNearbyPlayersCarryItem(playerConditionsDistance, x));
        config.ConditionNearbyPlayersStatus.SetIfLoaded(x => builder.SetConditionNearbyPlayersStatus(playerConditionsDistance, x));
        config.ConditionAreaIds.SetIfLoaded(builder.SetConditionAreaIds);

        // Conditions - Integrations
        TomlConfig cfg;

        {
            if (IntegrationManager.InstalledCLLC)
            {
                if (config.TryGet(SpawnSystemConfigCLLC.ModName, out cfg) &&
                    cfg is SpawnSystemConfigCLLC cllcConfig)
                {
                    if (cllcConfig.ConditionWorldLevelMin.IsSet || cllcConfig.ConditionWorldLevelMax.IsSet)
                    {
                        builder.SetCllcConditionWorldLevel(
                            cllcConfig.ConditionWorldLevelMin.IsSet ? cllcConfig.ConditionWorldLevelMin.Value : null,
                            cllcConfig.ConditionWorldLevelMax.IsSet ? cllcConfig.ConditionWorldLevelMax.Value : null
                        );
                    }
                }
            }

            if (IntegrationManager.InstalledEpicLoot)
            {
                if (config.TryGet(SpawnSystemConfigEpicLoot.ModName, out cfg) &&
                    cfg is SpawnSystemConfigEpicLoot elConfig)
                {
                    int distance = config.DistanceToTriggerPlayerConditions.IsSet && config.DistanceToTriggerPlayerConditions.Value is not null
                        ? (int)config.DistanceToTriggerPlayerConditions.Value
                        : (int)config.DistanceToTriggerPlayerConditions.DefaultValue;

                    elConfig.ConditionNearbyPlayerCarryLegendaryItem.SetIfLoaded(x => builder.SetEpicLootConditionNearbyPlayerCarryLegendaryItem(distance, x));
                    elConfig.ConditionNearbyPlayerCarryItemWithRarity.SetIfLoaded(x => builder.SetEpicLootConditionNearbyPlayersCarryItemWithRarity(distance, x));
                }
            }
        }

        // Position conditions
        config.ConditionLocation.SetIfLoaded(builder.SetPositionConditionLocation);

        // Modifiers
        config.SetFaction.SetIfLoaded(builder.SetModifierFaction);
        config.SetRelentless.SetValueOrDefaultIfLoaded(builder.SetModifierRelentless);
        config.SetTryDespawnOnConditionsInvalid.SetValueOrDefaultIfLoaded(
            x => x
            ? builder.SetModifierDespawnOnConditionsInvalid(
                config.SpawnDuringDay.IsSet ? config.SpawnDuringDay.Value : null,
                config.SpawnDuringNight.IsSet ? config.SpawnDuringNight.Value : null,
                config.RequiredEnvironments.IsSet ? config.RequiredEnvironments.Value : null)
            : builder.SetModifierDespawnOnConditionsInvalid(null, null, null)
        );

        config.SetTryDespawnOnAlert.SetValueOrDefaultIfLoaded(builder.SetModifierDespawnOnAlert);
        config.TemplateId.SetIfLoaded(builder.SetModifierTemplateId);
        config.SetTamed.SetValueOrDefaultIfLoaded(builder.SetModifierTamed);
        config.SetTamedCommandable.SetValueOrDefaultIfLoaded(builder.SetModifierTamedCommandable);

        // Modifiers - Integrations
        {
            if (IntegrationManager.InstalledCLLC)
            {
                if (config.TryGet(SpawnSystemConfigCLLC.ModName, out cfg) &&
                    cfg is SpawnSystemConfigCLLC cllcConfig)
                {
                    cllcConfig.SetBossAffix.SetIfLoaded(builder.SetCllcModifierBossAffix);
                    cllcConfig.SetExtraEffect.SetIfLoaded(builder.SetCllcModifierExtraEffect);
                    cllcConfig.SetInfusion.SetIfLoaded(builder.SetCllcModifierInfusion);
                    cllcConfig.UseDefaultLevels.SetIfLoaded(
                        x => x ?? cllcConfig.UseDefaultLevels.DefaultValue.Value
                        ? builder.SetModifier(
                            new ModifierDefaultRollLevel(
                                config.LevelMin.IsSet
                                    ? config.LevelMin.Value ?? config.LevelMin.DefaultValue.Value
                                    : config.LevelMin.DefaultValue.Value,
                                config.LevelMax.IsSet
                                    ? config.LevelMax.Value ?? config.LevelMax.DefaultValue.Value
                                    : config.LevelMax.DefaultValue.Value,
                                0,
                                10f
                            ))
                        : builder.SetModifier(new ModifierDefaultRollLevel(-1, -1, 0, -1))
                        );
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

    private static void SetIfLoaded<T>(this TomlConfigEntry<T> value, Func<T, IWorldSpawnBuilder> apply)
    {
        if (value is not null &&
            value.IsSet)
        {
            apply(value.Value);
        }
    }

    private static void SetIfHasValue(this TomlConfigEntry<string> value, Func<string, IWorldSpawnBuilder> apply)
    {
        if (value is not null &&
            value.IsSet && 
            value.Value.IsNotEmpty())
        {
            apply(value.Value);
        }
    }

    private static void SetValueOrDefaultIfLoaded<T>(this ITomlConfigEntry<T?> value, Func<T, IWorldSpawnBuilder> apply)
    where T : struct
    {
        if (value is not null &&
            value.IsSet)
        {
            apply(value.Value ?? value.DefaultValue.Value);
        }
    }
}
