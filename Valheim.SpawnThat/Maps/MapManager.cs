using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.WorldMap
{
    public static class MapManager
    {
        internal static AreaMap AreaMap { get; set; }

        public static void Initialize()
        {
            AreaMap = new AreaMap(new WorldGeneratorAreaProvider(), 10000);
            AreaMap.Complete();
        }

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

        public static float[][] GetTemplateAreaChanceMap(int templateIndex, int scaling = 1)
        {
            float[][] heatmap = new float[AreaMap.MapWidth][];

            Dictionary<int, float> chanceById = new();

            for(int x = 0; x < AreaMap.MapWidth; ++x)
            {
                heatmap[x] = new float[AreaMap.MapWidth];

                for (int y = 0; y < AreaMap.MapWidth; ++y)
                {
                    int id = AreaMap.GridIds[x][y];

                    if(chanceById.ContainsKey(id))
                    {
                        heatmap[x][y] = chanceById[id] * scaling;
                    }
                    else
                    {
                        System.Random rnd = new System.Random(id + WorldGenerator.instance.GetSeed() + templateIndex);
                        chanceById[id] = (float)rnd.NextDouble();
                    }
                }
            }

            return heatmap;
        }

        public static int[][] GetSpawnMap(int templateIndex)
        {
            float[][] chanceMap = GetTemplateAreaChanceMap(templateIndex, scaling: 100);
            int[][] spawnMap = new int[chanceMap.Length][];

            //SpawnSystem config is only expected to have a single first layer, namely "WorldSpawner", so we just grab the first entry.
            var spawnSystemConfigs = ConfigurationManager
                .SpawnSystemConfig?
                .Subsections? //[*]
                .Values?
                .FirstOrDefault();

            if(!spawnSystemConfigs.Subsections.TryGetValue(templateIndex.ToString(), out SpawnConfiguration config))
            {
                Log.LogWarning($"Unable to find config with index '{templateIndex}'");
                return null;
            }

            for (int x = 0; x < spawnMap.Length; ++x)
            {
                spawnMap[x] = new int[spawnMap.Length];

                for (int y = 0; y < spawnMap.Length; ++y)
                {
                    var allowedBiomes = config.ExtractBiomeMask();

                    if ((MapManager.AreaMap.Biomes[x][y] & (int)allowedBiomes) == 0)
                    {
                        continue;
                    }

                    if (chanceMap[x][y] <= config.ConditionSpawnChanceInArea.Value)
                    {
                        spawnMap[x][y] = 255;
                    }
                }
            }

            return spawnMap;
        }
    }
}
