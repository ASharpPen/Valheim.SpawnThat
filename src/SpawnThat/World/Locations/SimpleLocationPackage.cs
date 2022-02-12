using System;
using System.Collections.Generic;
using SpawnThat.Core;
using SpawnThat.Core.Network;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.World.Locations;

internal class SimpleLocationPackage : CompressedPackage
{
    public string[] LocationNames;

    public SimpleLocationDTO[] Locations;

    protected override void BeforePack()
    {
        var locationInstances = ZoneSystem.instance.IsNotNull()
            ? ZoneSystem.instance.m_locationInstances
            : null;

        Dictionary<string, ushort> locationNameIndexes = new Dictionary<string, ushort>();

        List<string> locationNames = new List<string>();
        List<SimpleLocationDTO> locationDtos = new List<SimpleLocationDTO>();

        foreach (var location in locationInstances)
        {
            ushort nameIndex;

            string locationName = location.Value.m_location.m_prefabName;

            if (locationNameIndexes.TryGetValue(location.Value.m_location.m_prefabName, out ushort index))
            {
                nameIndex = index;
            }
            else
            {
                locationNames.Add(locationName);
                nameIndex = (ushort)(locationNames.Count - 1);
                locationNameIndexes.Add(locationName, nameIndex);
            }

            locationDtos.Add(new SimpleLocationDTO(location.Key, nameIndex));
        }

        LocationNames = locationNames.ToArray();
        Locations = locationDtos.ToArray();

        Log.LogDebug($"Packaged locations: {LocationNames?.Length ?? 0}");
    }

    protected override void AfterUnpack(object obj)
    {
        if (obj is SimpleLocationPackage package)
        {
            Log.LogDebug("Received and deserialized location package");

            List<SimpleLocation> simpleLocations = new List<SimpleLocation>(package.Locations.Length);

            foreach (var location in package.Locations)
            {
                var position = new Vector2i(location.X, location.Y);

                simpleLocations.Add(new SimpleLocation
                {
                    LocationName = package.LocationNames[location.L],
                    Position = ZoneSystem.instance.GetZonePos(position),
                    ZonePosition = position
                });
            }

            LocationManager.SetLocations(simpleLocations);

            Log.LogDebug($"Unpacked locations: {simpleLocations?.Count ?? 0}");

            Log.LogInfo("Successfully unpacked locations.");
        }
        else
        {
            Log.LogWarning("Received bad location package. Unable to load.");
        }
    }
}

[Serializable]
public struct SimpleLocationDTO
{
    public int X;
    public int Y;

    public ushort L;

    public SimpleLocationDTO(Vector2i pos, ushort location)
    {
        X = pos.x;
        Y = pos.y;

        L = location;
    }
}
