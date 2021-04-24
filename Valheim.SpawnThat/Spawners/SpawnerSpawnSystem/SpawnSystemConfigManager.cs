using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Debugging;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.Spawners.Caches;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem
{
    public static class SpawnSystemConfigManager
    {
        private static bool FirstApplication = true;

        private const string FileNamePre = "world_spawners_pre_changes.txt";
        private const string FileNamePost = "world_spawners_post_changes.txt";

        internal static bool Wait = true;

        static SpawnSystemConfigManager()
        {
            StateResetter.Subscribe(() =>
            {
                Wait = true;
            });
        }

        public static void ApplyConfigsIfMissing(SpawnSystem __instance, Heightmap ___m_heightmap)
        {
            if (Wait)
            {
                return;
            }

            if (__instance.IsInitialized())
            {
                return;
            }

            try
            {
                ApplyConfigs(__instance, ___m_heightmap);
            }
            catch (Exception e)
            {
                Log.LogError($"Error while applying configs to SpawnSystem spawner {__instance}.", e);
                __instance.SetFailedInitialization();
            }

            if (__instance.IsFailedInitialization() && __instance.GetFailedInitCount() >= 2)
            {
                Log.LogTrace($"Too many failed initialization attempts for spawner {__instance}, will stop retrying.");
                __instance.SetInitialized(true);
                return;
            }
        }

        public static void ApplyConfigs(SpawnSystem __instance, Heightmap ___m_heightmap)
        {
            if (__instance.transform?.position is null)
            {
                __instance.SetSuccessfulInit();
                return;
            }

            var spawnerPos = __instance.transform.position;
            Log.LogTrace($"Modifying SpawnSystem at pos {spawnerPos}");

            if (__instance.m_spawners is null)
            {
                __instance.m_spawners = new List<SpawnSystem.SpawnData>();
            }

            if (ConfigurationManager.GeneralConfig?.WriteSpawnTablesToFileBeforeChanges?.Value == true && FirstApplication)
            {
                SpawnDataFileDumper.WriteToFile(__instance.m_spawners, FileNamePre);
            }

            if (ConfigurationManager.GeneralConfig?.ClearAllExisting?.Value == true)
            {
                Log.LogTrace($"Clearing spawners from spawn system: {spawnerPos}");
                __instance.m_spawners.Clear();
            }

            //SpawnSystem config is only expected to have a single first layer, namely "WorldSpawner", so we just grab the first entry.
            var spawnSystemConfigs = ConfigurationManager
                .SpawnSystemConfig?
                .Subsections? //[*]
                .Values?
                .FirstOrDefault()?
                .Subsections?//[WorldSpawner.*]
                .Values;

            if ((spawnSystemConfigs?.Count ?? 0) > 0)
            {
                foreach (var spawnConfig in spawnSystemConfigs.OrderBy(x => x.Index))
                {
                    if (string.IsNullOrWhiteSpace(spawnConfig.PrefabName?.Value))
                    {
                        Log.LogWarning($"PrefabName of config {spawnConfig.SectionKey} is empty. Skipping config.");
                        continue;
                    }

                    if (ConditionManager.Instance.FilterOnAwake(__instance, spawnConfig))
                    {
                        continue;
                    }

                    if (spawnConfig.Index < __instance.m_spawners.Count && ConfigurationManager.GeneralConfig?.AlwaysAppend?.Value == false)
                    {
                        Log.LogTrace($"Overriding world spawner entry {spawnConfig.Index}");
                        var spawner = __instance.m_spawners[spawnConfig.Index];

                        Override(spawner, spawnConfig);

                        SpawnSystemConfigCache.Set(spawner, spawnConfig);
                    }
                    else
                    {
                        Log.LogTrace($"Adding new spawner entry {spawnConfig.Name}");
                        var spawner = CreateNewEntry(spawnConfig);

                        SpawnSystemConfigCache.Set(spawner, spawnConfig);

                        __instance.m_spawners.Add(spawner);
                    }
                }
            }

            var simpleConfigs = ConfigurationManager.SimpleConfig?.Subsections;

            if (simpleConfigs is not null && simpleConfigs.Count > 0)
            {
                foreach (var spawner in __instance.m_spawners)
                {
                    if (string.IsNullOrWhiteSpace(spawner.m_prefab?.name))
                    {
                        continue;
                    }

                    var name = spawner.m_prefab.name;
                    var cleanedName = name.Trim().ToUpper();

                    if (simpleConfigs.TryGetValue(cleanedName, out SimpleConfig simpleConfig))
                    {
                        spawner.m_maxSpawned = (int)Math.Round(spawner.m_maxSpawned * simpleConfig.SpawnMaxMultiplier.Value);
                        spawner.m_groupSizeMin = (int)Math.Round(spawner.m_groupSizeMin * simpleConfig.GroupSizeMinMultiplier.Value);
                        spawner.m_groupSizeMax = (int)Math.Round(spawner.m_groupSizeMax * simpleConfig.GroupSizeMaxMultiplier.Value);
                        spawner.m_spawnInterval = (simpleConfig.SpawnFrequencyMultiplier.Value != 0)
                            ? spawner.m_spawnInterval / simpleConfig.SpawnFrequencyMultiplier.Value
                            : 0;
                    }
                }
            }

            __instance.SetSuccessfulInit();

            if (ConfigurationManager.GeneralConfig?.WriteSpawnTablesToFileAfterChanges?.Value == true && FirstApplication)
            {
                SpawnDataFileDumper.WriteToFile(__instance.m_spawners, FileNamePost);
            }

            DisableOutsideBiome(___m_heightmap, __instance.m_spawners);

            FirstApplication = false;
        }

        /// <summary>
        /// Minor improvements to disable spawners when unchanging conditions are not met.
        /// No more checking the same thing every update.
        /// </summary>
        public static void DisableOutsideBiome(Heightmap heightmap, List<SpawnSystem.SpawnData> spawners)
        {
            if (spawners is null)
            {
                return;
            }

            foreach (var spawner in spawners)
            {
                if (!spawner.m_enabled)
                {
                    continue;
                }

                if (!heightmap.HaveBiome(spawner.m_biome))
                {
                    spawner.m_enabled = false;
                }
            }
        }

        /// <summary>
        /// Overriding should be fine, as SpawnData is for some reason unique per spawner. Works out for us I guess.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="config"></param>
        public static void Override(SpawnSystem.SpawnData original, SpawnConfiguration config)
        {
            var prefab = ZNetScene.instance.GetPrefab(config.PrefabName.Value);

            Heightmap.Biome biome = ConvertToBiomeFlag(config);

            var envs = config.RequiredEnvironments?.Value?.SplitByComma() ?? new List<string>(0);

            original.m_name = config.Name.Value;
            original.m_enabled = config.Enabled.Value;
            original.m_biome = biome;
            original.m_prefab = prefab;
            original.m_huntPlayer = config.HuntPlayer.Value;
            original.m_maxSpawned = config.MaxSpawned.Value;
            original.m_spawnInterval = config.SpawnInterval.Value;
            original.m_spawnChance = config.SpawnChance.Value;
            original.m_minLevel = config.LevelMin.Value;
            original.m_maxLevel = config.LevelMax.Value;
            original.m_levelUpMinCenterDistance = config.LevelUpMinCenterDistance.Value;
            original.m_spawnDistance = config.SpawnDistance.Value;
            original.m_spawnRadiusMin = config.SpawnRadiusMin.Value;
            original.m_spawnRadiusMax = config.SpawnRadiusMax.Value;
            original.m_requiredGlobalKey = config.RequiredGlobalKey.Value;
            original.m_requiredEnvironments = envs;
            original.m_groupSizeMin = config.GroupSizeMin.Value;
            original.m_groupSizeMax = config.GroupSizeMax.Value;
            original.m_groupRadius = config.GroupRadius.Value;
            original.m_groundOffset = config.GroundOffset.Value;
            original.m_spawnAtDay = config.SpawnDuringDay.Value;
            original.m_spawnAtNight = config.SpawnDuringNight.Value;
            original.m_minAltitude = config.ConditionAltitudeMin.Value;
            original.m_maxAltitude = config.ConditionAltitudeMax.Value;
            original.m_minTilt = config.ConditionTiltMin.Value;
            original.m_maxTilt = config.ConditionTiltMax.Value;
            original.m_inForest = config.SpawnInForest.Value;
            original.m_outsideForest = config.SpawnOutsideForest.Value;
            original.m_maxOceanDepth = config.OceanDepthMax.Value;
            original.m_minOceanDepth = config.OceanDepthMin.Value;
        }

        public static SpawnSystem.SpawnData CreateNewEntry(SpawnConfiguration config)
        {
            var prefab = ZNetScene.instance.GetPrefab(config.PrefabName.Value);

            Heightmap.Biome biome = ConvertToBiomeFlag(config);

            var envs = config.RequiredEnvironments?.Value?.SplitByComma() ?? new List<string>(0);

            var spawnData = new SpawnSystem.SpawnData
            {
                m_name = config.Name.Value,
                m_prefab = prefab,
                m_enabled = config.Enabled.Value,
                m_biome = biome,
                m_groupRadius = config.GroupRadius.Value,
                m_groupSizeMax = config.GroupSizeMax.Value,
                m_groupSizeMin = config.GroupSizeMin.Value,
                m_huntPlayer = config.HuntPlayer.Value,
                m_inForest = config.SpawnInForest.Value,
                m_outsideForest = config.SpawnOutsideForest.Value,
                m_levelUpMinCenterDistance = config.LevelUpMinCenterDistance.Value,
                m_maxAltitude = config.ConditionAltitudeMax.Value,
                m_minAltitude = config.ConditionAltitudeMin.Value,
                m_maxLevel = config.LevelMax.Value,
                m_minLevel = config.LevelMin.Value,
                m_maxOceanDepth = config.OceanDepthMax.Value,
                m_minOceanDepth = config.OceanDepthMin.Value,
                m_maxTilt = config.ConditionTiltMax.Value,
                m_minTilt = config.ConditionTiltMin.Value,
                m_maxSpawned = config.MaxSpawned.Value,
                m_requiredEnvironments = envs,
                m_requiredGlobalKey = config.RequiredGlobalKey.Value,
                m_spawnAtDay = config.SpawnDuringDay.Value,
                m_spawnAtNight = config.SpawnDuringNight.Value,
                m_spawnChance = config.SpawnChance.Value,
                m_spawnDistance = config.SpawnDistance.Value,
                m_spawnInterval = config.SpawnInterval.Value,
                m_spawnRadiusMax = config.SpawnRadiusMax.Value,
                m_spawnRadiusMin = config.SpawnRadiusMin.Value,
                m_groundOffset = config.GroundOffset.Value,
            };

            return spawnData;
        }

        private static Heightmap.Biome ConvertToBiomeFlag(SpawnConfiguration config)
        {
            //Well, since you bastards were packing enums before, lets return the gesture (not really, <3 you devs!)
            Heightmap.Biome biome = Heightmap.Biome.None;

            var biomeArray = config.Biomes?.Value?.SplitByComma() ?? new List<string>(0);

            if (biomeArray.Count == 0)
            {
                //Set all biomes allowed.
                biome = (Heightmap.Biome)1023;
            }

            foreach (var requiredBiome in biomeArray)
            {
                if (Enum.TryParse(requiredBiome, out Heightmap.Biome reqBiome))
                {
                    biome |= reqBiome;
                }
                else
                {
                    Log.LogWarning($"Unable to parse biome '{requiredBiome}' of spawner config {config.Index}");
                }
            }

            return biome;
        }
    }
}
