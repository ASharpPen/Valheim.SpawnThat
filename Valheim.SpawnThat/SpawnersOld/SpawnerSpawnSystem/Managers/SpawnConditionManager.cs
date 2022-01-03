using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.Caches;
using Valheim.SpawnThat.Spawners.Conditions;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions.ModSpecific;
using Valheim.SpawnThat.Startup;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Managers
{
    public class SpawnConditionManager
    {
        private HashSet<IConditionOnAwake> OnAwakeConditions = new HashSet<IConditionOnAwake>();
        private List<IConditionOnSpawn> OnSpawnConditions = new();
        private HashSet<IConditionOnSpawn> DefaultSpawnConditions = new HashSet<IConditionOnSpawn>();

        private static SpawnConditionManager _instance;

        public static SpawnConditionManager Instance => _instance ??= new();

        SpawnConditionManager()
        {
            StateResetter.Subscribe(() =>
            {
                _instance = null;
            });

            // OnSpawn conditions
            OnSpawnConditions.Add(ConditionDistanceToCenter.Instance);
            OnSpawnConditions.Add(ConditionLocation.Instance);
            OnSpawnConditions.Add(ConditionWorldAge.Instance);
            OnSpawnConditions.Add(ConditionNotGlobalKeys.Instance);
            OnSpawnConditions.Add(ConditionNearbyPlayersCarryValue.Instance);
            OnSpawnConditions.Add(ConditionNearbyPlayersCarryItem.Instance);
            OnSpawnConditions.Add(ConditionNearbyPlayersNoise.Instance);
            OnSpawnConditions.Add(ConditionNearbyPlayersStatus.Instance);
            OnSpawnConditions.Add(ConditionAreaSpawnChance.Instance);
            OnSpawnConditions.Add(ConditionAreaIds.Instance);

            OnSpawnConditions.Add(ConditionLoaderCLLC.ConditionWorldLevel);

            OnSpawnConditions.Add(ConditionLoaderEpicLoot.ConditionNearbyPlayerCarryItemWithRarity);
            OnSpawnConditions.Add(ConditionLoaderEpicLoot.ConditionNearbyPlayerCarryLegendaryItem);

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
            var cache = SpawnDataCache.Get(spawn);

            if (cache?.Config == null)
            {
                return false;
            }

            if (spawn?.m_prefab is null)
            {
                return true;
            }

            if(spawner is null)
            {
                return true;
            }

            var context = new SpawnConditionContext
            {
                Config = cache.Config,
                Position = spawner.transform.position,
                SpawnData = spawn
            };

            return OnSpawnConditions.Any(x =>
            {
                try
                {
                    return x?.ShouldFilter(context) ?? false;
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
                var cache = SpawnDataCache.Get(spawn);

                if (cache?.Config == null)
                {
                    return false;
                }

                config = cache.Config;
            }

            if (spawn?.m_prefab is null)
            {
                return true;
            }

            if (spawner is null)
            {
                return true;
            }

            var context = new SpawnConditionContext
            {
                Config = config,
                Position = spawner.transform.position,
                SpawnData = spawn,
            };

            return DefaultSpawnConditions.Any(x => x?.ShouldFilter(context) ?? false);
        }
    }
}
