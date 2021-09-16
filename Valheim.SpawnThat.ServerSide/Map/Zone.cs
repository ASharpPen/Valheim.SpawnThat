using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Heightmap;

namespace Valheim.SpawnThat.ServerSide.Map
{
    public class Zone
    {
        private BiomeArea? _biomeArea;

        public Biome Biome { get; }

        public Vector2i ZoneId { get; }

        public Vector3 ZonePos { get; }

        private HeightmapBuilder.HMBuildData HeightmapData { get; }

        public Biome[] BiomeCorners => HeightmapData.m_cornerBiomes;

        public BiomeArea BiomeArea => _biomeArea ??= BiomeCorners.All(x => BiomeCorners[0] == x) ? BiomeArea.Median : BiomeArea.Edge;


        public Zone(Vector2i zoneId)
        {
            ZoneId = zoneId;
            ZonePos = ZoneSystem.instance.GetZonePos(ZoneId);

            HeightmapData = Generate();

            // Check if in the middle of a region, by checking if all biomes are the same.
            var biome = HeightmapData.m_cornerBiomes[0];
            if (biome == HeightmapData.m_cornerBiomes[1] &&
                biome == HeightmapData.m_cornerBiomes[2] &&
                biome == HeightmapData.m_cornerBiomes[3])
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

        private HeightmapBuilder.HMBuildData Generate()
        {
            // Simulate HeightMapBuilder Build without the unnecessary height data.

            int width = 64; // Heightmap.m_width

            var heightmapData = new HeightmapBuilder.HMBuildData(ZonePos, width, 1, false, WorldGenerator.instance);
            HeightmapBuilder.instance.Build(heightmapData);

            return heightmapData;
        }
    }
}
