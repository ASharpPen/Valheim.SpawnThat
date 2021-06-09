using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions.ModSpecific;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Managers
{
    public class SpawnConditionManager
    {
        private HashSet<IConditionOnAwake> OnAwakeConditions = new HashSet<IConditionOnAwake>();
        private HashSet<IConditionOnSpawn> OnSpawnConditions = new HashSet<IConditionOnSpawn>();
        private HashSet<IConditionOnSpawn> DefaultSpawnConditions = new HashSet<IConditionOnSpawn>();

        private static SpawnConditionManager _instance;

        public static SpawnConditionManager Instance
        {
            get
            {
                return _instance ??= new SpawnConditionManager();
            }
        }

        SpawnConditionManager()
        {
            StateResetter.Subscribe(() =>
            {
                _instance = null;
            });

            // OnAwake conditions

            OnAwakeConditions.Add(ConditionDistanceToCenter.Instance);
            OnAwakeConditions.Add(ConditionLocation.Instance);

            // OnSpawn conditions

            OnSpawnConditions.Add(ConditionWorldAge.Instance);
            OnSpawnConditions.Add(ConditionNotGlobalKeys.Instance);
            OnSpawnConditions.Add(ConditionNearbyPlayersCarryValue.Instance);
            OnSpawnConditions.Add(ConditionNearbyPlayersCarryItem.Instance);
            OnSpawnConditions.Add(ConditionNearbyPlayersNoise.Instance);
            OnSpawnConditions.Add(ConditionNearbyPlayersStatus.Instance);

            OnSpawnConditions.Add(ConditionLoaderCLLC.ConditionWorldLevel);

            // Default conditions

            DefaultSpawnConditions.Add(ConditionDaytime.Instance);
            DefaultSpawnConditions.Add(ConditionGlobalKeysRequired.Instance);
            DefaultSpawnConditions.Add(ConditionMaxSpawned.Instance);
            DefaultSpawnConditions.Add(ConditionEnvironments.Instance);
        }

        public bool FilterOnAwake(SpawnSystem spawner, SpawnConfiguration config)
        {
            return OnAwakeConditions.Any(x =>
            {
                try
                {
                    return x?.ShouldFilter(spawner, config) ?? false;
                }
                catch (Exception e)
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
                catch (Exception e)
                {
                    Log.LogError($"Error while attempting to check OnSpawn condition {x.GetType().Name}.", e);
                    return false;
                }
            });
        }

        public bool FilterDefault(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration config = null)
        {
            if (config is null)
            {
                var cache = SpawnSystemConfigCache.Get(spawn);

                if (cache?.Config == null)
                {
                    return false;
                }

                config = cache.Config;
            }

            return DefaultSpawnConditions.Any(x => x?.ShouldFilter(spawner, spawn, config) ?? false);
        }
    }
}
