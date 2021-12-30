using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    internal class ConditionNotGlobalKeys : IConditionOnSpawn
    {
        private static ConditionNotGlobalKeys _instance;

        public static ConditionNotGlobalKeys Instance => _instance ??= new();

        public bool ShouldFilter(SpawnConditionContext context)
        {
            if(IsValid(context.Config))
            {
                return false;
            }

            Log.LogTrace($"Ignoring world config {context.Config.Name} due to finding a global key from {nameof(context.Config.RequiredNotGlobalKey)}.");
            return true;
        }

        public bool IsValid(SpawnConfiguration config)
        {
            if (!string.IsNullOrEmpty(config.RequiredNotGlobalKey?.Value))
            {
                var requiredNotKeys = config.RequiredNotGlobalKey.Value.SplitByComma();

                if (requiredNotKeys.Count > 0)
                {
                    foreach (var key in requiredNotKeys)
                    {
                        if (ZoneSystem.instance.GetGlobalKey(key))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
