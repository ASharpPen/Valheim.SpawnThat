﻿using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    internal class ConditionGlobalKeys : IConditionOnSpawn
    {
        private static ConditionGlobalKeys _instance;

        public static ConditionGlobalKeys Instance
        {
            get
            {
                return _instance ??= new ConditionGlobalKeys();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration config)
        {
            if (!string.IsNullOrEmpty(config.RequiredNotGlobalKey?.Value))
            {
                var requiredNotKeys = config.RequiredNotGlobalKey.Value.SplitByComma();

                if (requiredNotKeys.Count > 0)
                {
                    bool foundNotRequiredKey = false;

                    foreach (var key in requiredNotKeys)
                    {
                        if (ZoneSystem.instance.GetGlobalKey(key))
                        {
                            foundNotRequiredKey = true;
                            break;
                        }
                    }
                    if (foundNotRequiredKey)
                    {
                        Log.LogTrace($"Ignoring world config {config.Name} due to finding a global key from {nameof(config.RequiredNotGlobalKey)}.");
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
