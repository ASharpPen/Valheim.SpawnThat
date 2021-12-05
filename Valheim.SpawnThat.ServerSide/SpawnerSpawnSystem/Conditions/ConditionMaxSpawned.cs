using UnityEngine;
using Valheim.SpawnThat.Core.Cache;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;
using Valheim.SpawnThat.ServerSide.Utilities;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionMaxSpawned : ISpawnCondition
    {
        public int Max { get; }

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

            // TODO: Not the best solution. Need to figure out a better way to deal with the templates and their variations.
            if (template is DefaultSpawnSystemTemplate defaultTemplate)
            {
                var entityCounter = context.EntityAreaCounter;

                GameObject prefab = ZNetScene.instance.GetPrefab(defaultTemplate.PrefabHash);

                // Check if we are dealing with a creature. If so, don't count tamed.
                int count = ComponentCache.GetComponent<Character>(prefab) is not null
                    ? entityCounter.CountEntitiesInRange(defaultTemplate.PrefabHash, x => !x.GetTamed())
                    : entityCounter.CountEntitiesInRange(defaultTemplate.PrefabHash);

                return count <= Max;
            }

            return true;
        }
    }
}
