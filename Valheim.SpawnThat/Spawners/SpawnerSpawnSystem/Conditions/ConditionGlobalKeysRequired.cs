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

        public static ConditionGlobalKeysRequired Instance
        {
            get
            {
                return _instance ??= new ConditionGlobalKeysRequired();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration config)
        {
            if(config is null)
            {
                return false;
            }

            if(IsValid(config))
            {
                return false;
            }

            Log.LogTrace($"Filtering {config.Name} due to global keys required.");
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
