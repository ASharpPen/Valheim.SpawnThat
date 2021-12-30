using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionMaxSpawned : IConditionOnSpawn
    {
        private static ConditionMaxSpawned _instance;

        public static ConditionMaxSpawned Instance => _instance ??= new();

        public bool ShouldFilter(SpawnConditionContext context)
        {
            return !IsValid(context.SpawnData.m_prefab, context.Config);
        }

        public bool IsValid(GameObject spawn, SpawnConfiguration config)
        {
            var count = SpawnSystem.GetNrOfInstances(spawn, Vector3.zero, 0f, false, false);

            if (count > config.MaxSpawned.Value)
            {
                return true;
            }

            return false;
        }
    }
}
