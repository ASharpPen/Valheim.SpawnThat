using UnityEngine;
using static Heightmap;

namespace Valheim.SpawnThat.World;

public interface IZone
{
    int Width { get; }
    Biome Biome { get; }
    Vector2i ZoneId { get; }
    Vector3 ZonePos { get; }
    Biome[] BiomeCorners { get; }
    BiomeArea BiomeArea { get; }

    bool HasBiome(Biome biome);

    /// <summary>
    /// <para>Transform world coordinate to coordinate relative to the zone.</para>
    /// </summary>
    Vector2i WorldToZoneCoordinate(Vector3 worldCoordinate);

    /// <summary>
    /// World height at the given point.
    /// </summary>
    public float Height(Vector3 worldCoordinate);

    /// <summary>
    /// World height at the local zone coordinates.
    /// </summary>
    float Height(Vector2i zoneLocalCoordinate);

    /// <summary>
    /// <para>Ocean depth at the coordinate.</para>
    /// </summary>
    float OceanDepth(Vector3 worldCoordinate);

    /// <summary>
    /// <para>Ocean depth at the local zone coordinate.</para>
    /// </summary>
    float OceanDepth(Vector2i zoneLocalCoordinate);

    /// <summary>
    /// Surface tilt in degrees at point.
    /// </summary>
    float Tilt(Vector3 worldCoordinate);
}
