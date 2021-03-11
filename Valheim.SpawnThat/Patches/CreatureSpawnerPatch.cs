using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.ConfigurationCore;
using Valheim.SpawnThat.ConfigurationTypes;

namespace Valheim.SpawnThat.Patches
{
    [HarmonyPatch(typeof(CreatureSpawner), "Awake")]
    public static class CreatureSpawnerPatch
    {
        internal static HashSet<Vector3> AppliedConfigs = new HashSet<Vector3>();

        internal static Dictionary<string, Dictionary<string, CreatureSpawnerConfig>> ConfigLookupTable = null;

        private static void Postfix(CreatureSpawner __instance)
        {
            var spawnerPos = __instance.transform.position;

            try
            {
                if (AppliedConfigs.Contains(spawnerPos))
                {
                    Log.LogTrace("Already applied config. Skipping.");

                    return;
                }
                else
                {
                    AppliedConfigs.Add(spawnerPos);
                }
            }
            catch(Exception e)
            {
                Log.LogError("Failed to check if spawner has been checked.", e);
                return;
            }

            if (ConfigLookupTable == null)
            {
                InitializeConfigs();
            }

            var prefabName = __instance.m_creaturePrefab?.name;

            if (string.IsNullOrEmpty(prefabName))
            {
                return;
            }

            var cleanedName = prefabName.Trim().ToUpperInvariant();

            var config = FindConfig(cleanedName, spawnerPos);

            if (config != null)
            {
                //TODO: Lets make this even more specific.
                Log.LogTrace($"Found and applying config for spawner within constraints {config.PrefabName}:{config.Locations}");

                //Override existing config values:

                __instance.m_levelupChance = config.LevelUpChance.Value;
                __instance.m_maxLevel = config.LevelMax.Value;
                __instance.m_minLevel = config.LevelMin.Value;
                //__instance.m_requireSpawnArea = config.RequireSpawnArea.Value; //Disabled for now, since it isn't being used by the game.
                __instance.m_respawnTimeMinuts = config.RespawnTime.Value;
                __instance.m_setPatrolSpawnPoint = config.SetPatrolPoint.Value;
                __instance.m_spawnAtDay = config.SpawnAtDay.Value;
                __instance.m_spawnAtNight = config.SpawnAtNight.Value;
                __instance.m_triggerDistance = config.TriggerDistance.Value;
                __instance.m_triggerNoise = config.TriggerNoise.Value;
            }
        }

        private static void InitializeConfigs()
        {
            var configs = ConfigurationManager.CreatureSpawnerConfig?.Sections?.Values.ToList() ?? new List<CreatureSpawnerConfig>();

            ConfigLookupTable = new Dictionary<string, Dictionary<string, CreatureSpawnerConfig>>();

            foreach (var config in configs)
            {
                string cleanedName = config.PrefabName.Value.Trim().ToUpperInvariant();

                Dictionary<string, CreatureSpawnerConfig> locationTable;
                if(ConfigLookupTable.TryGetValue(cleanedName, out Dictionary<string, CreatureSpawnerConfig> prefabConfigs))
                {
                    locationTable = prefabConfigs;
                }
                else
                {
                    locationTable = new Dictionary<string, CreatureSpawnerConfig>();
                    ConfigLookupTable.Add(cleanedName, locationTable); 
                }

                var locations = config.Locations.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if(locations.Length > 0)
                {
                    foreach(var location in locations)
                    {
                        string locationName = location.Trim().ToUpperInvariant();

                        if(locationTable.ContainsKey(locationName))
                        {
                            Log.LogWarning($"Multiple creature spawner configurations detected for {config.PrefabName.Value}:{location}, overriding last seen.");
                        }

                        locationTable[locationName] = config;
                    }
                }
                else
                {
                    locationTable[""] = config;
                }
            }
        }

        private static CreatureSpawnerConfig FindConfig(string cleanedName, Vector3 spawnerPos)
        {
            if (ConfigLookupTable.TryGetValue(cleanedName, out Dictionary<string, CreatureSpawnerConfig> locationTable))
            {
                //Jesus, is this really the only way? Thats so inefficient. Can't even store it, since it might be modified over time :S
                var locations = ZoneSystem.instance.GetLocationList();
                var spawnerZone = ZoneSystem.instance.GetZone(spawnerPos);
                var location = locations.FirstOrDefault(x => ZoneSystem.instance.GetZone(x.m_position) == spawnerZone);

                if(locationTable.TryGetValue(location.m_location.m_prefabName.Trim().ToUpperInvariant(), out CreatureSpawnerConfig locationConfig))
                {
                    return locationConfig;
                }
                else if(locationTable.TryGetValue("", out CreatureSpawnerConfig defaultConfig))
                {
                    return defaultConfig;
                }
            }

            return null;
        }
    }
}
