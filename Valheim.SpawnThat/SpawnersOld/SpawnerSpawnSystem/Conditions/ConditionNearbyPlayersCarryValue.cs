using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionNearbyPlayersCarryValue : IConditionOnSpawn
    {
        private static ConditionNearbyPlayersCarryValue _instance;

        public static ConditionNearbyPlayersCarryValue Instance => _instance ??= new();

        public bool ShouldFilter(SpawnConditionContext context)
        {
            if((context.Config.DistanceToTriggerPlayerConditions?.Value ?? 0) <= 0)
            {
                return false;
            }

            if(context.Config.ConditionNearbyPlayersCarryValue.Value <= 0)
            {
                return false;
            }

            List<Player> players = PlayerUtils.GetPlayersInRadius(context.Position, context.Config.DistanceToTriggerPlayerConditions.Value);

            var requiredSum = context.Config.ConditionNearbyPlayersCarryValue?.Value ?? 0;
            var valueSum = 0;

            if((players?.Count ?? 0) == 0)
            {
                Log.LogTrace($"Ignoring world config {context.Config.Name} due to condition {nameof(ConditionNearbyPlayersCarryValue)}.");
                return true;
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

            if (valueSum < requiredSum)
            {
                Log.LogTrace($"Filtering {context.Config.Name} due to summed value of nearby players inventory.");
                return true;
            }

            return false;
        }
    }
}
