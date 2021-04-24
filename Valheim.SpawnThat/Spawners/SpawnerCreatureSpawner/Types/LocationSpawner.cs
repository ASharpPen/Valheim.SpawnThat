using System;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Locations;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.Types
{
    internal static class LocationSpawner
    {
        public static bool ApplyConfig(CreatureSpawner spawner)
        {
            CreatureSpawnerConfig config;
            try
            {
                var spawnerPos = spawner.transform.position;

                var prefabName = spawner.m_creaturePrefab?.name;

                if (string.IsNullOrWhiteSpace(prefabName))
                {
                    return false;
                }

                var locationName = FindLocationName(spawnerPos);

                if (string.IsNullOrEmpty(locationName))
                {
                    return false;
                }

                string creatureName = spawner?.m_creaturePrefab?.name?.Trim()?.ToUpperInvariant();

                config = ConfigFinder.SearchConfig(locationName, creatureName);
            }
            catch (Exception e)
            {
                Log.LogWarning($"Error while attempting to find config for local {spawner}: " + e.Message);
                return false;
            }

            try
            {
                SpawnerConfigApplier.ApplyConfiguration(spawner, config);
                return true;
            }
            catch(Exception e)
            {
                Log.LogWarning($"Error while attempting to apply config to local spawner {spawner}: " + e.Message);
                return false;
            }
        }

        private static string FindLocationName(Vector3 pos)
        {
            var location = LocationHelper.FindLocation(pos);

            if (string.IsNullOrWhiteSpace(location?.LocationName))
            {
#if DEBUG
                Log.LogWarning("Empty location prefab.");
#endif
                return null;
            }

            return location.LocationName;
        }
    }
}
