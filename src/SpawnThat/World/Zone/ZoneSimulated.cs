using System.Linq;
using SpawnThat.World.Zone;
using UnityEngine;
using static Heightmap;

namespace SpawnThat.World;

// TODO: Take into account terrain deformations.
internal class ZoneSimulated : IZone
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

    public ZoneSimulated(Vector2i zoneId)
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
    /// <inheritdoc/>
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
    /// <inheritdoc/>
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
    /// <inheritdoc/>
    /// <para>Based on <c>HeightmapBuilder.GetOceanDepth</c>.</para>
    /// </summary>
    public float OceanDepth(Vector3 worldCoordinate)
    {
        return OceanDepth(WorldToZoneCoordinate(worldCoordinate));
    }

    /// <summary>
    /// <inheritdoc/>
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
        oceanDepth[1] = Mathf.Max(0, waterLevel - Height(new Vector2i(Width, Width)));
        oceanDepth[2] = Mathf.Max(0, waterLevel - Height(new Vector2i(Width, 0)));
        oceanDepth[3] = Mathf.Max(0, waterLevel - Height(new Vector2i(0, 0)));

        return oceanDepth;
    }

    private static Vector2i North { get; } = new Vector2i(0, 1);
    private static Vector2i South { get; } = new Vector2i(0, -1);
    private static Vector2i West { get; } = new Vector2i(-1, 0);
    private static Vector2i East { get; } = new Vector2i(1, 0);

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <remarks>Does not currently take into account terrain changes</remarks>
    /// <remarks>I hate the heightmaps so god damn much.</remarks>
    public float Tilt(Vector3 worldCoordinate)
    {
        var point = WorldToZoneCoordinate(worldCoordinate);

#if false && DEBUG
        if (Player.m_localPlayer != null)
        {
            Log.LogTrace("ZoneId: " + zone.ZoneId);
            Log.LogTrace("HM ZonePos: " + Heightmap.FindHeightmap(point).transform.position);
            Log.LogTrace("HM ZoneId: " + ZoneSystem.instance.GetZone(Heightmap.FindHeightmap(point).transform.position));
            Log.LogTrace("Real ZoneId: " + ZoneSystem.instance.GetZone(point));
        }
#endif

        var center = point;
        var north = center + North;
        var south = center + South;
        var west = center + West;
        var east = center + East;

#if false && DEBUG
        if (Player.m_localPlayer != null)
        {
            Log.LogTrace("Built in Tilt: ");
            Log.LogTrace("\t" + Heightmap.FindHeightmap(point).CalcNormal(center.x, center.y));
            Log.LogTrace("\t" + Vector3.Angle(Vector3.up, Heightmap.FindHeightmap(point).CalcNormal(center.x, center.y)));


            Log.LogTrace("WorldToZoneCoordinate: " + center);
            Heightmap.FindHeightmap(point).WorldToVertex(point, out var wtvX, out var wtvY);
            Log.LogTrace("WorldToVertex: " + new Vector2i(wtvX, wtvY));

            Log.LogTrace("Original index in BaseHeights");
            Log.LogTrace($"\tY: {wtvY} * {65} = {wtvY * 65}");
            Log.LogTrace($"\tX: {wtvX}");
            Log.LogTrace($"\t == {wtvY * 65 + wtvX}");

            var hm = Heightmap.FindHeightmap(point);
            var modifiers =
                TerrainModifier.GetAllInstances()
                .Where(x => x.enabled && hm.TerrainVSModifier(x))
                .ToList();

            Log.LogTrace("Terrain Modifiers: " + modifiers.Count);

            modifiers = modifiers
                .Where(x =>
                {
                    var modPos = x.transform.position + Vector3.up * x.m_levelOffset;
                    if (x.m_level)
                    {
                        return Vector3.Distance(modPos, point) <= x.m_levelRadius;
                    }
                    else if(x.m_smooth)
                    {
                        return Vector3.Distance(modPos, point) <= x.m_smoothRadius;
                    }

                    return false;
                })
                .ToList();

            foreach(var modifier in modifiers)
            {
                var modPos = modifier.transform.position + Vector3.up * modifier.m_levelOffset;
                Log.LogTrace("\t" + (modPos - modifier.transform.position));
            }
        }
#endif
        var centerHeight = Height(center);
        var northHeight = Height(north);
        var southHeight = Height(south);
        var westHeight = Height(west);
        var eastHeight = Height(east);

#if false && DEBUG
        Log.LogTrace("Heights:");
        Log.LogTrace($"\tC: " + zone.Height(center));
        Log.LogTrace($"\tN: " + northHeight);
        Log.LogTrace($"\tS: " + southHeight);
        Log.LogTrace($"\tW: " + westHeight);
        Log.LogTrace($"\tE: " + eastHeight);

        if (Player.m_localPlayer != null)
        {
            Log.LogTrace("WorldToVertex:");
            Log.LogTrace($"\tScale: " + Heightmap.FindHeightmap(point).m_scale);
            Log.LogTrace($"\tWidth: " + Heightmap.FindHeightmap(point).m_width);
            Log.LogTrace($"\tY: " + Heightmap.FindHeightmap(point).transform.position.y);

            Log.LogTrace("Original heights:");
            {
                Heightmap.GetHeight(point, out var centerHeight);
                Heightmap.GetHeight(point + new Vector3(0, 2), out var nHeight);
                Heightmap.GetHeight(point + new Vector3(0, -2), out var sHeight);
                Heightmap.GetHeight(point + new Vector3(-2, 0), out var wHeight);
                Heightmap.GetHeight(point + new Vector3(2, 0), out var eHeight);

                Log.LogTrace("\tC: " + centerHeight);
                Log.LogTrace("\tN: " + nHeight);
                Log.LogTrace("\tS: " + sHeight);
                Log.LogTrace("\tW: " + wHeight);
                Log.LogTrace("\tE: " + eHeight);
            }

            Log.LogTrace("Original Base heights:");
            {
                float cHeight = Heightmap.FindHeightmap(point).GetBaseHeight(center.x, center.y);
                float nHeight = Heightmap.FindHeightmap(point).GetBaseHeight(north.x, north.y);
                float sHeight = Heightmap.FindHeightmap(point).GetBaseHeight(south.x, south.y);
                float wHeight = Heightmap.FindHeightmap(point).GetBaseHeight(west.x, west.y);
                float eHeight = Heightmap.FindHeightmap(point).GetBaseHeight(east.x, east.y);

                Log.LogTrace("\tC: " + cHeight);
                Log.LogTrace("\tN: " + nHeight);
                Log.LogTrace("\tS: " + sHeight);
                Log.LogTrace("\tW: " + wHeight);
                Log.LogTrace("\tE: " + eHeight);
            }
        }
#endif

        var toNorth = (new Vector3(North.x, northHeight - centerHeight, North.y));
        var toSouth = (new Vector3(South.x, southHeight - centerHeight, South.y));
        var toWest = (new Vector3(West.x, westHeight - centerHeight, West.y));
        var toEast = (new Vector3(East.x, eastHeight - centerHeight, East.y));

#if false && DEBUG
        Log.LogTrace($"North: " + toNorth);
        Log.LogTrace($"South: " + toSouth);
        Log.LogTrace($"West : " + toWest);
        Log.LogTrace($"East : " + toEast);
#endif

        toNorth = toNorth.normalized;
        toSouth = toSouth.normalized;
        toWest = toWest.normalized;
        toEast = toEast.normalized;

#if false && DEBUG
        Log.LogTrace($"North: " + toNorth);
        Log.LogTrace($"South: " + toSouth);
        Log.LogTrace($"West : " + toWest);
        Log.LogTrace($"East : " + toEast);
#endif
        var angleNorth = AngleByCross(toNorth, toEast);
        var angleEast = AngleByCross(toEast, toSouth);
        var angleSouth = AngleByCross(toSouth, toWest);
        var angleWest = AngleByCross(toWest, toNorth);

        float AngleByCross(Vector3 p1, Vector3 p2)
        {
            var cross = Vector3.Cross(p1, p2);

            return Mathf.Abs(Vector3.Angle(Vector2.up, cross));
        }

#if false && DEBUG
        Log.LogTrace($"North: " + angleNorth);
        Log.LogTrace($"South: " + angleSouth);
        Log.LogTrace($"West : " + angleWest);
        Log.LogTrace($"East : " + angleEast);
#endif

        var avgAngle = (angleNorth + angleSouth + angleWest + angleEast) / 4;

        return avgAngle;
    }
}
