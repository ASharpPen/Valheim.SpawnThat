using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Debugging;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem;

namespace Valheim.SpawnThat.SpawnerSpawnSystem
{
    [HarmonyPatch(typeof(SpawnSystem), "Awake")]
    public static class SpawnSystemPatch
    {
        private const string FileNamePre = "world_spawners_pre_changes.txt";
        private const string FileNamePost = "world_spawners_post_changes.txt";

        internal static bool FirstApplication = true;

        private static SpawnSystemConfigurationFile Config => ConfigurationManager.SpawnSystemConfig;

        private static void Postfix(SpawnSystem __instance, Heightmap ___m_heightmap, ZNetView ___m_nview)
        {
            var spawnerPos = __instance.transform.position;
            Log.LogTrace($"Postfixing SpawnSystem Awake at pos {spawnerPos}");

            if (ConfigurationManager.GeneralConfig.WriteSpawnTablesToFileBeforeChanges.Value && FirstApplication)
            {
                SpawnDataFileDumper.WriteToFile(__instance.m_spawners, FileNamePre);
            }

            if (ConfigurationManager.GeneralConfig.ClearAllExisting?.Value == true)
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
                //TODO: Clean up some of these extractions, too many copies
                foreach (var spawnConfig in spawnSystemConfigs.OrderBy(x => x.Index))
                {
                    var distance = spawnerPos.magnitude;

                    if(distance < spawnConfig.ConditionDistanceToCenterMin.Value)
                    {
                        Log.LogTrace($"Ignoring world config {spawnConfig.Name} due to distance less than min.");
                        continue;
                    }
                    if(spawnConfig.ConditionDistanceToCenterMax.Value > 0 && distance > spawnConfig.ConditionDistanceToCenterMax.Value)
                    {
                        Log.LogTrace($"Ignoring world config {spawnConfig.Name} due to distance greater than max.");
                        continue;
                    }

                    var day = EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds());

                    if (spawnConfig.Index < __instance.m_spawners.Count && !ConfigurationManager.GeneralConfig.AlwaysAppend.Value)
                    {
                        Log.LogTrace($"Overriding world spawner entry {spawnConfig.Index}");
                        var spawner = __instance.m_spawners[spawnConfig.Index];

                        Override(spawner, spawnConfig);

                        SpawnDataCache.Set(spawner, spawnConfig);
                    }
                    else
                    {
                        Log.LogTrace($"Adding new spawner entry {spawnConfig.Name}");
                        var spawner = CreateNewEntry(spawnConfig);

                        SpawnDataCache.Set(spawner, spawnConfig);

                        __instance.m_spawners.Add(spawner);
                    }
                }
            }

            var simpleConfigs = ConfigurationManager.SimpleConfig?.Subsections;

            if (simpleConfigs is not null && simpleConfigs.Count > 0)
            {
                foreach (var spawner in __instance.m_spawners)
                {
                    var name = spawner.m_prefab.name;
                    var cleanedName = name.Trim().ToUpper();

                    if (simpleConfigs.TryGetValue(cleanedName, out SimpleConfig simpleConfig))
                    {
                        Log.LogDebug($"Found and applying simple config {simpleConfig.SectionKey} for spawner of {name}");

                        spawner.m_maxSpawned = (int)Math.Round(spawner.m_maxSpawned * simpleConfig.SpawnMaxMultiplier.Value);
                        spawner.m_groupSizeMin = (int)Math.Round(spawner.m_groupSizeMin * simpleConfig.GroupSizeMinMultiplier.Value);
                        spawner.m_groupSizeMax = (int)Math.Round(spawner.m_groupSizeMax * simpleConfig.GroupSizeMaxMultiplier.Value);
                        spawner.m_spawnInterval = (simpleConfig.SpawnFrequencyMultiplier.Value != 0)
                            ? spawner.m_spawnInterval / simpleConfig.SpawnFrequencyMultiplier.Value
                            : 0;
                    }
                }
            }
            
            if (ConfigurationManager.GeneralConfig.WriteSpawnTablesToFileAfterChanges.Value && FirstApplication)
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
            foreach(var spawner in spawners)
            {
                if(!spawner.m_enabled)
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

            var envs = config.RequiredEnvironments?.Value?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)?.ToList();

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

            var envs = config.RequiredEnvironments?.Value?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)?.ToList();

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

            var biomeArray = config.Biomes.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (biomeArray.Length == 0)
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
