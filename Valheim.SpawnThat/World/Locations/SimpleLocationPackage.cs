using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Network;
using Valheim.SpawnThat.World.Locations;

namespace Valheim.SpawnThat.Locations;

[Serializable]
internal class SimpleLocationPackage : CompressedPackage
{
    public string[] LocationNames;

    public SimpleLocationDTO[] Locations;

    protected override void BeforePack()
    {
        var locationInstances = ZoneSystem.instance.m_locationInstances;

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
                var position = new Vector2i(location.PositionX, location.PositionY);

                simpleLocations.Add(new SimpleLocation
                {
                    LocationName = package.LocationNames[location.Location],
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
    public int PositionX;
    public int PositionY;

    public ushort Location;

    public SimpleLocationDTO(Vector2i pos, ushort location)
    {
        PositionX = pos.x;
        PositionY = pos.y;

        Location = location;
    }
}
