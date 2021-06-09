using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Locations;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Position
{
    public class PositionConditionLocation : ISpawnPositionCondition
    {
        private static PositionConditionLocation instance;

        public static PositionConditionLocation Instance
        {
            get
            {
                return instance ??= new();
            }
        }

        public bool ShouldFilter(SpawnSystem.SpawnData spawn, SpawnConfiguration config, Vector3 position)
        {
            if(IsValid(config, position))
            {
                return false;
            }

#if DEBUG && false
            Log.LogTrace($"Ignoring world config {config.Name} due to position not being in required location.");
#endif
            return true;
        }

        public bool IsValid(SpawnConfiguration config, Vector3 position)
        {
            if(config is null)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(config.ConditionLocation.Value))
            {
                return true;
            }

            var locations = config.ConditionLocation.Value.SplitByComma(true);

            var location = LocationHelper
                .FindLocation(position)?
                .LocationName?
                .Trim()?
                .ToUpperInvariant();

            if (location is null)
            {
                return false;
            }

            if (locations.Any(x => x == location))
            {
                return true;
            }

            return false;
        }
    }
}
