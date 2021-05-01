using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionWorldAge : IConditionOnSpawn
    {
        private static ConditionWorldAge _instance;

        public static ConditionWorldAge Instance
        {
            get
            {
                return _instance ??= new ConditionWorldAge();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration config)
        {
            if (IsValid(config))
            {
                return false;
            }

            Log.LogTrace($"Filtering spawner {spawn.m_name} due to world age.");
            return true;
        }

        public bool IsValid(SpawnConfiguration config)
        {
            int day = EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds());

            if (config.ConditionWorldAgeDaysMin.Value > 0 && config.ConditionWorldAgeDaysMin.Value > day)
            {
                return false;
            }

            if (config.ConditionWorldAgeDaysMax.Value > 0 && config.ConditionWorldAgeDaysMax.Value < day)
            {
                return false;
            }

            return true;
        }
    }
}
