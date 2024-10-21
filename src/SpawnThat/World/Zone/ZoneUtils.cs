using System.Collections.Generic;
using UnityEngine;

namespace SpawnThat.World.Zone;

public static class ZoneUtils
{
    public static List<Vector2i> GetZonesInSquare(int minX, int minZ, int maxX, int maxZ)
    {
        List<Vector2i> sectors = new List<Vector2i>();

        int stepMinX = Zonify(minX);
        int stepMaxX = Zonify(maxX);

        int stepMinZ = Zonify(minZ);
        int stepMaxZ = Zonify(maxZ);

        for (int x = stepMinX; x <= stepMaxX; ++x)
        {
            for (int z = stepMinZ; z <= stepMaxZ; ++z)
            {
                sectors.Add(new Vector2i(x, z));
            }
        }

        return sectors;
    }

    public static Vector2i GetZone(Vector3 pos) => ZoneSystem.GetZone(pos);

    public static Vector2i GetZone(int x, int z)
    {
        return new Vector2i(Zonify(x), Zonify(z));
    }

    public static int GetZoneIndex(Vector2i zone)
    {
        return ZDOMan.instance.SectorToIndex(zone);
    }

    public static int Zonify(int coordinate)
    {
        return Mathf.FloorToInt((coordinate + 32) / 64f);
    }
}
