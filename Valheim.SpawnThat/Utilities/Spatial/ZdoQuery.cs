using System.Collections.Generic;
using UnityEngine;

namespace Valheim.SpawnThat.Utilities.Spatial
{
    public abstract class ZdoQuery
    {
        protected Vector3 Center { get; }
        protected int Range { get; }

        protected List<Vector2i> ZoneIds { get; private set; }
        protected List<ZDO> Zdos { get; private set; }

        protected int MinX { get; private set; }
        protected int MinZ { get; private set; }
        protected int MaxX { get; private set; }
        protected int MaxZ { get; private set; }

        private bool initialized;

        /// <summary>
        /// Prepares for querying zdo's within the zones 
        /// indicated by the center and range.
        /// Selects all ZDO's and ZoneId's using the square formed
        /// by the input, and caches them for subsequent queries.
        /// </summary>
        protected ZdoQuery(Vector3 center, int range)
        {
            Center = center;
            Range = range;

            Initialize();
        }

        protected virtual void Initialize()
        {
            if (initialized)
            {
                return;
            }

            (MinX, MaxX) = GetRange((int)Center.x, Range);
            (MinZ, MaxZ) = GetRange((int)Center.z, Range);

            // Get zones to check
            ZoneIds = ZoneUtils.GetZones(MinX, MinZ, MaxX, MaxZ);

            // Get zdo's
            Zdos = new List<ZDO>();

            foreach (var zone in ZoneIds)
            {
                ZDOMan.instance.FindObjects(zone, Zdos);
            }

            initialized = true;
        }

        protected static (int min, int max) GetRange(int center, int range)
        {
            return (center - range, center + range);
        }

        protected bool IsWithinRangeManhattan(ZDO zdo)
        {
            // Check if within manhattan distance.
            if (zdo.m_position.x < MinX || zdo.m_position.x > MaxX)
            {
                return false;
            }

            if (zdo.m_position.z < MinZ || zdo.m_position.z > MaxZ)
            {
                return false;
            }

            return true;
        }

        protected bool IsWithinRange(ZDO zdo)
        {
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
    }
}
