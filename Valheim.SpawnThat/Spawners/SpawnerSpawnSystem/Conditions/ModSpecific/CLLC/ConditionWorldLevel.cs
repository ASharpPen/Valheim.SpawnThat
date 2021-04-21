using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions.ModSpecific.CLLC
{
    internal class ConditionWorldLevel : IConditionOnSpawn
    {
        private static ConditionWorldLevel _instance;

        public static ConditionWorldLevel Instance
        {
            get
            {
                return _instance ??= new ConditionWorldLevel();
            }
        }

        public bool ShouldFilter(SpawnSystem.SpawnData spawner, SpawnConfiguration spawnerConfig)
        {
            if(spawnerConfig.TryGet(SpawnSystemConfigCLLC.ModName, out Config modConfig))
            {
                if (modConfig is SpawnSystemConfigCLLC config)
                { 
                    int worldLevel = CreatureLevelControl.API.GetWorldLevel();

                    if (config.ConditionWorldLevelMin.Value >= 0 && worldLevel < config.ConditionWorldLevelMin.Value)
                    {
                        Log.LogTrace($"Filtering spawner {spawner.m_name} due to CLLC world level being too low. {worldLevel} < {config.ConditionWorldLevelMin}.");
                        return true;
                    }

                    if (config.ConditionWorldLevelMax.Value >= 0 && worldLevel > config.ConditionWorldLevelMax.Value)
                    {
                        Log.LogTrace($"Filtering spawner {spawner.m_name} due to CLLC world level being too high. {worldLevel} > {config.ConditionWorldLevelMax}.");
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
