using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.ConfigurationTypes;
using Valheim.SpawnThat.ConfigurationCore;
using UnityEngine;

namespace Valheim.SpawnThat
{
    [HarmonyPatch(typeof(SpawnSystem), "Awake")]
    public static class SpawnSystemPatch
    {
        public static List<SpawnerConfiguration> Config => ConfigurationManager.DropConfigs;

        private static HashSet<Vector3> AppliedConfigs = new HashSet<Vector3>();

        private static void Postfix(ref SpawnSystem __instance)
        {
            if(Config == null)
            {
                ConfigurationManager.LoadAllConfigurations();
            }

            if(AppliedConfigs.Contains(__instance.transform.position))
            {
                Log.LogDebug("Already applied config. Skipping.");
                return;
            }


            var field = typeof(SpawnSystem).GetField("m_instances", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            Log.LogDebug($"Searching spawners. {(field.GetValue(__instance) as List<SpawnSystem>)?.Count ?? 0}");

            var configs = Config.SelectMany(x => x.Sections.Values);

            foreach (var spawner in __instance.m_spawners)
            {
                var match = configs.FirstOrDefault(x => x.PrefabName.Value.Trim().ToUpperInvariant() == spawner.m_prefab.name.Trim().ToUpperInvariant());

                if(match != null)
                {
                    Log.LogDebug($"Applying config to {match.PrefabName}");

                    spawner.m_groupSizeMax = (int)Math.Round(spawner.m_groupSizeMax * match.GroupSizeMaxMultiplier.Value);
                    spawner.m_groupSizeMin = (int)Math.Round(spawner.m_groupSizeMin * match.GroupSizeMinMultiplier.Value);
                    spawner.m_spawnInterval = spawner.m_spawnInterval / match.SpawnFrequencyMultiplier.Value;
                    spawner.m_maxSpawned = (int)Math.Round(spawner.m_maxSpawned * match.SpawnMaxMultiplier.Value);

                    Log.LogDebug($"m_groupSizeMax: {spawner.m_groupSizeMax}");
                    Log.LogDebug($"m_groupSizeMin: {spawner.m_groupSizeMin}");
                    Log.LogDebug($"m_spawnInterval: {spawner.m_spawnInterval}");
                    Log.LogDebug($"m_maxSpawned: {spawner.m_maxSpawned}");
                }
            }

            AppliedConfigs.Add(__instance.transform.position);

            /*
            foreach (var spawnConfig in )
            {

            }

            var deer = __instance.m_spawners.Where(x => x.m_name == "deer").FirstOrDefault();

            if(deer != null)
            {
                foreach(var x in __instance.m_biomeFolded)
                {
                    Log.LogTrace($"Biome: {x}");
                }

                deer.m_prefab = 

                var heightMapField = typeof(SpawnSystem).GetField("m_spawnDistanceMin", System.Reflection.BindingFlags.NonPublic);
                if (heightMapField != null) Log.LogDebug($"Heighmap: {heightMapField.GetValue(__instance)}");

                var minSpawnField = typeof(SpawnSystem).GetField("m_spawnDistanceMin", System.Reflection.BindingFlags.NonPublic);
                var maxSpawnField = typeof(SpawnSystem).GetField("m_spawnDistanceMin", System.Reflection.BindingFlags.NonPublic);

                if (minSpawnField != null) Log.LogDebug($"MinDist: {minSpawnField.GetValue(__instance)}");
                if (minSpawnField != null) Log.LogDebug($"MaxDist: {minSpawnField.GetValue(__instance)}");

                Log.LogDebug("Found the deer!");

                deer.m_spawnRadiusMin = 0;
                deer.m_spawnRadiusMax = 0;

                deer.m_maxSpawned = 20;

                deer.m_spawnDistance = 0;
                deer.m_spawnChance = 100;
                deer.m_groupRadius = 0;
                deer.m_groupSizeMin = 10;
                deer.m_groupSizeMax = 10;

                deer.m_spawnInterval = 1;
            }
            */
        }
    }
}
