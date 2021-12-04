using System.Linq;
using UnityEngine;
using static Heightmap;

namespace Valheim.SpawnThat.Utilities.World;

public class Zone
{
    private BiomeArea? _biomeArea;
    private float[] _oceanDepthCorners;

    public int Width { get; } = 64;

    public Biome Biome { get; }

    public Vector2i ZoneId { get; }

    public Vector3 ZonePos { get; }

    public Biome[] BiomeCorners { get; }

    public BiomeArea BiomeArea => _biomeArea ??= BiomeCorners.All(x => BiomeCorners[0] == x) ? BiomeArea.Median : BiomeArea.Edge;

    private float[] OceanDepthCorners => _oceanDepthCorners ??= CalculateOceanDepths();

    public Zone(Vector2i zoneId)
    {
        ZoneId = zoneId;

        if (ZoneSystem.instance != null)
        {
            ZonePos = ZoneSystem.instance.GetZonePos(ZoneId);
        }
        else
        {
            ZonePos = new Vector3(zoneId.x * Width, 0, zoneId.y * Width);
        }

        BiomeCorners = CalculateCornerBiomes(ZonePos);

        // Check if in the middle of a region, by checking if all biomes are the same.
        var biome = BiomeCorners[0];
        if (biome == BiomeCorners[1] &&
            biome == BiomeCorners[2] &&
            biome == BiomeCorners[3])
        {
            Biome = biome;
        }
        else
        {
            // Simplified lookup.
            Biome = WorldGenerator.instance.GetBiome(ZonePos);
        }
    }

    public bool HasBiome(Biome biome) => BiomeCorners.Any(x => (x & biome) > 0);

    /// <summary>
    /// <para>Gets zones local grid coordinates.</para>
    /// <para>Based on <c>Heightmap.WorldToVertex</c>.</para>
    /// </summary>
    /// <param name="worldCoordinate"></param>
    /// <returns></returns>
    public Vector2i WorldToZoneCoordinate(Vector3 worldCoordinate)
{
        Vector3 vector = worldCoordinate - ZonePos;
        var x = Mathf.FloorToInt(vector.x + 0.5f) + Width / 2;
        var y = Mathf.FloorToInt(vector.z + 0.5f) + Width / 2;

        return new Vector2i(x, y);
    }

    /// <summary>
    /// <para>
    /// Calculates world height at the given point.
    /// </para>
    /// <para>
    /// Based on <c>HeightmapBuilder.Build</c>, but instead of calculating 
    /// a full grid of 4226 heights, only the requested position is calculated.
    /// </para>
    /// </summary>
    public float Height(Vector3 worldCoordinate)
    {
        return Height(WorldToZoneCoordinate(worldCoordinate));
    }

    /// <summary>
    /// <para>
    /// Calculates world height at the local zone coordinates.
    /// </para>
    /// <para>
    /// Based on <c>HeightmapBuilder.Build</c>, but instead of calculating 
    /// a full grid of 65*65 heights, only the requested position is calculated.
    /// </para>
    /// </summary>
    public float Height(Vector2i zoneLocalCoordinate)
    {
        int y = zoneLocalCoordinate.y;
        int x = zoneLocalCoordinate.x;

        Vector3 zonePos = ZonePos + new Vector3(Width * -0.5f, 0f, Width * -0.5f);

        float wy = zonePos.z + y;
        float t = Mathf.SmoothStep(0f, 1f, y / (float)Width);

        float wx = zonePos.x + x;
        float t2 = Mathf.SmoothStep(0f, 1f, x / (float)Width);
        float value;

        WorldGenerator worldGen = WorldGenerator.instance;

        var biome = BiomeCorners[0];
        if (biome == BiomeCorners[1] &&
            biome == BiomeCorners[2] &&
            biome == BiomeCorners[3])
        {
            value = worldGen.GetBiomeHeight(biome, wx, wy);
        }
        else
        {
            float biomeHeight = worldGen.GetBiomeHeight(biome, wx, wy);
            float biomeHeight2 = worldGen.GetBiomeHeight(BiomeCorners[1], wx, wy);
            float biomeHeight3 = worldGen.GetBiomeHeight(BiomeCorners[2], wx, wy);
            float biomeHeight4 = worldGen.GetBiomeHeight(BiomeCorners[3], wx, wy);
            float a = Mathf.Lerp(biomeHeight, biomeHeight2, t2);
            float b = Mathf.Lerp(biomeHeight3, biomeHeight4, t2);
            value = Mathf.Lerp(a, b, t);
        }

        return value;
    }

    /// <summary>
    /// <para>Calculates ocean depth at the coordinate.</para>
    /// <para>Based on <c>HeightmapBuilder.GetOceanDepth</c>.</para>
    /// </summary>
    public float OceanDepth(Vector3 worldCoordinate)
    {
        return OceanDepth(WorldToZoneCoordinate(worldCoordinate));
    }

    /// <summary>
    /// <para>Calculates ocean depth at the local zone coordinate.</para>
    /// <para>Based on <c>Heightmap.GetOceanDepth</c>.</para>
    /// </summary>
    public float OceanDepth(Vector2i zoneLocalCoordinate)
    {
        float t = zoneLocalCoordinate.x / (float)Width;
        float t2 = zoneLocalCoordinate.y / (float)Width;
        float a = Mathf.Lerp(OceanDepthCorners[3], OceanDepthCorners[2], t);
        float b = Mathf.Lerp(OceanDepthCorners[0], OceanDepthCorners[1], t);
        return Mathf.Lerp(a, b, t2);
    }

    /// <summary>
    /// Based on <c>HeightmapBuilder.Build</c> biome corner calculation.
    /// </summary>
    private Biome[] CalculateCornerBiomes(Vector3 pos)
    {
        Vector3 vector = pos + new Vector3(Width * -0.5f, 0f, Width * -0.5f);
        WorldGenerator worldGen = WorldGenerator.instance;
        var cornerBiomes = new Heightmap.Biome[4];
        cornerBiomes[0] = worldGen.GetBiome(vector.x, vector.z);
        cornerBiomes[1] = worldGen.GetBiome(vector.x + Width, vector.z);
        cornerBiomes[2] = worldGen.GetBiome(vector.x, vector.z + Width);
        cornerBiomes[3] = worldGen.GetBiome(vector.x + Width, vector.z + Width);

        return cornerBiomes;
    }

    /// <summary>
    /// Based on <c>Heightmap.UpdateCornerDepths</c>.
    /// </summary>
    /// <returns></returns>
    private float[] CalculateOceanDepths()
    {
        float[] oceanDepth = new float[4];

        float waterLevel = ZoneSystem.instance ? ZoneSystem.instance.m_waterLevel : 30f;

        oceanDepth[0] = Mathf.Max(0, waterLevel - Height(new Vector2i(0, Width)));
        oceanDepth[1] =  Mathf.Max(0, waterLevel - Height(new Vector2i(Width, Width)));
        oceanDepth[2] = Mathf.Max(0, waterLevel - Height(new Vector2i(Width, 0)));
        oceanDepth[3] = Mathf.Max(0, waterLevel - Height(new Vector2i(0, 0)));

        return oceanDepth;
    }
}
