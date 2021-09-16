using UnityEngine;
using Valheim.SpawnThat.Core.Cache;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.Utilities;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionMaxSpawned : ISpawnCondition
    {
        private int Max { get; }

        public ConditionMaxSpawned(int max)
        {
            Max = max;
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (Max < 1)
            {
                return false;
            }

            var entityCounter = context.EntityAreaCounter;

            GameObject prefab = ZNetScene.instance.GetPrefab(template.PrefabHash);

            // Check if we are dealing with a creature. If so, don't count tamed.
            int count = ComponentCache.GetComponent<Character>(prefab) is not null
                ? entityCounter.CountEntitiesInRange(template.PrefabHash, x => !x.GetTamed())
                : entityCounter.CountEntitiesInRange(template.PrefabHash);

            return count <= Max;
        }
    }
}
