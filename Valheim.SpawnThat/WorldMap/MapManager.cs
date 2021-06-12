using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Valheim.SpawnThat.WorldMap
{
    public static class MapManager
    {
        internal static AreaMap AreaMap { get; set; }

        public static int GetAreaId(Vector3 position)
        {
            int x = Math.Min(AreaMap.MapWidth, Math.Max(0, AreaMap.CoordinateToIndex((int)position.x)));
            int y = Math.Min(AreaMap.MapWidth, Math.Max(0, AreaMap.CoordinateToIndex((int)position.z)));

            return AreaMap.GridIds[x][y];
        }

        public static float GetAreaChance(Vector3 position, int modifier = 0)
        {
            int x = Math.Min(AreaMap.MapWidth, Math.Max(0, AreaMap.CoordinateToIndex((int)position.x)));
            int y = Math.Min(AreaMap.MapWidth, Math.Max(0, AreaMap.CoordinateToIndex((int)position.z)));

            int id = AreaMap.GridIds[x][y];

            System.Random random = new(id + WorldGenerator.instance.GetSeed() + modifier);

            return (float)random.NextDouble();
        }
    }
}
