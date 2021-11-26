using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Locations;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionLocation : IConditionOnSpawn
    {
        private static ConditionLocation _instance;

        public static ConditionLocation Instance => _instance ??= new();
        
        public bool ShouldFilter(SpawnConditionContext context)
        {
            if (context is null || context.Config is null)
            {
                return false;
            }

            if (IsValid(context.Position, context.Config))
            {
                return false;
            }

            Log.LogTrace($"Ignoring world config {context.Config.Name} due to spawner not being in required location.");
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
                return false;
            }

            if(locations.Any(x => x == location))
            {
                return true;
            }

            return false;
        }
    }
}
