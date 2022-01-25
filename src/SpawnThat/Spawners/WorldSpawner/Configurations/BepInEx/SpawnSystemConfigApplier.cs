using System;
using System.Linq;
using SpawnThat.Core.Configuration;
using SpawnThat.Core;
using SpawnThat.Utilities.Extensions;
using SpawnThat.Utilities;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;

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
            .Where(x => x.Enabled.Value);

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
        if (config.Name?.Value.IsNotEmpty() == true)
        {
            builder.SetTemplateName(config.Name.Value);
        }

        builder.SetTemplateEnabled(config.Enabled.Value);
        builder.SetConditionBiomes(config.ExtractBiomeMask());

        if (config.PrefabName.Value.IsNotEmpty())
        {
            builder.SetPrefabName(config.PrefabName.Value);
        }

        builder.SetModifierHuntPlayer(config.HuntPlayer.Value);
        builder.SetMaxSpawned((uint)config.MaxSpawned.Value);
        builder.SetSpawnInterval(TimeSpan.FromSeconds(config.SpawnInterval.Value));
        builder.SetSpawnChance(config.SpawnChance.Value);
        builder.SetMinLevel((uint)config.LevelMin.Value);
        builder.SetMaxLevel((uint)config.LevelMax.Value);
        builder.SetLevelUpDistance(config.LevelUpMinCenterDistance.Value);
        builder.SetMinDistanceToOther(config.SpawnDistance.Value);
        builder.SetSpawnAtDistanceToPlayerMin(config.SpawnRadiusMin.Value);
        builder.SetSpawnAtDistanceToPlayerMax(config.SpawnRadiusMax.Value);

        if (config.RequiredGlobalKey.Value.IsNotEmpty())
        {
            builder.SetConditionRequiredGlobalKey(config.RequiredGlobalKey.Value);
        }
        if (config.RequiredEnvironments.Value.IsNotEmpty())
        {
            builder.SetConditionEnvironments(config.RequiredEnvironments.Value.SplitByComma());
        }
        builder.SetPackSizeMin((uint)config.GroupSizeMin.Value);
        builder.SetPackSizeMax((uint)config.GroupSizeMax.Value);
        builder.SetPackSpawnCircleRadius(config.GroupRadius.Value);
        builder.SetSpawnAtDistanceToGround(config.GroundOffset.Value);
        builder.SetConditionAllowDuringDay(config.SpawnDuringDay.Value);
        builder.SetConditionAllowDuringNight(config.SpawnDuringNight.Value);
        builder.SetConditionAltitude(config.ConditionAltitudeMin.Value, config.ConditionAltitudeMax.Value);
        builder.SetConditionTilt(config.ConditionTiltMin.Value, config.ConditionTiltMax.Value);
        builder.SetConditionAllowInForest(config.SpawnInForest.Value);
        builder.SetConditionAllowOutsideForest(config.SpawnOutsideForest.Value);
        builder.SetConditionOceanDepth(config.OceanDepthMin.Value, config.OceanDepthMax.Value);

        // Conditions
        builder.SetConditionDistanceToCenter(config.ConditionDistanceToCenterMin.Value, config.ConditionDistanceToCenterMax.Value);
        if (config.ConditionLocation.Value.IsNotEmpty())
        {
            builder.SetConditionLocation(config.ConditionLocation.Value.SplitByComma());
        }
        builder.SetConditionWorldAge((int)config.ConditionWorldAgeDaysMin.Value, (int)config.ConditionWorldAgeDaysMax.Value);
        if (config.RequiredNotGlobalKey.Value.IsNotEmpty())
        {
            builder.AddOrReplaceCondition(new ConditionGlobalKeysRequiredMissing(config.RequiredNotGlobalKey.Value.SplitByComma().ToArray()));
        }
        if (config.ConditionNearbyPlayersCarryValue.Value > 0)
        {
            builder.SetConditionNearbyPlayersCarryValue((int)config.DistanceToTriggerPlayerConditions.Value, config.ConditionNearbyPlayersCarryValue.Value);
        }
        if (config.ConditionNearbyPlayerCarriesItem.Value.IsNotEmpty())
        {
            builder.AddConditionNearbyPlayersCarryItem((int)config.DistanceToTriggerPlayerConditions.Value, config.ConditionNearbyPlayerCarriesItem.Value.SplitByComma());
        }
        if (config.ConditionNearbyPlayersNoiseThreshold.Value > 0)
        {
            builder.SetConditionNearbyPlayersNoise((int)config.DistanceToTriggerPlayerConditions.Value, config.ConditionNearbyPlayersNoiseThreshold.Value);
        }
        if (config.ConditionNearbyPlayersStatus.Value.IsNotEmpty())
        {
            builder.AddConditionNearbyPlayersStatus((int)config.DistanceToTriggerPlayerConditions.Value, config.ConditionNearbyPlayersStatus.Value.SplitByComma().ToArray());
        }
        if (config.ConditionAreaSpawnChance.Value != 100)
        {
            builder.SetConditionAreaSpawnChance(config.ConditionAreaSpawnChance.Value);
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
                    builder.AddCllcConditionWorldLevel(cllcConfig.ConditionWorldLevelMin.Value, cllcConfig.ConditionWorldLevelMax.Value);
                }
            }

            if (config.TryGet(SpawnSystemConfigEpicLoot.ModName, out cfg) &&
                cfg is SpawnSystemConfigEpicLoot elConfig)
            {
                if (elConfig.ConditionNearbyPlayerCarryLegendaryItem.Value.IsNotEmpty())
                {
                    builder.AddEpicLootConditionNearbyPlayerCarryLegendaryItem((int)config.DistanceToTriggerPlayerConditions.Value, elConfig.ConditionNearbyPlayerCarryLegendaryItem.Value.SplitByComma());
                }
                if (elConfig.ConditionNearbyPlayerCarryItemWithRarity.Value.IsNotEmpty())
                {
                    builder.AddEpicLootConditionNearbyPlayersCarryItemWithRarity((int)config.DistanceToTriggerPlayerConditions.Value, elConfig.ConditionNearbyPlayerCarryItemWithRarity.Value.SplitByComma());
                }
            }
        }

        // Position conditions
        if (config.ConditionLocation.Value.IsNotEmpty())
        {
            builder.SetPositionConditionLocation(config.ConditionLocation.Value.SplitByComma());
        }

        // Modifiers
        if (config.SetFaction.Value.IsNotEmpty())
        {
            builder.AddOrReplaceModifier(new ModifierSetFaction(config.SetFaction.Value));
        }
        if (config.SetRelentless.Value)
        {
            builder.SetModifierRelentless(config.SetRelentless.Value);
        }
        if (config.SetTryDespawnOnConditionsInvalid.Value)
        {
            builder.SetModifierDespawnOnConditionsInvalid(config.SpawnDuringDay.Value, config.SpawnDuringNight.Value, config.RequiredEnvironments.Value.SplitByComma());
        }
        if (config.SetTryDespawnOnAlert.Value)
        {
            builder.SetModifierDespawnOnAlert(config.SetTryDespawnOnAlert.Value);
        }
        if (config.TemplateId.Value.IsNotEmpty())
        {
            builder.SetModifierTemplateId(config.TemplateId.Value);
        }
        builder.SetModifierTamed(config.SetTamed.Value);
        builder.SetModifierTamedCommandable(config.SetTamedCommandable.Value);

        // Modifiers - Integrations
        {
            if (config.TryGet(SpawnSystemConfigCLLC.ModName, out cfg) &&
                cfg is SpawnSystemConfigCLLC cllcConfig)
            {
                if (cllcConfig.SetBossAffix.Value.IsNotEmpty())
                {
                    builder.SetCllcModifierBossAffix(cllcConfig.SetBossAffix.Value);
                }
                if (cllcConfig.SetExtraEffect.Value.IsNotEmpty())
                {
                    builder.SetCllcModifierExtraEffect(cllcConfig.SetExtraEffect.Value);
                }
                if (cllcConfig.SetInfusion.Value.IsNotEmpty())
                {
                    builder.SetCllcModifierInfusion(cllcConfig.SetInfusion.Value);
                }

                if (cllcConfig.UseDefaultLevels.Value)
                {
                    builder.AddOrReplaceModifier(new ModifierDefaultRollLevel(config.LevelMin.Value, config.LevelMax.Value, 0, 10f));
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

}
