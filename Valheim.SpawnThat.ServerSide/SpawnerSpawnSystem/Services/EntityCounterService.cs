using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Services
{
    internal class EntityCounterService : IEntityCounterService
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

        public EntityCounterService(Vector3 center, int range)
        {
            Center = center;
            Range = range;
        }

        private void Initialize()
        {
            if (initialized)
            {
                return;
            }

            (MinX, MaxX) = GetRange((int)Center.x, Range);
            (MinZ, MaxZ) = GetRange((int)Center.z, Range);

            // Get sectors to check
            sectors = GetSectors(MinX, MinZ, MaxX, MaxZ);

            // Get zdo's
            zdos = new List<ZDO>();

            foreach (var sector in sectors)
            {
                ZDOMan.instance.FindObjects(sector, zdos);
            }

            initialized = true;
        }

        public int CountEntitiesInRange(int prefabHash, Func<ZDO, bool> condition = null)
        {
            Initialize();

            if (CachedPrefabResults.TryGetValue(prefabHash, out int cachedCount))
            {
                return cachedCount;
            }

            int instances = 0;

            // Search zdo's with same prefab
            if (condition is null)
            {
                instances = zdos.Count(x => WithinRange(x, prefabHash));
            }
            else
            {
                instances = zdos.Count(x => 
                    WithinRange(x, prefabHash) &&
                    condition(x));
            }

            CachedPrefabResults[prefabHash] = instances;

            return instances;
        }

        public bool HasAnyInRange(int prefabHash)
        {
            Initialize();

            if (CachedPrefabResults.TryGetValue(prefabHash, out int cachedCount))
            {
                return cachedCount > 0;
            }

            return zdos.Any(x => WithinRange(x, prefabHash));
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
