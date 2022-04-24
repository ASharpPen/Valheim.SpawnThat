using System;
using BepInEx;
using BepInEx.Logging;
using SpawnThat.Integrations.CLLC.Models;
using SpawnThat.Integrations.EpicLoot.Models;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Spawners;
using SpawnThat.Spawners.DestructibleSpawner;
using SpawnThat.Spawners.LocalSpawner;
using SpawnThat.Spawners.WorldSpawner;
using SpawnThat.Utilities.Enums;

namespace SpawnThatTestMod
{
    [BepInDependency("asharppen.valheim.spawn_that", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("asharppen.valheim.spawn_that_test", "Spawn That - Test", "0.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;

        void Awake()
        {
            try
            {
                Log = Logger;
                SpawnerConfigurationManager.OnConfigure += ConfigureSpawners;
            }
            catch(Exception e)
            {
                System.Console.WriteLine(e);
            }
        }

        public static void ConfigureSpawners(ISpawnerConfigurationCollection config)
        {
            try
            {
                ConfigureLocalSpawnerByNamed(config);
                ConfigureLocalSpawnerOverridenByFile(config);
                ConfigureLocalSpawnerWithIntegration(config);

                ConfigureWorldSpawner(config);
                ConfigureWorldSpawnerBySettings(config);
                ConfigureWorldSpawnerOverrideDefault(config);

                ConfigureDestructibleSpawner(config);
                ConfigureDestructibleSpawnerBySettings(config);
            }
            catch(Exception e)
            {
                System.Console.WriteLine($"Something went horribly wrong: {e.Message}\nStackTrace:\n{e.StackTrace}");
            }
        }

        private static void ConfigureLocalSpawnerByNamed(ISpawnerConfigurationCollection config)
        {
            try
            {
                LocalSpawnSettings settings = new()
                {
                    Conditions = new[] { new ConditionNearbyPlayersCarryValue(100, 2000) },
                    SpawnInterval = TimeSpan.FromSeconds(1),
                };

                config.ConfigureLocalSpawnerByName("Spawner_Skeleton")
                    .WithSettings(settings);

                config.ConfigureLocalSpawnerByName("Spawner_Skeleton_night_noarcher")
                    .WithSettings(settings);

                config.ConfigureLocalSpawnerByName("Spawner_Skeleton_poison")
                    .WithSettings(settings);

                config.ConfigureLocalSpawnerByName("Spawner_Skeleton_respawn_30")
                    .WithSettings(settings);
            }
            catch(Exception e)
            {
                Log.LogError(e);
            }
        }

        private static void ConfigureLocalSpawnerWithIntegration(ISpawnerConfigurationCollection config)
        {
            config.ConfigureLocalSpawnerByLocationAndCreature("StoneTowerRuins08", "Skeleton")
                .SetMinLevel(5)
                .SetMaxLevel(10)
                .SetSpawnInterval(TimeSpan.FromSeconds(1))
                .SetCllcModifierInfusion(CllcCreatureInfusion.Fire)
                .SetEpicLootConditionNearbyPlayersCarryItemWithRarity(100, EpicLootRarity.Magic, EpicLootRarity.Epic, EpicLootRarity.Rare);
        }

        /// <summary>
        /// TODO: Add an automatically created test cfg to configs. Currently just assumes a cfg exists which sets [Runestone_Boars.Boar].
        /// </summary>
        private static void ConfigureLocalSpawnerOverridenByFile(ISpawnerConfigurationCollection config)
        {
            try
            {
                config.ConfigureLocalSpawnerByLocationAndCreature("Runestone_Boars", "Boar")
                    .SetPrefabName("Skeleton");
            }
            catch(Exception e)
            {
                Log.LogError(e);
            }
        }

        private static void ConfigureWorldSpawner(ISpawnerConfigurationCollection config)
        {
            try
            {
                config.ConfigureWorldSpawner(10_000_000)
                    .SetPrefabName("Boar")
                    .SetTemplateName("Skydiver Boars")
                    .SetSpawnInterval(TimeSpan.FromSeconds(30))
                    .SetSpawnAtDistanceToGround(100)
                    .SetPackSizeMin(3)
                    .SetPackSizeMax(3)
                    .SetMaxSpawned(20)
                    .SetSpawnAtDistanceToPlayerMin(1)
                    .SetSpawnAtDistanceToPlayerMax(5)
                    ;
            }
            catch (Exception e)
            {
                Log.LogError(e);
            }
        }

        /// <summary>
        /// Disables default deer template. No more standard deer should show up in starting zone.
        /// </summary>
        private static void ConfigureWorldSpawnerOverrideDefault(ISpawnerConfigurationCollection config)
        {
            try
            {
                config.ConfigureWorldSpawner(0)
                    .SetEnabled(false)
                    .SetTemplateName("Disabled deer");
            }
            catch (Exception e)
            {
                Log.LogError(e);
            }
        }

        private static void ConfigureWorldSpawnerBySettings(ISpawnerConfigurationCollection config)
        {
            try
            {
                config.ConfigureWorldSpawner(10_000_001)
                    .WithSettings(new()
                    {
                        PrefabName = "Troll",
                        SpawnInterval = TimeSpan.FromSeconds(30),
                        Biomes = new() { Heightmap.Biome.Meadows },
                        MaxSpawned = 10
                    })
                    .SetTemplateName("Trolling meadows");
            }
            catch (Exception e)
            {
                Log.LogError(e);
            }
        }

        private static void ConfigureDestructibleSpawner(ISpawnerConfigurationCollection config)
        {
            var spawner = config.ConfigureDestructibleSpawner()
                .SetTemplateName("Meadow Spawners")
                .SetIdentifierBiome(Heightmap.Biome.Meadows)
                .SetSpawnInterval(TimeSpan.FromSeconds(5))
                ;

            spawner.GetSpawnBuilder(0)
                .SetPrefabName("Troll")
                .SetSpawnWeight(5)
                .SetConditionAllOfGlobalKeys(GlobalKey.Bat, GlobalKey.Troll)
                ;

            spawner.GetSpawnBuilder(1)
                .SetPrefabName("Wolf")
                .SetSpawnWeight(1)
                ;
        }

        private static void ConfigureDestructibleSpawnerBySettings(ISpawnerConfigurationCollection config)
        {
            var spawnerSettings = new DestructibleSpawnerSettings()
            {
                SpawnInterval = TimeSpan.FromSeconds(5),
                SetPatrol = true,
                
            };

            var spawnSettings = new DestructibleSpawnSettings
            {
                PrefabName = "Boar",
                Modifiers = new[]
                {
                    new ModifierSetTamed(true)
                },
            };

            var spawner = config.ConfigureDestructibleSpawner()
                .WithSettings(spawnerSettings)
                .SetIdentifierName("Spawner_GreydwarfNest")
                ;

            spawner.GetSpawnBuilder(0)
                .WithSettings(spawnSettings)
                .SetLevelMin(1)
                .SetLevelMax(1)
                .SetSpawnWeight(5)
                ;

            spawner.GetSpawnBuilder(1)
                .WithSettings(spawnSettings)
                .SetLevelMin(3)
                .SetLevelMax(3)
                .SetSpawnWeight(1)
                ;
        }
    }
}
