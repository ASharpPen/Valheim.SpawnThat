using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.Configuration.ConfigTypes;
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

        public bool ShouldFilter(SpawnSystem.SpawnData spawner, SpawnConfiguration config)
        {
            if (!string.IsNullOrEmpty(config.RequiredNotGlobalKey))
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
