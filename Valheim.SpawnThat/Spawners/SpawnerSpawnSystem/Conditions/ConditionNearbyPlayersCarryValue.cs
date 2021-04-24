using System.Collections.Generic;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionNearbyPlayersCarryValue : IConditionOnSpawn
    {
        private static ConditionNearbyPlayersCarryValue _instance;

        public static ConditionNearbyPlayersCarryValue Instance
        {
            get
            {
                return _instance ??= new ConditionNearbyPlayersCarryValue();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration config)
        {
            if(spawner is null || config is null)
            {
                return false;
            }

            if(config.DistanceToTriggerPlayerConditions.Value < 0)
            {
                return false;
            }

            List<Player> players = new();
            Player.GetPlayersInRange(spawner.transform.position, config.DistanceToTriggerPlayerConditions.Value, players);

            var requiredSum = config.ConditionNearbyPlayersCarryValue.Value;
            var valueSum = 0;

            foreach(var player in players)
            {
                var items = player.GetInventory().GetAllItems();

                foreach(var item in items)
                {
                    valueSum += item.GetValue();
                }
            }

            if(valueSum < requiredSum)
            {
                Log.LogTrace($"Ignoring world config {config.Name} due to player value of '{valueSum}' being less than the required '{requiredSum}'.");
                return true;
            }
            return false;
        }
    }
}
