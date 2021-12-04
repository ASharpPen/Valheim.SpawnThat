using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.Utilities.World;

public static class WorldData
{
    private static Dictionary<Vector2i, Zone> ZoneByZoneId { get; set; } = new();

    static WorldData()
    {
        StateResetter.Subscribe(() =>
        {
            ZoneByZoneId = new();
        });
    }

    public static Zone GetZone(Vector3 point)
    {
        return GetZone(point.GetZoneId());
    }

    public static Zone GetZone(Vector2i zoneId)
    {
        if (ZoneByZoneId.TryGetValue(zoneId, out var cached))
        {
            return cached;
        }

        return ZoneByZoneId[zoneId] = new Zone(zoneId);
    }

    private static Vector2i North { get; } = new Vector2i(0, 1);
    private static Vector2i South { get; } = new Vector2i(0, -1);
    private static Vector2i West { get; } = new Vector2i(-1, 0);
    private static Vector2i East { get; } = new Vector2i(1, 0);

    /// <summary>
    /// Calculates surface tilt at point, based on surrounding world height.
    /// </summary>
    /// <remarks>Does not currently take into account terrain changes</remarks>
    /// <remarks>I hate the heightmaps so god damn much.</remarks>
    public static float Tilt(Vector3 point)
    {
        Zone zone = WorldData.GetZone(point);

#if false && DEBUG
        if (Player.m_localPlayer != null)
        {
            Log.LogTrace("ZoneId: " + zone.ZoneId);
            Log.LogTrace("HM ZonePos: " + Heightmap.FindHeightmap(point).transform.position);
            Log.LogTrace("HM ZoneId: " + ZoneSystem.instance.GetZone(Heightmap.FindHeightmap(point).transform.position));
            Log.LogTrace("Real ZoneId: " + ZoneSystem.instance.GetZone(point));
        }
#endif

        var center = zone.WorldToZoneCoordinate(point);
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
        var centerHeight = zone.Height(center);
        var northHeight = zone.Height(north);
        var southHeight = zone.Height(south);
        var westHeight = zone.Height(west);
        var eastHeight = zone.Height(east);

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
