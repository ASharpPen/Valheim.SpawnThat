using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.Spawners.Caches;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Position;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Managers
{
    public static class SpawnPositionManager
    {
        private static HashSet<ISpawnPositionCondition> PositionConditions = new();

        static SpawnPositionManager()
        {
            StateResetter.Subscribe(InitPositionConditions);

            InitPositionConditions();

            void InitPositionConditions()
            {
                PositionConditions = new();
                PositionConditions.Add(PositionConditionLocation.Instance);
            }
        }

        public static bool ShouldFilter(SpawnSystem.SpawnData spawn, Vector3 position)
        {
            var config = SpawnDataCache.Get(spawn);

            if (config?.Config is null)
            {
                return false;
            }

            foreach (var condition in PositionConditions)
            {
                try
                {
                    if(condition is not null && condition.ShouldFilter(spawn, config.Config, position))
                    {
                        return true;
                    }
                }
                catch(Exception e)
                {
                    Log.LogError($"Error while attempting to check valid spawn position for condition '{condition.GetType().Name}'.", e);
                }
            }

            return false;
        }
    }
}
