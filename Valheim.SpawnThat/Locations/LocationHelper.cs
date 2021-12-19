using System.Collections.Generic;
using UnityEngine;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Startup;

namespace Valheim.SpawnThat.Locations;

public static class LocationHelper
{
    private static Dictionary<Vector2i, SimpleLocation> _simpleLocationsByZone { get; set; }

    static LocationHelper()
    {
        StateResetter.Subscribe(() =>
        {
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
        if (ZoneSystem.instance is null)
        {
            Log.LogWarning("Attempting to retrieve location before ZoneSystem is initialized.");
            return null;
        }

        var zoneId = ZoneSystem.instance.GetZone(position);

        if ((ZoneSystem.instance?.m_locationInstances?.Count ?? 0) > 0)
        {
            if (ZoneSystem.instance.m_locationInstances.TryGetValue(zoneId, out ZoneSystem.LocationInstance defaultLocation))
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
}
