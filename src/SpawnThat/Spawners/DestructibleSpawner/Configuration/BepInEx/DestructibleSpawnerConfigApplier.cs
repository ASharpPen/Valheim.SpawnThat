using System;
using System.Collections.Generic;
using SpawnThat.Core.Configuration;
using SpawnThat.Integrations;
using SpawnThat.Options.Modifiers;
using SpawnThat.Utilities;
using SpawnThat.Options.Conditions;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration.BepInEx;

internal static class DestructibleSpawnerConfigApplier
{
    public static void ApplyBepInExConfigs(ISpawnerConfigurationCollection configs)
    {
        if (DestructibleSpawnerBepInExCfgManager.Config is null)
        {
            return;
        }

        foreach (var spawnerConfig in DestructibleSpawnerBepInExCfgManager.Config.Subsections)
        {
            var config = spawnerConfig.Value;
            
            // Skip if all identifiers are empty.
            if (string.IsNullOrWhiteSpace(config.IdentifyByName?.Value) &&
                string.IsNullOrWhiteSpace(config.IdentifyByBiome?.Value) &&
                string.IsNullOrWhiteSpace(config.IdentifyByLocation?.Value) &&
                string.IsNullOrWhiteSpace(config.IdentifyByRoom?.Value))
            {
                continue;
            }

            var builder = configs.ConfigureDestructibleSpawner();

            ConfigureSpawner(config, builder);

            foreach (var spawnConfig in config.Subsections)
            {
                ConfigureSpawn(config, spawnConfig.Value, builder.GetSpawnBuilder((uint)spawnConfig.Value.Index));
            }
        }
    }

    private static void ConfigureSpawner(DestructibleSpawnerConfig config, IDestructibleSpawnerBuilder builder)
    {
        // Set identifiers
        if (!string.IsNullOrWhiteSpace(config.IdentifyByName.Value))
        {
            builder.SetIdentifierName(config.IdentifyByName.Value);
        }

        if (!string.IsNullOrWhiteSpace(config.IdentifyByBiome.Value))
        {
            var biomes = HeightmapUtils.ParseBiomes(config.IdentifyByBiome.Value.SplitByComma());
            builder.SetIdentifierBiome(biomes);
        }

        if (!string.IsNullOrWhiteSpace(config.IdentifyByLocation.Value))
        {
            builder.SetIdentifierLocation(config.IdentifyByLocation.Value.SplitByComma());
        }

        if (!string.IsNullOrWhiteSpace(config.IdentifyByRoom.Value))
        {
            builder.SetIdentifierRoom(config.IdentifyByRoom.Value.SplitByComma());
        }

        // Set default settings
        builder.SetLevelUpChance(config.LevelUpChance.Value);
        builder.SetSpawnInterval(TimeSpan.FromSeconds(config.SpawnInterval.Value));
        builder.SetPatrol(config.SetPatrol.Value);
        builder.SetConditionPlayerWithinDistance(config.ConditionPlayerWithinDistance.Value);
        builder.SetConditionMaxCloseCreatures(config.ConditionMaxCloseCreatures.Value);
        builder.SetConditionMaxCreatures(config.ConditionMaxCreatures.Value);
        builder.SetDistanceConsideredClose(config.DistanceConsideredClose.Value);
        builder.SetDistanceConsideredFar(config.DistanceConsideredFar.Value);
        builder.SetOnGroundOnly(config.OnGroundOnly.Value);

        // Set custom settings
    }

