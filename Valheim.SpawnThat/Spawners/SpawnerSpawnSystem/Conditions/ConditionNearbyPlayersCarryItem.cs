﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionNearbyPlayersCarryItem : IConditionOnSpawn
    {
        private static ConditionNearbyPlayersCarryItem _instance;

        public static ConditionNearbyPlayersCarryItem Instance => _instance ??= new();

        public bool ShouldFilter(SpawnConditionContext context)
        {
            if ((context.Config.DistanceToTriggerPlayerConditions?.Value ?? 0) <= 0)
            {
                return false;
            }

            if(string.IsNullOrWhiteSpace(context.Config.ConditionNearbyPlayerCarriesItem?.Value))
            {
                return false;
            }

            List<Player> players = PlayerUtils.GetPlayersInRadius(context.Position, context.Config.DistanceToTriggerPlayerConditions.Value);

            var itemsLookedFor = context.Config.ConditionNearbyPlayerCarriesItem?.Value?.SplitByComma(true)?.ToHashSet();

            if(itemsLookedFor is null)
            {
                return false;
            }

            foreach (var player in players.Where(x => x is not null && x))
            {
                var items = player.GetInventory()?.GetAllItems();

                if(items is null)
                {
                    continue;
                }

                foreach(var item in items)
                {
                    if(item is null)
                    {
                        continue;
                    }

                    string itemName = item.m_dropPrefab?.name?.Trim()?.ToUpperInvariant();

                    if(string.IsNullOrWhiteSpace(itemName))
                    {
                        continue;
                    }

                    if(itemsLookedFor.Contains(itemName))
                    {
                        return false;
                    }
                }
            }

            Log.LogTrace($"Ignoring world config {context.Config.Name} due to not finding any of the items on nearby players.");
            return true;
        }
    }
}
