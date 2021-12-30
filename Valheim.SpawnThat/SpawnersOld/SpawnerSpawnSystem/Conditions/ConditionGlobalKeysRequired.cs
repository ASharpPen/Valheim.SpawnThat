using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionGlobalKeysRequired : IConditionOnSpawn
    {
        private static ConditionGlobalKeysRequired _instance;

        public static ConditionGlobalKeysRequired Instance => _instance ??= new();

        public bool ShouldFilter(SpawnConditionContext context)
        {
            if(IsValid(context.Config))
            {
                return false;
            }

            Log.LogTrace($"Filtering {context.Config.Name} due to global keys required.");
            return true;
        }

        public bool IsValid(SpawnConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(config.RequiredGlobalKey.Value))
            {
                return true;
            }

            if (ZoneSystem.instance.GetGlobalKey(config.RequiredGlobalKey.Value))
            {
                return true;
            }

            return false;
        }
    }
}
