using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnThat.Utilities.Spatial;

internal static class SearchSpatial
{
    public static T FindClosest<T>(this IList<T> points, Vector3 pos) where T : class, IHaveVector3, IPoint
    {
        if ((points?.Count ?? 0) == 0)
        {
            return null;
        }

        T closest = null;
        float closestDistance = float.MaxValue;

        //Get range + a bit to the left.
        int rangeFromX = points.IndexLeft(pos.x - 20);

        //Get range + a bit to the right.
        int rangeToX = points.IndexRight(pos.x + 20);

        for (int i = rangeFromX; i <= rangeToX && i < points.Count; ++i)
        {
            var room = points[i];

            //Skip if way above/below room.
            if (Math.Abs(room.Pos.y - pos.y) > 100)
            {
                continue;
            }

            var distX = pos.x - room.Pos.x;
            var distZ = pos.z - room.Pos.z;

            var distance = Math.Abs(distX) + Math.Abs(distZ);

            if (distance < closestDistance)
            {
                closest = room;
                closestDistance = distance;
            }
        }

        return closest;
    }

    public static bool Contains(IBox box, Vector3 pos)
    {
        var polygon = CreateRectangle(box);

        return Contains(polygon, pos);
    }

    /// <summary>
    /// Based on: https://math.stackexchange.com/a/190373
    /// </summary>
    public static bool Contains(Vector2[] rectangle, Vector3 pos)
    {
        var a = rectangle[0];
        var b = rectangle[1];
        var d = rectangle[3];

        var m = new Vector2(pos.x, pos.z);

        var ab = b - a;
        var ad = d - a;
        var am = m - a;

        var d1 = Vector2.Dot(am, ab);
        var d2 = Vector2.Dot(ab, ab);
        var d3 = Vector2.Dot(am, ad);
        var d4 = Vector2.Dot(ad, ad);

        if (0 < d1 &&
            d1 < d2 &&
            0 < d3 &&
            d3 < d4)
        {
            return true;
        }

        return false;
    }

    public static Vector2[] CreateRectangle(IBox box)
    {
        var lt_x = box.Pos.x - box.Size.x;
        var lt_z = box.Pos.z + box.Size.z;

        var rt_x = box.Pos.x + box.Size.x;
        var rt_z = box.Pos.z +box.Size.z;

        var rb_x = box.Pos.x + box.Size.x;
        var rb_z = box.Pos.z - box.Size.z;

        var lb_x = box.Pos.x - box.Size.x;
        var lb_z = box.Pos.z - box.Size.z;

        var lt = new Vector2(lt_x, lt_z);
        var rt = new Vector2(rt_x, rt_z);
        var rb = new Vector2(rb_x, rb_z);
        var lb = new Vector2(lb_x, lb_z);

        return [
            Pivot(lt),
            Pivot(rt),
            Pivot(rb),
            Pivot(lb),
            ];

        Vector2 Pivot(Vector2 point)
        {
            var p = new Vector3(point.x, box.Pos.y, point.y);
            var direction = p - box.Pos;

            var rotation = box.Rotation * direction;

            var newPos = rotation + box.Pos;

            return new Vector2(newPos.x, newPos.z);
        }
    }
}
