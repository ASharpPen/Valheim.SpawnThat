using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    internal class ConditionDaytime : IConditionOnSpawn
    {
        public const string ZdoConditionDay = "spawnthat_condition_daytime_day";
        public const string ZdoConditionNight = "spawnthat_condition_daytime_night";

        private static ConditionDaytime _instance;

        public static ConditionDaytime Instance
        {
            get
            {
                return _instance ??= new ConditionDaytime();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration config)
        {
            if(config is null)
            {
                return false;
            }

            if(IsValid(config.SpawnDuringDay.Value, config.SpawnDuringNight.Value))
            {
                return false;
            }

            Log.LogTrace($"Filtering {config.Name} due to daytime.");
            return true;
        }

        public bool IsValid(bool spawnDuringDay, bool spawnDuringNight)
        {
            var envMan = EnvMan.instance;

            if (!spawnDuringDay && envMan.IsDay())
            {
                return false;
            }

            if (!spawnDuringNight && envMan.IsNight())
            {
                return false;
            }

            return true;
        }
    }
}
