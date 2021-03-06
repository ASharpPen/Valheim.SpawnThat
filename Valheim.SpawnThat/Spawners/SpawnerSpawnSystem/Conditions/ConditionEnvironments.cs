﻿using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionEnvironments : IConditionOnSpawn
    {
        public const string ZdoCondition = "spawnthat_condition_environments";

        private static ConditionEnvironments _instance;

        public static ConditionEnvironments Instance => _instance ??= new();

        public bool ShouldFilter(SpawnConditionContext context)
        {
            return !IsValid(context.Config.RequiredEnvironments.Value);
        }

        public bool IsValid(string requiredEnvironments)
        {
            if (string.IsNullOrWhiteSpace(requiredEnvironments))
            {
                return true;
            }

            var envMan = EnvMan.instance;
            var currentEnv = envMan.GetCurrentEnvironment()?.m_name?.Trim()?.ToUpperInvariant();


            var environments = requiredEnvironments.SplitByComma(true);

            foreach (var requiredEnvironment in environments)
            {
                if (string.IsNullOrWhiteSpace(requiredEnvironment))
                {
                    continue;
                }

                if (requiredEnvironment == currentEnv)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
