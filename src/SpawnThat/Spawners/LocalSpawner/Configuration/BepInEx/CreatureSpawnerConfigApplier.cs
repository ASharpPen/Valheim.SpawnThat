using System;
using BepInEx;
using SpawnThat.Core;
using System.IO;
using SpawnThat.Core.Toml;
using SpawnThat.Integrations;
using SpawnThat.Options.Modifiers;
using SpawnThat.Spawners.DestructibleSpawner;
using SpawnThat.Utilities;

namespace SpawnThat.Spawners.LocalSpawner.Configuration.BepInEx;

internal static class CreatureSpawnerConfigApplier
{
    internal static void ApplyBepInExConfigs(ISpawnerConfigurationCollection spawnerConfigs)
    {
        var configs = CreatureSpawnerConfigurationManager.CreatureSpawnerConfig?.Subsections;

        if ((configs?.Count ?? 0) == 0)
        {
            return;
        }

        foreach (var locationConfig in configs)
        {
            foreach (var creatureConfig in locationConfig.Value.Subsections)
            {
                if (creatureConfig.Value.TemplateEnabled.IsSet &&
                    (creatureConfig.Value.TemplateEnabled.Value ?? creatureConfig.Value.TemplateEnabled.DefaultValue.Value) == false)
                {
                    continue;
                }

                // BepInEx configs are not set up to distinguish between rooms and locations in config path.
                // Instead, the first matching name is used. Therefore, two builders are configured to leave
                // it up to the first and most specific identified to be used.
                var roomBuilder = spawnerConfigs.ConfigureLocalSpawnerByRoomAndCreature(locationConfig.Key, creatureConfig.Key);
                var locationBuilder = spawnerConfigs.ConfigureLocalSpawnerByLocationAndCreature(locationConfig.Key, creatureConfig.Key);

                ApplyConfigToBuilder(creatureConfig.Value, roomBuilder);
                ApplyConfigToBuilder(creatureConfig.Value, locationBuilder);
            }
        }
    }

    private static void ApplyConfigToBuilder(CreatureSpawnerConfig config, ILocalSpawnBuilder builder)
    {
        // Default
        config.PrefabName.SetIfHasValue(builder.SetPrefabName);
        config.Enabled.SetValueOrDefaultIfLoaded(builder.SetEnabled);
        config.LevelMin.SetIfLoaded(builder.SetMinLevel);
        config.LevelMax.SetIfLoaded(builder.SetMaxLevel);
        config.LevelUpChance.SetIfLoaded(builder.SetLevelUpChance);
        config.RespawnTime.SetIfLoaded(x => x is null
            ? builder.SetSpawnInterval(null)
            : builder.SetSpawnInterval(TimeSpan.FromMinutes(x.Value)));
        config.SetPatrolPoint.SetIfLoaded(builder.SetPatrolSpawn);
        config.SpawnAtDay.SetIfLoaded(builder.SetSpawnDuringDay);
        config.SpawnAtNight.SetIfLoaded(builder.SetSpawnDuringNight);
        config.TriggerDistance.SetIfLoaded(builder.SetConditionPlayerWithinDistance);
        config.TriggerNoise.SetIfLoaded(builder.SetConditionPlayerNoise);
        config.SpawnInPlayerBase.SetIfLoaded(builder.SetSpawnInPlayerBase);

        // Modifiers
        config.SetFaction.SetIfLoaded(builder.SetModifierFaction);
        config.SetTamed.SetValueOrDefaultIfLoaded(builder.SetModifierTamed);
        config.SetTamedCommandable.SetValueOrDefaultIfLoaded(builder.SetModifierTamedCommandable);

        // Modifiers - Integrations
        TomlConfig cfg;

        {
            if (IntegrationManager.InstalledCLLC)
            {
                if (config.TryGet(CreatureSpawnerConfigCLLC.ModName, out cfg) &&
                    cfg is CreatureSpawnerConfigCLLC cllcConfig)
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
                    if (mobAIConfig.SetAI.IsSet)
                    {
                        var ai = mobAIConfig.SetAI.Value;

                        if (string.IsNullOrWhiteSpace(ai))
                        {
                            builder.SetMobAiModifier(null, null);
                        }

                        try
                        {
                            string filePath = Path.Combine(Paths.ConfigPath, mobAIConfig.AIConfigFile.Value);

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
                        catch (Exception e)
                        {
                            Log.LogError("Error while attempting to read MobAI config.", e);
                        }
                    }
                }
            }
        }
    }

    private static void SetIfHasValue(this ITomlConfigEntry<string> value, Func<string, ILocalSpawnBuilder> apply)
    {
        if (value is not null &&
            value.IsSet &&
            value.Value.IsNotEmpty())
        {
            apply(value.Value);
        }
    }

    private static void SetIfLoaded<T>(this ITomlConfigEntry<T> value, Func<T, ILocalSpawnBuilder> apply)
    {
        if (value is not null &&
            value.IsSet)
        {
            apply(value.Value);
        }
    }

    private static void SetValueOrDefaultIfLoaded<T>(this ITomlConfigEntry<T?> value, Func<T, ILocalSpawnBuilder> apply)
        where T : struct
    {
        if (value is not null &&
            value.IsSet)
        {
            apply(value.Value ?? value.DefaultValue.Value);
        }
    }
}
