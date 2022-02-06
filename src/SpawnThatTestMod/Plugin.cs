using System;
using BepInEx;
using BepInEx.Logging;
using SpawnThat.Options.Conditions;
using SpawnThat.Spawners;
using SpawnThat.Spawners.LocalSpawner;

namespace SpawnThatTestMod
{
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
                ConfigureNamedLocalSpawner(config);
                ConfigureFileOverridenLocalSpawner(config);
                ConfigureWorldSpawner(config);
                ConfigureWorldSpawnerBySettings(config);
                ConfigureWorldSpawnerOverrideDefault(config);
            }
            catch(Exception e)
            {
                System.Console.WriteLine($"Something went horribly wrong: {e.Message}\nStackTrace:\n{e.StackTrace}");
            }
        }

        private static void ConfigureNamedLocalSpawner(ISpawnerConfigurationCollection config)
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

        /// <summary>
        /// TODO: Add an automatically created test cfg to configs.
        /// </summary>
        private static void ConfigureFileOverridenLocalSpawner(ISpawnerConfigurationCollection config)
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
                    .SetSpawnAtDistanceToGround(200)
                    .SetPackSizeMin(3)
                    .SetPackSizeMax(3)
                    .SetMaxSpawned(20)
                    .SetSpawnAtDistanceToPlayerMin(1)
                    .SetSpawnAtDistanceToPlayerMin(5)
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
    }
}
