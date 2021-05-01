using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionMaxSpawned : IConditionOnSpawn
    {
        private static ConditionMaxSpawned _instance;

        public static ConditionMaxSpawned Instance
        {
            get
            {
                return _instance ??= new ConditionMaxSpawned();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration config)
        {
            if (spawn is null || !spawn.m_prefab || spawn.m_prefab is null || config is null)
            {
                return false;
            }

            return !IsValid(spawn.m_prefab, config);
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
