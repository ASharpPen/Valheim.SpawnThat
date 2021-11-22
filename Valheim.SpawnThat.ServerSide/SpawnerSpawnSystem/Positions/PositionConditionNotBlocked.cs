using UnityEngine;
using Valheim.SpawnThat.Utilities.Spatial;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    /// <summary>
    /// Simplified blocking check.
    /// Will simply check if any zdo with prefab in the blocking layer 
    /// is within range 4 of position.
    /// </summary>
    public class PositionConditionNotBlocked : ISpawnPositionCondition
    {
        private int blockingRange;

        public PositionConditionNotBlocked(int defaultBlockingRange = 4)
        {
            blockingRange = defaultBlockingRange;
        }

        public bool IsValid(PositionContext context)
        {
            var query = new BlockingZdoQuery(context.Point, blockingRange);

            return !query.AnyBlocking();
        }

        private class BlockingZdoQuery : ZdoQuery
        {
            public BlockingZdoQuery(Vector3 center, int range) : base(center, range)
            {
            }

            public bool AnyBlocking()
            {
                foreach(var zdo in Zdos)
                {
                    // Filter for nearby zdos.
                    if (!IsWithinRangeManhattan(zdo))
                    {
                        continue;
                    }

                    var prefabHash = zdo.GetPrefab();

                    var prefab = ZNetScene.instance.GetPrefab(prefabHash);

                    if (prefab is null)
                        continue;

                    var blockingLayer = ZoneSystem.instance.m_blockRayMask;

                    // Check if prefab can block
                    if ((prefab.layer & blockingLayer) == 0)
                        continue;

                    return true;
                }

                return false;
            }
        }
    }
}
