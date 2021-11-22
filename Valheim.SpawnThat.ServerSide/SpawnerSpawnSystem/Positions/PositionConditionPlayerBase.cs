using UnityEngine;
using Valheim.SpawnThat.ServerSide.Utilities;
using Valheim.SpawnThat.Utilities.Spatial;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    public class PositionConditionPlayerBase : ISpawnPositionCondition
    {
        private int Distance { get; }

        public PositionConditionPlayerBase(int distance = 20)
        {
            Distance = distance;
        }

        public bool IsValid(PositionContext context)
        {
            var query = new PlayerBaseZdoQuery(context.Point, Distance);

            return !query.AnyBlocking();
        }

        private class PlayerBaseZdoQuery : ZdoQuery
        {
            public PlayerBaseZdoQuery(Vector3 center, int range) : base(center, range)
            {
            }

            public bool AnyBlocking()
            {
                foreach (var zdo in Zdos)
                {
                    // Filter for nearby zdos.
                    if (!IsWithinRange(zdo))
                    {
                        continue;
                    }

                    var isPlayerBase = PrefabData.IsPlayerBase(zdo.GetPrefab());

                    if (isPlayerBase)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
