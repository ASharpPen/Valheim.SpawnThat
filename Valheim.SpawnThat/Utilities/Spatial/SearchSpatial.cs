using System;
using System.Collections.Generic;
using UnityEngine;

namespace Valheim.SpawnThat.Utilities
{
    public static class SearchSpatial
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

#if DEBUG

            //Log.LogDebug($"Indexes: {rangeFromX}, {rangeToX}");

            //Log.LogDebug("Checking rooms: " + RoomList.GetRange(rangeFromX, Math.Max(0, rangeToX - rangeFromX)).Select(x => x.Pos).Join());
#endif


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
    }
}
