using UnityEngine;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Locations;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.Types
{
    internal static class LocationSpawner
    {
        public static void ApplyConfig(CreatureSpawner spawner)
        {
            var spawnerPos = spawner.transform.position;

            var prefabName = spawner.m_creaturePrefab?.name;

            if (string.IsNullOrWhiteSpace(prefabName))
            {
                return;
            }

            var locationName = FindLocationName(spawnerPos);

            if(string.IsNullOrEmpty(locationName))
            {
                return;
            }

            string creatureName = spawner?.m_creaturePrefab?.name?.Trim()?.ToUpperInvariant();

            var config = ConfigFinder.SearchConfig(locationName, creatureName);

            SpawnerModifier.ApplyConfiguration(spawner, config);
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
