using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.ConfigurationCore;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.Types
{
    internal static class LocationSpawner
    {
        public static void ApplyConfig(CreatureSpawner spawner)
        {
            var spawnerPos = spawner.transform.position;

#if DEBUG
            var locations = ZoneSystem.instance.GetLocationList();
            var spawnerZone = ZoneSystem.instance.GetZone(spawnerPos);
            var location = locations.FirstOrDefault(x => ZoneSystem.instance.GetZone(x.m_position) == spawnerZone);
            Log.LogInfo($"Creature spawner found with location {spawnerPos}:{location.m_location.m_prefabName} - {spawner.m_creaturePrefab?.name}");
#endif

            var prefabName = spawner.m_creaturePrefab?.name;

            if (string.IsNullOrWhiteSpace(prefabName))
            {
                return;
            }

            var locationName = FindLocationName(spawnerPos);
            string creatureName = spawner?.m_creaturePrefab?.name?.Trim()?.ToUpperInvariant();

            var config = CreatureSpawnerConfigCache.SearchConfig(locationName, creatureName);

            SpawnerModifier.ApplyConfiguration(spawner, config);
        }

        private static string FindLocationName(Vector3 pos)
        {
            var locations = ZoneSystem.instance.GetLocationList();
            var spawnerZone = ZoneSystem.instance.GetZone(pos);
            var location = locations.FirstOrDefault(x => ZoneSystem.instance.GetZone(x.m_position) == spawnerZone);

            var locationName = location.m_location?.m_prefabName;

            if (string.IsNullOrWhiteSpace(locationName))
            {
                Log.LogWarning("Empty location prefab.");
                return null;
            }

            return locationName;
        }
    }
}
