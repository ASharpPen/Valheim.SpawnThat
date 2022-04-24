using UnityEngine;
using SpawnThat.Utilities.Extensions;
using static Heightmap;
using SpawnThat.World.Zone;

namespace SpawnThat.World;

/// <summary>
/// Zone using a cached Heightmap as basis.
/// 
/// This class should be cleaned up when heightmap is destroyed.
/// </summary>
internal class ZoneHeightmap : IZone
{
    private Heightmap Heightmap { get; }

    public ZoneHeightmap(Heightmap heightmap)
    {
        Heightmap = heightmap;

        ZonePos = heightmap.transform.position;
        ZoneId = ZonePos.GetZoneId();

        Width = heightmap.m_width;
        Biome = heightmap.GetBiome(ZonePos);
        BiomeCorners = heightmap.GetBiomes().ToArray();
        BiomeArea = heightmap.GetBiomeArea();
    }

    public int Width { get; }
    public Biome Biome { get; }
    public Vector2i ZoneId { get; }
    public Vector3 ZonePos { get; }
    public Biome[] BiomeCorners { get; }
    public BiomeArea BiomeArea { get; }

    public bool HasBiome(Biome biome) => Heightmap.HaveBiome(biome);

    public Biome GetBiome(Vector3 pos)
    {
        return Heightmap.GetBiome(pos);
    }

    public float Height(Vector3 worldCoordinate)
    {
        Heightmap.GetHeight(worldCoordinate, out var height);
        return height;
    }

    public float Height(Vector2i zoneLocalCoordinate) => Heightmap.GetHeight(zoneLocalCoordinate.x, zoneLocalCoordinate.y);

    public float OceanDepth(Vector3 worldCoordinate) => Heightmap.GetOceanDepth(worldCoordinate);

    public float OceanDepth(Vector2i zoneLocalCoordinate)
    {
        var worldCoordinate = Heightmap.CalcVertex(zoneLocalCoordinate.x, zoneLocalCoordinate.y);
        return OceanDepth(worldCoordinate);
    }

    /// <summary>
    /// <inheritdoc/>
    /// <para>Gets surface tilt (in degrees) by raycast.</para>
    /// </summary>
    /// <remarks>Based on ZoneSystem.GetGroundData and SpawnSystem.IsSpawnPointGood.</remarks>
    public float Tilt(Vector3 worldCoordinate)
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(worldCoordinate + Vector3.up * 5000f, Vector3.down, out raycastHit, 10000f, ZoneSystem.instance.m_terrainRayMask))
        {
            var normal = raycastHit.normal;

            // Kinda simplified I guess, but if it works...
            return 90 - Mathf.Asin(normal.y) * Mathf.Rad2Deg;
        }

        return 0;
    }

    public Vector2i WorldToZoneCoordinate(Vector3 worldCoordinate)
    {
        Heightmap.WorldToVertex(worldCoordinate, out var x, out var y);
        return new Vector2i(x, y);
    }
}
