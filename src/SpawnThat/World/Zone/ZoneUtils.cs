using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace SpawnThat.World.Zone;

public static class ZoneUtils
{
    internal const int ZoneWidth = WorldGenerator.c_BiomeAreaI;
    internal const double ZoneHalfWidth = WorldGenerator.c_BiomeAreaI / 2;

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

    /// <summary>
    /// Zones are centered around [0,0], with a radius of ZoneHalfWidth.
    /// </summary>
    public static int Zonify(int coordinate) =>
        (int)Math.Floor((coordinate + ZoneHalfWidth) / ZoneWidth);

    public static int ZoneToWorld(int zonePos) => 
        zonePos * ZoneWidth;
}
