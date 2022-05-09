using System;
using SpawnThat.Core.Toml;
using SpawnThat.Integrations;
using SpawnThat.Options.Modifiers;
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
                if (!creatureConfig.Value.TemplateEnabled.Value)
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
        config.Enabled.SetIfLoaded(builder.SetEnabled);
        config.LevelMin.SetIfLoaded(builder.SetMinLevel);
        config.LevelMax.SetIfLoaded(builder.SetMaxLevel);
        config.LevelUpChance.SetIfLoaded(builder.SetLevelUpChance);
        config.RespawnTime.SetIfLoaded(x => builder.SetSpawnInterval(TimeSpan.FromMinutes(x)));
        config.SetPatrolPoint.SetIfLoaded(builder.SetPatrolSpawn);
        config.SpawnAtDay.SetIfLoaded(builder.SetSpawnDuringDay);
        config.SpawnAtNight.SetIfLoaded(builder.SetSpawnDuringNight);
        config.TriggerDistance.SetIfLoaded(builder.SetConditionPlayerWithinDistance);
        config.TriggerNoise.SetIfLoaded(builder.SetConditionPlayerNoise);
        config.SpawnInPlayerBase.SetIfLoaded(builder.SetSpawnInPlayerBase);

        // Modifiers
        config.SetFaction.SetIfHasValue(x => builder.SetModifier(new ModifierSetFaction(x)));
        config.SetTamed.SetIfLoaded(builder.SetModifierTamed);
        config.SetTamedCommandable.SetIfLoaded(builder.SetModifierTamedCommandable);

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
                        x => x
                        ? builder.SetModifier(
                            new ModifierDefaultRollLevel(
                                config.LevelMin.IsSet ? config.LevelMin.Value : config.LevelMin.DefaultValue,
                                config.LevelMax.IsSet ? config.LevelMax.Value : config.LevelMax.DefaultValue,
                                0,
                                config.LevelUpChance.IsSet ? config.LevelUpChance.Value : config.LevelUpChance.DefaultValue
                                )
                            )
                        : builder.SetModifier(new ModifierDefaultRollLevel(-1, -1, 0, -1))
                        );
                }
            }

            if (IntegrationManager.InstalledMobAI)
            {
                if (config.TryGet(CreatureSpawnerConfigMobAI.ModName, out cfg) &&
                    cfg is CreatureSpawnerConfigMobAI mobAIConfig)
                {
                    mobAIConfig.AIConfigFile.SetIfLoaded(
                        x => builder.SetMobAiModifier(
                            mobAIConfig.SetAI.IsSet ? mobAIConfig.SetAI.Value ?? mobAIConfig.SetAI.DefaultValue,
                            x
                        ));
                }
            }
        }
    }

    private static void SetIfHasValue(this TomlConfigEntry<string> value, Func<string, ILocalSpawnBuilder> apply)
    {
        if (value is not null &&
            value.IsSet &&
            value.Value.IsNotEmpty())
        {
            apply(value.Value);
        }
    }

    private static void SetIfLoaded<T>(this TomlConfigEntry<T> value, Func<T, ILocalSpawnBuilder> apply)
    {
        if (value is not null &&
            value.IsSet)
        {
            apply(value.Value);
        }
    }
}
