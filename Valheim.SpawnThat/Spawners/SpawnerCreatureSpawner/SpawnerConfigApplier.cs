using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner
{
    internal static class SpawnerConfigApplier
    {
        internal static void ApplyConfiguration(this CreatureSpawner spawner, CreatureSpawnerConfig config)
        {
            var prefabName = spawner.m_creaturePrefab?.name;

            if (string.IsNullOrEmpty(prefabName))
            {
                return;
            }

            if (config is not null)
            {
                Log.LogDebug($"Found and applying config for local spawner {spawner.name}");

                CreatureSpawnerConfigCache.Set(spawner, config);

                var prefab = spawner.m_creaturePrefab;

                //Find creature prefab, if it needs to be overriden
                if (prefabName != config.PrefabName.Value)
                {
                    prefab = ZNetScene.instance.GetPrefab(config.PrefabName.Value);
                }

                if (!prefab || prefab is null)
                {
                    Log.LogWarning($"Unable to find prefab for {config.PrefabName.Value}");
                    spawner.SetSuccessfulInit();
                    return;
                }

                //Override existing config values:
                spawner.m_creaturePrefab = prefab;
                spawner.m_levelupChance = config.LevelUpChance.Value;
                spawner.m_maxLevel = config.LevelMax.Value;
                spawner.m_minLevel = config.LevelMin.Value;
                //__instance.m_requireSpawnArea = config.RequireSpawnArea.Value; //Disabled for now, since it isn't being used by the game.
                spawner.m_respawnTimeMinuts = config.RespawnTime.Value;
                spawner.m_setPatrolSpawnPoint = config.SetPatrolPoint.Value;
                spawner.m_spawnAtDay = config.SpawnAtDay.Value;
                spawner.m_spawnAtNight = config.SpawnAtNight.Value;
                spawner.m_triggerDistance = config.TriggerDistance.Value;
                spawner.m_triggerNoise = config.TriggerNoise.Value;
                spawner.m_spawnInPlayerBase = config.SpawnInPlayerBase.Value;
            }

            spawner.SetSuccessfulInit();
        }
    }
}
