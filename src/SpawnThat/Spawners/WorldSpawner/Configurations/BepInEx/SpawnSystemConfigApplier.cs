using System;
using System.Linq;
using SpawnThat.Core.Configuration;
using SpawnThat.Core;
using SpawnThat.Utilities;
using SpawnThat.Options.Modifiers;
using System.Collections.Generic;
using SpawnThat.Integrations.CLLC.Models;

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
            .Where(x => x.TemplateEnabled.Value);

        foreach (var spawnConfig in configs)
        {
            if (string.IsNullOrWhiteSpace(spawnConfig.PrefabName?.Value))
            {
                Log.LogWarning($"PrefabName of world spawner config {spawnConfig.SectionKey} is empty. Skipping config.");
                continue;
            }

            if (spawnConfig.Index < 0)
            {
                Log.LogWarning($"Index of world spawner config {spawnConfig.SectionKey} is less than 0. Skipping config.");
                continue;
            }

            var builder = spawnerConfigs.ConfigureWorldSpawner((uint)spawnConfig.Index);

            ApplyConfigToBuilder(spawnConfig, builder);
        }
    }

    private static void ApplyConfigToBuilder(SpawnConfiguration config, IWorldSpawnBuilder builder)
    {
        // Default
        config.Name.SetIfHasValue(builder.SetTemplateName);
        config.PrefabName.SetIfHasValue(builder.SetPrefabName);
        config.RequiredGlobalKey.SetIfHasValue(builder.SetConditionRequiredGlobalKey);
        config.RequiredEnvironments.SetIfHasValue(builder.SetConditionEnvironments);

        builder.SetEnabled(config.Enabled.Value);
        builder.SetTemplateEnabled(config.TemplateEnabled.Value);
        builder.SetConditionBiomes(config.ExtractBiomeMask());
        builder.SetModifierHuntPlayer(config.HuntPlayer.Value);
        builder.SetMaxSpawned((uint)config.MaxSpawned.Value);
        builder.SetSpawnInterval(TimeSpan.FromSeconds(config.SpawnInterval.Value));
        builder.SetSpawnChance(config.SpawnChance.Value);
        builder.SetMinLevel((uint)config.LevelMin.Value);
        builder.SetMaxLevel((uint)config.LevelMax.Value);
        builder.SetDistanceToCenterForLevelUp(config.LevelUpMinCenterDistance.Value);
        builder.SetMinDistanceToOther(config.SpawnDistance.Value);
        builder.SetSpawnAtDistanceToPlayerMin(config.SpawnRadiusMin.Value);
        builder.SetSpawnAtDistanceToPlayerMax(config.SpawnRadiusMax.Value);
        builder.SetPackSizeMin((uint)config.GroupSizeMin.Value);
        builder.SetPackSizeMax((uint)config.GroupSizeMax.Value);
        builder.SetPackSpawnCircleRadius(config.GroupRadius.Value);
        builder.SetSpawnAtDistanceToGround(config.GroundOffset.Value);
        builder.SetSpawnDuringDay(config.SpawnDuringDay.Value);
        builder.SetSpawnDuringNight(config.SpawnDuringNight.Value);
        builder.SetConditionAltitude(config.ConditionAltitudeMin.Value, config.ConditionAltitudeMax.Value);
        builder.SetConditionTilt(config.ConditionTiltMin.Value, config.ConditionTiltMax.Value);
        builder.SetSpawnInForest(config.SpawnInForest.Value);
        builder.SetSpawnOutsideForest(config.SpawnOutsideForest.Value);
        builder.SetConditionOceanDepth(config.OceanDepthMin.Value, config.OceanDepthMax.Value);

        // Conditions
        var playerConditionsDistance = (int)config.DistanceToTriggerPlayerConditions.Value;

        config.ConditionLocation.SetIfHasValue(builder.SetConditionLocation);
        config.RequiredNotGlobalKey.SetIfHasValue(builder.SetGlobalKeysRequiredMissing);
        config.ConditionNearbyPlayersCarryValue.SetIfGreaterThanZero(x => builder.SetConditionNearbyPlayersCarryValue(playerConditionsDistance, x));
        config.ConditionNearbyPlayersNoiseThreshold.SetIfGreaterThanZero(x => builder.SetConditionNearbyPlayersNoise(playerConditionsDistance, x));
        config.ConditionAreaSpawnChance.SetIfNotEqual(100, builder.SetConditionAreaSpawnChance);

        builder.SetConditionDistanceToCenter(config.ConditionDistanceToCenterMin.Value, config.ConditionDistanceToCenterMax.Value);
        builder.SetConditionWorldAge((int)config.ConditionWorldAgeDaysMin.Value, (int)config.ConditionWorldAgeDaysMax.Value);

        if (config.ConditionNearbyPlayerCarriesItem.Value.IsNotEmpty())
        {
            builder.SetConditionNearbyPlayersCarryItem((int)config.DistanceToTriggerPlayerConditions.Value, config.ConditionNearbyPlayerCarriesItem.Value.SplitByComma());
        }
        if (config.ConditionNearbyPlayersStatus.Value.IsNotEmpty())
        {
            builder.SetConditionNearbyPlayersStatus((int)config.DistanceToTriggerPlayerConditions.Value, config.ConditionNearbyPlayersStatus.Value.SplitByComma().ToArray());
        }
        if (config.ConditionAreaIds.Value.IsNotEmpty())
        {
            builder.SetConditionAreaIds(config.ConditionAreaIds.Value.SplitByComma().ConvertAll(x => int.Parse(x)));
        }

        // Conditions - Integrations
        Config cfg;

        {
            if (config.TryGet(SpawnSystemConfigCLLC.ModName, out cfg) &&
                cfg is SpawnSystemConfigCLLC cllcConfig)
            {
                if (cllcConfig.ConditionWorldLevelMin.Value >= 0 || cllcConfig.ConditionWorldLevelMax.Value >= 0)
                {
                    builder.SetCllcConditionWorldLevel(cllcConfig.ConditionWorldLevelMin.Value, cllcConfig.ConditionWorldLevelMax.Value);
                }
            }

            if (config.TryGet(SpawnSystemConfigEpicLoot.ModName, out cfg) &&
                cfg is SpawnSystemConfigEpicLoot elConfig)
            {
                if (elConfig.ConditionNearbyPlayerCarryLegendaryItem.Value.IsNotEmpty())
                {
                    builder.SetEpicLootConditionNearbyPlayerCarryLegendaryItem((int)config.DistanceToTriggerPlayerConditions.Value, elConfig.ConditionNearbyPlayerCarryLegendaryItem.Value.SplitByComma());
                }
                if (elConfig.ConditionNearbyPlayerCarryItemWithRarity.Value.IsNotEmpty())
                {
                    builder.SetEpicLootConditionNearbyPlayersCarryItemWithRarity((int)config.DistanceToTriggerPlayerConditions.Value, elConfig.ConditionNearbyPlayerCarryItemWithRarity.Value.SplitByComma());
                }
            }
        }

        // Position conditions
        config.ConditionLocation.SetIfHasValue(builder.SetPositionConditionLocation);

        // Modifiers
        if (config.SetFaction.Value.IsNotEmpty())
        {
            builder.SetModifier(new ModifierSetFaction(config.SetFaction.Value));
        }

        builder.SetModifierRelentless(config.SetRelentless.Value);
        builder.SetModifierDespawnOnConditionsInvalid(config.SpawnDuringDay.Value, config.SpawnDuringNight.Value, config.RequiredEnvironments.Value.SplitByComma());
        builder.SetModifierDespawnOnAlert(config.SetTryDespawnOnAlert.Value);

        config.TemplateId.SetIfHasValue(builder.SetModifierTemplateId);

        builder.SetModifierTamed(config.SetTamed.Value);
        builder.SetModifierTamedCommandable(config.SetTamedCommandable.Value);

        // Modifiers - Integrations
        {
            if (config.TryGet(SpawnSystemConfigCLLC.ModName, out cfg) &&
                cfg is SpawnSystemConfigCLLC cllcConfig)
            {
                if (cllcConfig.SetBossAffix.Value.IsNotEmpty() &&
                    Enum.TryParse(cllcConfig.SetBossAffix.Value, out CllcBossAffix bossAffix))
                {
                    builder.SetCllcModifierBossAffix(bossAffix);
                }

                if (cllcConfig.SetExtraEffect.Value.IsNotEmpty() &&
                    Enum.TryParse(cllcConfig.SetExtraEffect.Value, out CllcCreatureExtraEffect extraEffect))
                {
                    builder.SetCllcModifierExtraEffect(extraEffect);
                }

                if (cllcConfig.SetInfusion.Value.IsNotEmpty() &&
                    Enum.TryParse(cllcConfig.SetInfusion.Value, out CllcCreatureInfusion infusion))
                {
                    builder.SetCllcModifierInfusion(infusion);
                }

                if (cllcConfig.UseDefaultLevels.Value)
                {
                    builder.SetModifier(new ModifierDefaultRollLevel(config.LevelMin.Value, config.LevelMax.Value, 0, 10f));
                }
            }

            if (config.TryGet(SpawnSystemConfigMobAI.ModName, out cfg) &&
                cfg is SpawnSystemConfigMobAI mobAIConfig)
            {
                if (mobAIConfig.SetAI.Value.IsNotEmpty())
                {
                    builder.SetMobAiModifier(mobAIConfig.SetAI.Value, mobAIConfig.AIConfigFile.Value);
                }
            }
        }
    }

    private static void SetIfHasValue(this ConfigurationEntry<string> value, Func<string, IWorldSpawnBuilder> apply)
    {
        if (value is not null && 
            value.Value.IsNotEmpty())
        {
            apply(value.Value);
        }
    }

    private static void SetIfHasValue(this ConfigurationEntry<string> value, Func<List<string>, IWorldSpawnBuilder> apply)
    {
        if (value is not null &&
            value.Value.IsNotEmpty())
        {
            apply(value.Value.SplitByComma());
        }
    }

    private static void SetIfGreaterThanZero(this ConfigurationEntry<int> value, Func<int, IWorldSpawnBuilder> apply)
    {
        if (value is not null &&
            value.Value > 0)
        {
            apply(value.Value);
        }
    }

    private static void SetIfNotEqual<T>(this ConfigurationEntry<T> value, T compare, Func<T, IWorldSpawnBuilder> apply)
    {
        if (value is not null &&
            value.Value.Equals(compare))
        {
            apply(value.Value);
        }
    }

    private static void SetIfGreaterThanZero(this ConfigurationEntry<float> value, Func<float, IWorldSpawnBuilder> apply)
    {
        if (value is not null &&
            value.Value > 0)
        {
            apply(value.Value);
        }
    }
}
