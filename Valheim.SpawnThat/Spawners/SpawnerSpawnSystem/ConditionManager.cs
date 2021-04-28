using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions.ModSpecific;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem
{
    public class ConditionManager
    {
        private HashSet<IConditionOnAwake> OnAwakeConditions = new HashSet<IConditionOnAwake>();
        private HashSet<IConditionOnSpawn> OnSpawnConditions = new HashSet<IConditionOnSpawn>();

        private static ConditionManager _instance;

        public static ConditionManager Instance
        {
            get
            {
                return _instance ??= new ConditionManager();
            }
        }

        ConditionManager()
        {
            StateResetter.Subscribe(() =>
            {
                _instance = null;
            });

            #region Add OnAwake
            
            OnAwakeConditions.Add(ConditionDistanceToCenter.Instance);

            #endregion

            #region Add OnSpawn

            OnSpawnConditions.Add(ConditionWorldAge.Instance);
            OnSpawnConditions.Add(ConditionGlobalKeys.Instance);
            OnSpawnConditions.Add(ConditionNearbyPlayersCarryValue.Instance);
            OnSpawnConditions.Add(ConditionNearbyPlayersCarryItem.Instance);

            OnSpawnConditions.Add(ConditionLoaderCLLC.ConditionWorldLevel);

            #endregion
        }

        public bool FilterOnAwake(SpawnSystem spawner, SpawnConfiguration config)
        {
            return OnAwakeConditions.Any(x =>
            {
                try
                {
                    return x?.ShouldFilter(spawner, config) ?? false;
                }
                catch(Exception e)
                {
                    Log.LogError($"Error while attempting to check OnAwake condition {x.GetType().Name}.", e);
                    return false;
                }
            });
        }

        public bool FilterOnSpawn(SpawnSystem spawner, SpawnSystem.SpawnData spawn)
        {
            var cache = SpawnSystemConfigCache.Get(spawn);

            if (cache?.Config == null)
            {
                return false;
            }

            return OnSpawnConditions.Any(x =>
            {
                try
                {
                    return x?.ShouldFilter(spawner, spawn, cache.Config) ?? false;
                }
                catch(Exception e)
                {
                    Log.LogError($"Error while attempting to check OnSpawn condition {x.GetType().Name}.", e);
                    return false;
                }
            });
        }
    }
}
