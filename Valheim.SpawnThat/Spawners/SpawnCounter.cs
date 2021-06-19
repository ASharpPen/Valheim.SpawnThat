using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners
{
    public class SpawnCounter
    {
        private Vector3 Center { get; }
        private int Range { get; }
        private Dictionary<int, int> CachedPrefabResults = new();

        private List<Vector2i> sectors;
        private List<ZDO> zdos;

        private int MinX;
        private int MinZ;
        private int MaxX;
        private int MaxZ;

        private bool initialized;

        public SpawnCounter(Vector3 center, int range)
        {
            Center = center;
            Range = range;
        }

        private void Initialize()
        {
            if(initialized)
            {
                return;
            }

            (MinX, MaxX) = GetRange((int)Center.x, Range);
            (MinZ, MaxZ) = GetRange((int)Center.z, Range);

            // Get sectors to check
            sectors = GetSectors(MinX, MinZ, MaxX, MaxZ);

#if DEBUG
            Log.LogTrace($"Counting in {sectors.Count} sectors.");
#endif

            // Get zdo's
            zdos = new List<ZDO>();

            foreach (var sector in sectors)
            {
                ZDOMan.instance.FindObjects(sector, zdos);
            }

#if DEBUG
            Log.LogTrace($"Found {zdos.Count} zdos.");
#endif

            initialized = true;
        }

        public bool HasAnyInRange(GameObject prefab)
        {
            Initialize();

            int prefabId = ZNetScene.instance.GetPrefabHash(prefab);

            if (CachedPrefabResults.TryGetValue(prefabId, out int cachedCount))
            {
                return cachedCount > 0;
            }

            return zdos.Any(x => WithinRange(x, prefabId));
        }

        
        public int CountInstancesInRange(GameObject prefab)
        {
            Initialize();

            int prefabId = ZNetScene.instance.GetPrefabHash(prefab);

            if (CachedPrefabResults.TryGetValue(prefabId, out int cachedCount))
            {
                return cachedCount;
            }

#if DEBUG
            Log.LogTrace($"Testing prefab hash: {prefabId}");
#endif

            int instances = 0;

#if DEBUG
            Log.LogTrace($"Checking {zdos.Count} zdos for prefab.");
#endif

            // Search zdo's with same prefab
            instances = zdos.Count(x => WithinRange(x, prefabId));

            CachedPrefabResults[prefabId] = instances;

#if DEBUG
            Log.LogTrace($"Found {instances} entities in range");
#endif

            return instances;

#if FALSE
            // May want to look into parallelizing the task, if more speed is needed.
            void CountParallel()
            {
                // Search zdo's with same prefab
                Parallel.ForEach(zdos, zdo =>
                {
                    if (zdo.m_prefab != prefabId)
                    {
                        return;
                    }

                    // Check if within manhattan distance.
                    if (zdo.m_position.x < minX || zdo.m_position.x > maxX)
                    {
                        return;
                    }

                    if (zdo.m_position.z < minZ || zdo.m_position.z > maxZ)
                    {
                        return;
                    }

                    if (zdo.m_position.y < minY || zdo.m_position.y > minY)
                    {
                        return;
                    }

                    // Check if within distance
                    if (Vector3.Distance(zdo.m_position, center) < range)
                    {
                        ++instances;
                    }
                });
            }
#endif
        }

        private bool WithinRange(ZDO zdo, int prefabId)
        {
            if (zdo.m_prefab != prefabId)
            {
                return false;
            }

            // Check if within manhattan distance.
            if (zdo.m_position.x < MinX || zdo.m_position.x > MaxX)
            {
                return false;
            }

            if (zdo.m_position.z < MinZ || zdo.m_position.z > MaxZ)
            {
                return false;
            }

            // Check if within circle distance
            return Utils.DistanceXZ(zdo.m_position, Center) <= Range;
        }

        private static (int min, int max) GetRange(int center, int range)
        {
            return (center - range, center + range);
        }

        private static List<Vector2i> GetSectors(int minX, int minZ, int maxX, int maxZ)
        {
            List<Vector2i> sectors = new List<Vector2i>();

            int stepMinX = Zonify(minX);
            int stepMaxX = Zonify(maxX);

            int stepMinZ = Zonify(minZ);
            int stepMaxZ = Zonify(maxZ);

            for (int x = stepMinX; x <= stepMaxX; ++x)
            {
                for (int z = stepMinZ; z <= stepMaxZ; ++z)
                {
                    sectors.Add(new Vector2i(x, z));
                }
            }

            return sectors;

            int Zonify(int coordinate)
            {
                return Mathf.FloorToInt((coordinate + 32) / 64f);
            }
        }
    }
}
