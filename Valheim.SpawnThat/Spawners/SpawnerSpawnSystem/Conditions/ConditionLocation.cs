using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Locations;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionLocation : IConditionOnAwake
    {
        private static ConditionLocation _instance;

        public static ConditionLocation Instance
        {
            get
            {
                return _instance ??= new ConditionLocation();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnConfiguration config)
        {
            if (!spawner || spawner is null || config is null)
            {
                return false;
            }

            if (IsValid(spawner.transform.position, config))
            {
                return false;
            }

            return true;
        }

        public bool IsValid(Vector3 position, SpawnConfiguration config)
        {
            if(string.IsNullOrWhiteSpace(config.ConditionLocation.Value))
            {
                return true;
            }

            var locations = config.ConditionLocation.Value.SplitByComma(true);

            if(locations.Count == 0)
            {
                return true;
            }

            var location = LocationHelper
                .FindLocation(position)?
                .LocationName?
                .Trim()?
                .ToUpperInvariant();

            if(location is null)
            {
                Log.LogTrace($"Ignoring world config {config.Name} due to spawner not being in required location.");
                return false;
            }

            if(locations.Any(x => x == location))
            {
                return true;
            }

            Log.LogTrace($"Ignoring world config {config.Name} due to spawner not being in required location.");
            return false;
        }
    }
}
