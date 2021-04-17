﻿using Valheim.SpawnThat.Configuration.ConfigTypes;
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

        public bool ShouldFilter(SpawnSystem.SpawnData spawner, SpawnConfiguration config)
        {
            int day = EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds());

            if (config.ConditionWorldAgeDaysMin > 0 && config.ConditionWorldAgeDaysMin > day)
            {
                Log.LogInfo($"Filtering spawner {spawner.m_name} due to world not being old enough.");
                return true;
            }

            if (config.ConditionWorldAgeDaysMax > 0 && config.ConditionWorldAgeDaysMax < day)
            {
                Log.LogInfo($"Filtering spawner {spawner.m_name} due to world being too old.");
                return true;
            }

            return false;
        }
    }
}