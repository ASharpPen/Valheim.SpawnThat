using System.Collections.Generic;
using UnityEngine;
using SpawnThat.Core;
using SpawnThat.Lifecycle;
using SpawnThat.Locations;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.World.Locations;

public static class LocationManager
{
    private static Dictionary<Vector2i, SimpleLocation> _simpleLocationsByZone { get; set; }

    static LocationManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            _simpleLocationsByZone = null;
        });
    }

    public static Dictionary<Vector2i, SimpleLocation> GetLocations()
    {
        if (_simpleLocationsByZone is not null)
        {
            return new(_simpleLocationsByZone);
        }
        else if (ZoneSystem.instance.IsNotNull() && ZoneSystem.instance.m_locationInstances is not null)
        {
            Dictionary<Vector2i, SimpleLocation> simpleLocations = new();

            foreach (var instance in ZoneSystem.instance.m_locationInstances)
            {
                simpleLocations[instance.Key] = new SimpleLocation
                {
                    LocationName = instance.Value.m_location?.m_prefabName ?? "",
                    Position = instance.Value.m_position,
                    ZonePosition = instance.Key,
                };
            }

            _simpleLocationsByZone = simpleLocations;
            return simpleLocations;
        }

        return null;
    }

    public static SimpleLocation GetLocation(Vector3 position)
    {
        var zoneSystem = ZoneSystem.instance;
        if (!zoneSystem || zoneSystem is null)
        {
            Log.LogWarning("Attempting to retrieve location before ZoneSystem is initialized.");
            return null;
        }

        var zoneId = position.GetZoneId();

        if ((zoneSystem.m_locationInstances?.Count ?? 0) > 0)
        {
            if (zoneSystem.m_locationInstances.TryGetValue(zoneId, out ZoneSystem.LocationInstance defaultLocation))
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
}
