using System.Collections.Generic;
using System.Linq;
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
            if (!spawner || !spawner.transform || spawner is null || config is null || spawner.transform?.position is null)
            {
                return false;
            }

            if((config.DistanceToTriggerPlayerConditions?.Value ?? 0) <= 0)
            {
                return false;
            }

            List<Player> players = new List<Player>();
            Player.GetPlayersInRange(spawner.transform.position, config.DistanceToTriggerPlayerConditions.Value, players);

            var requiredSum = config.ConditionNearbyPlayersCarryValue?.Value ?? 0;
            var valueSum = 0;

            if((players?.Count ?? 0) == 0)
            {
                Log.LogTrace($"Ignoring world config {config.Name} due to condition {nameof(ConditionNearbyPlayersCarryValue)}.");
                return false;
            }

            foreach(var player in players.Where(x => x is not null && x))
            {
                var items = player.GetInventory()?.GetAllItems();

                if (items is null)
                {
                    continue;
                }

                foreach (var item in items)
                {
                    if (item is null)
                    {
                        continue;
                    }

                    if(item.m_shared is null)
                    {
                        continue;
                    }

                    valueSum += item.GetValue();
                }
            }

            if(valueSum < requiredSum)
            {
                Log.LogTrace($"Ignoring world config {config.Name} due to the combined player's in range value of '{valueSum}' being less than the required '{requiredSum}'.");
                return true;
            }
            return false;
        }
    }
}