    private static void ConfigureSpawn(DestructibleSpawnerConfig spawnerConfig, DestructibleSpawnConfig config, IDestructibleSpawnBuilder builder)
    {
        if (config.TemplateEnabled.Value == false)
        {
            return;
        }

        builder.SetEnabled(config.Enabled.Value);
        builder.SetPrefabName(config.PrefabName.Value);
        builder.SetSpawnWeight(config.SpawnWeight.Value);
        builder.SetLevelMin(config.LevelMin.Value);
        builder.SetLevelMax(config.LevelMax.Value);

        // Conditions
        if (config.ConditionDistanceToCenterMin.Value > 0 || 
            config.ConditionDistanceToCenterMax.Value > 0)
        {
            builder.SetConditionDistanceToCenter(config.ConditionDistanceToCenterMin.Value, config.ConditionDistanceToCenterMax.Value);
        }

        if (config.ConditionWorldAgeDaysMin.Value > 0 ||
            config.ConditionWorldAgeDaysMax.Value > 0)
        {
            builder.SetConditionWorldAge(config.ConditionWorldAgeDaysMin.Value, config.ConditionWorldAgeDaysMax.Value);
        }

        if (config.DistanceToTriggerPlayerConditions.Value > 0)
        {
            var dist = (int)config.DistanceToTriggerPlayerConditions.Value;

            config.ConditionNearbyPlayersCarryValue.SetIfGreaterThanZero(x => builder.SetConditionNearbyPlayersCarryValue(dist, x));
            config.ConditionNearbyPlayerCarriesItem.SetIfHasValue(x => builder.SetConditionNearbyPlayersCarryItem(dist, x.SplitByComma()));
            config.ConditionNearbyPlayersNoiseThreshold.SetIfGreaterThanZero(x => builder.SetConditionNearbyPlayersNoise(dist, x));
            config.ConditionNearbyPlayersStatus.SetIfHasValue(x => builder.SetConditionNearbyPlayersStatus(dist, x.SplitByComma()));
        }

        config.ConditionAreaSpawnChance.SetIfNotEqual(100, x => builder.SetConditionAreaSpawnChance(x));
        config.ConditionLocation.SetIfHasValue(x => builder.SetConditionLocation(x.SplitByComma()));
        config.ConditionAreaIds.SetIfHasValue(x => builder.SetConditionAreaIds(x.SplitByComma().ConvertAll(x => int.Parse(x))));

        config.ConditionBiome.SetIfHasValue(x => builder.SetConditionBiome(x.SplitByComma()));
        config.ConditionAllOfGlobalKeys.SetIfHasValue(x => builder.SetConditionAllOfGlobalKeys(x.SplitByComma()));
        config.ConditionAnyOfGlobalKeys.SetIfHasValue(x => builder.SetConditionAnyOfGlobalKeys(x.SplitByComma()));
        config.ConditionNoneOfGlobalKeys.SetIfHasValue(x => builder.SetConditionNoneOfGlobalkeys(x.SplitByComma()));
        config.ConditionEnvironment.SetIfHasValue(x => builder.SetConditionEnvironment(x.SplitByComma()));
        config.ConditionDaytime.SetIfNotEqual(Utilities.Enums.Daytime.All, x => builder.SetConditionDaytime(x));

        builder.SetConditionAltitude(config.ConditionAltitudeMin.Value, config.ConditionAltitudeMax.Value);
        builder.SetCondition(new ConditionForest(config.ConditionInForest.Value, config.ConditionOutsideForest.Value));

        if (config.ConditionOceanDepthMin.Value != config.ConditionOceanDepthMax.Value)
        {
            builder.SetConditionOceanDepth(config.ConditionOceanDepthMin.Value, config.ConditionOceanDepthMax.Value);
        }

        // Conditions - Integrations
        Config cfg;
        {
            if (IntegrationManager.InstalledCLLC)
            {
                if (config.TryGet(DestructibleSpawnConfigCLLC.ModName, out cfg) &&
                    cfg is DestructibleSpawnConfigCLLC cllcConfig)
                {
                    if (cllcConfig.ConditionWorldLevelMin.Value > 0 || 
                        cllcConfig.ConditionWorldLevelMax.Value > 0)
                    {
                        builder.SetCllcConditionWorldLevel(cllcConfig.ConditionWorldLevelMin.Value, cllcConfig.ConditionWorldLevelMax.Value);
                    }
                }
            }

            if (IntegrationManager.InstalledEpicLoot)
            {
                if (config.TryGet(DestructibleSpawnConfigEpicLoot.ModName, out cfg) &&
                    cfg is DestructibleSpawnConfigEpicLoot elConfig)
                {
                    var dist = (int)config.DistanceToTriggerPlayerConditions.Value;

                    if (dist > 0)
                    {
                        elConfig.ConditionNearbyPlayerCarryItemWithRarity.SetIfHasValue(x => builder.SetEpicLootConditionNearbyPlayersCarryItemWithRarity(dist, x.SplitByComma()));
                        elConfig.ConditionNearbyPlayerCarryLegendaryItem.SetIfHasValue(x => builder.SetEpicLootConditionNearbyPlayerCarryLegendaryItem(dist, x.SplitByComma()));
                    }
                }
            }
        }

        // Position conditions

        // Modifiers
        config.SetFaction.SetIfHasValue(x => builder.SetModifierFaction(x));

        builder.SetModifierRelentless(config.SetRelentless.Value);
        builder.SetModifierDespawnOnAlert(config.SetTryDespawnOnAlert.Value);
        builder.SetModifierTamed(config.SetTamed.Value);
        builder.SetModifierTamedCommandable(config.SetTamedCommandable.Value);
        builder.SetModifierHuntPlayer(config.SetHuntPlayer.Value);

        // Modifiers - Integrations
        {
            if (IntegrationManager.InstalledCLLC)
            {
                if (config.TryGet(DestructibleSpawnConfigCLLC.ModName, out cfg) &&
                    cfg is DestructibleSpawnConfigCLLC cllcConfig)
                {
                    cllcConfig.SetBossAffix.SetIfHasValue(x => builder.SetCllcModifierBossAffix(x));
                    cllcConfig.SetExtraEffect.SetIfHasValue(x => builder.SetCllcModifierExtraEffect(x));
                    cllcConfig.SetInfusion.SetIfHasValue(x => builder.SetCllcModifierInfusion(x));

                    if (cllcConfig.UseDefaultLevels.Value)
                    {
                        builder.SetModifier(new ModifierDefaultRollLevel(config.LevelMin.Value, config.LevelMax.Value, 0, spawnerConfig.LevelUpChance.Value));
                    }
                }
            }

            if (IntegrationManager.InstalledMobAI)
            {
                if (config.TryGet(DestructibleSpawnConfigMobAI.ModName, out cfg) &&
                    cfg is DestructibleSpawnConfigMobAI mobAIConfig)
                {
                    mobAIConfig.SetAI.SetIfHasValue(x => builder.SetMobAiModifier(x, mobAIConfig.AIConfigFile.Value));
                }
            }
        }
    }

