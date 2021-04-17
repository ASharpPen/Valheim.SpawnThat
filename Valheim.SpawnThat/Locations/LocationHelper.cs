using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;

namespace Valheim.SpawnThat.Locations
{
    [HarmonyPatch(typeof(ZoneSystem))]
    public static class LocationHelper
    {
        private static Dictionary<Vector2i, ZoneSystem.LocationInstance> _locationInstances { get; set; } = null;
        private static Dictionary<Vector2i, SimpleLocation> _simpleLocationsByZone { get; set; } = null;

        static LocationHelper()
        {
            StateResetter.Subscribe(() =>
            {
                _locationInstances = null;
                _simpleLocationsByZone = null;
            });
        }

        internal static void SetLocations(IEnumerable<SimpleLocation> locations)
        {
            if (_simpleLocationsByZone is null)
            {
#if DEBUG
                Log.LogDebug($"Instantiating new dictionary for SimpleLocations.");
#endif
                _simpleLocationsByZone = new Dictionary<Vector2i, SimpleLocation>();
            }

#if DEBUG
            Log.LogDebug($"Assigning locations.");
#endif

            foreach (var location in locations)
            {
                _simpleLocationsByZone[location.ZonePosition] = location;
            }
        }

        public static SimpleLocation FindLocation(Vector3 position)
        {
            var zoneId = ZoneSystem.instance.GetZone(position);

            if ((_locationInstances?.Count ?? 0) > 0)
            {
                if(_locationInstances.TryGetValue(zoneId, out ZoneSystem.LocationInstance defaultLocation))
                {
                    return new SimpleLocation
                    {
                        LocationName = defaultLocation.m_location?.m_prefabName ?? "",
                        Position = defaultLocation.m_position,
                        ZonePosition = zoneId,
                    };
                }
            }

            if (_simpleLocationsByZone is not null)
            {
                if (_simpleLocationsByZone.TryGetValue(zoneId, out SimpleLocation location))
                {
                    return location;
                }
            }

            return null;
        }

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void LoadLocations(Dictionary<Vector2i, ZoneSystem.LocationInstance> ___m_locationInstances)
        {
            _locationInstances = ___m_locationInstances;
        }
    }
}
