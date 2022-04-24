using UnityEngine;
using static Heightmap;

namespace SpawnThat.World.Zone;

public interface IZone
{
    int Width { get; }

    /// <summary>
    /// Main biome of zone. Calculated using <c>GetBiome(ZonePos)</c>
    /// Note that this means that edge zones may display other biomes.
    /// </summary>
    Biome Biome { get; }
    Vector2i ZoneId { get; }
    Vector3 ZonePos { get; }
    Biome[] BiomeCorners { get; }
    BiomeArea BiomeArea { get; }

    bool HasBiome(Biome biome);

    /// <summary>
    /// <para> Get biome at position. </para>
    /// <para> This is the general biome understanding in valheim, but not the only one.</para>
    /// </summary>
    /// <remarks>
    ///     Valheim is weird... Biome is calculated in a multitude of ways, depending on position. 
    ///     This is the main one, also used by minimap to get current biome name.
    /// </remarks>
    Biome GetBiome(Vector3 pos);

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