    private static void SetIfHasValue(this ConfigurationEntry<string> value, Func<string, IDestructibleSpawnBuilder> apply)
    {
        if (value is not null &&
            value.Value.IsNotEmpty())
        {
            apply(value.Value);
        }
    }

    private static void SetIfHasValue(this ConfigurationEntry<string> value, Func<List<string>, IDestructibleSpawnBuilder> apply)
    {
        if (value is not null &&
            value.Value.IsNotEmpty())
        {
            apply(value.Value.SplitByComma());
        }
    }

    private static void SetIfGreaterThanZero(this ConfigurationEntry<int> value, Func<int, IDestructibleSpawnBuilder> apply)
    {
        if (value is not null &&
            value.Value > 0)
        {
            apply(value.Value);
        }
    }

    private static void SetIfNotEqual<T>(this ConfigurationEntry<T> value, T compare, Func<T, IDestructibleSpawnBuilder> apply)
    {
        if (value is not null &&
            value.Value.Equals(compare))
        {
            apply(value.Value);
        }
    }

    private static void SetIfGreaterThanZero(this ConfigurationEntry<float> value, Func<float, IDestructibleSpawnBuilder> apply)
    {
        if (value is not null &&
            value.Value > 0)
        {
            apply(value.Value);
        }
    }
}
