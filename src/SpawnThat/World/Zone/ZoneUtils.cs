using System.Collections.Generic;
using UnityEngine;

namespace SpawnThat.World.Zone;

public static class ZoneUtils
{
    public static List<Vector2s> GetZonesInSquare(int minX, int minZ, int maxX, int maxZ)
    {
        List<Vector2s> sectors = new List<Vector2s>();

        int stepMinX = Zonify(minX);
        int stepMaxX = Zonify(maxX);

        int stepMinZ = Zonify(minZ);
        int stepMaxZ = Zonify(maxZ);

        for (int x = stepMinX; x <= stepMaxX; ++x)
        {
            for (int z = stepMinZ; z <= stepMaxZ; ++z)
            {
                sectors.Add(new Vector2s(x, z));
            }
        }

        return sectors;
    }

    public static Vector2s GetZone(int x, int z)
    {
        return new Vector2s(Zonify(x), Zonify(z));
    }

    public static int Zonify(int coordinate)
    {
        return Mathf.FloorToInt((coordinate + 32) / 64f);
    }
}
