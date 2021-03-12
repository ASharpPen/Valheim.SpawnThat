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

            Log.LogTrace("Searching for configs for local spawner {spawnerPos}");

            var prefabName = __instance.m_creaturePrefab?.name;

            if (string.IsNullOrEmpty(prefabName))
            {
                return;
            }

            var config = FindConfig(__instance, spawnerPos);

            if (config != null)
            {
                Log.LogDebug($"Found and applying config for local spawner {spawnerPos}");

                var prefab = __instance.m_creaturePrefab;

                //Find creature prefab, if it needs to be overriden
                if (prefabName != config.PrefabName.Value)
                {
                    prefab = ZNetScene.instance.GetPrefab(config.PrefabName.Value);
                }

                //Override existing config values:
                __instance.m_creaturePrefab = prefab;
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
            var configs = ConfigurationManager.CreatureSpawnerConfig;

            ConfigLookupTable = new Dictionary<string, Dictionary<string, CreatureSpawnerConfig>>();

            foreach (var config in configs)
            {
                var cleanedLocationName = config.Key.Trim().ToUpperInvariant();

                Dictionary<string, CreatureSpawnerConfig> locationTable;
                if(ConfigLookupTable.TryGetValue(cleanedLocationName, out Dictionary<string, CreatureSpawnerConfig> existingTable))
                {
                    locationTable = existingTable;
                }
                else
                {
                    locationTable = new Dictionary<string, CreatureSpawnerConfig>();
                    ConfigLookupTable.Add(cleanedLocationName, locationTable);
                }

                foreach(var creature in config.Value.Sections)
                {
                    var cleanedCreatureName = creature.Key.Trim().ToUpperInvariant();

                    if(locationTable.ContainsKey(cleanedCreatureName))
                    {
                        Log.LogWarning($"Multiple creature spawner configurations detected for {config.Key}.{creature.Key}, overriding last seen.");
                    }

                    locationTable[cleanedCreatureName] = creature.Value;
                }
            }
        }

        private static CreatureSpawnerConfig FindConfig(CreatureSpawner spawner, Vector3 spawnerPos)
        {
            //Jesus, is this really the only way? Thats so inefficient. Can't even store it, since it might be modified over time :S
            var locations = ZoneSystem.instance.GetLocationList();
            var spawnerZone = ZoneSystem.instance.GetZone(spawnerPos);
            var location = locations.FirstOrDefault(x => ZoneSystem.instance.GetZone(x.m_position) == spawnerZone);
            var locationName = location.m_location.m_prefabName.Trim().ToUpperInvariant();

            if (ConfigLookupTable.TryGetValue(locationName, out Dictionary<string, CreatureSpawnerConfig> locationTable))
            {
                string creatureName = spawner.m_creaturePrefab.name.Trim().ToUpperInvariant();

                if(locationTable.TryGetValue(creatureName, out CreatureSpawnerConfig config))
                {
                    return config;
                }
            }

            return null;
        }
    }
}
