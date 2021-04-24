using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionNearbyPlayersCarryItem : IConditionOnSpawn
    {
        private static ConditionNearbyPlayersCarryItem _instance;

        public static ConditionNearbyPlayersCarryItem Instance
        {
            get
            {
                return _instance ??= new ConditionNearbyPlayersCarryItem();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration config)
        {
            if (spawner is null || config is null)
            {
                return false;
            }

            if (config.DistanceToTriggerPlayerConditions.Value < 0)
            {
                return false;
            }

            if(string.IsNullOrWhiteSpace(config.ConditionNearbyPlayerCarriesItem.Value))
            {
                return false;
            }

            List<Player> players = new();
            Player.GetPlayersInRange(spawner.transform.position, config.DistanceToTriggerPlayerConditions.Value, players);

            var itemsLookedFor = config.ConditionNearbyPlayerCarriesItem.Value.SplitByComma(true).ToHashSet();

            foreach (var player in players)
            {
                var items = player.GetInventory().GetAllItems();

                if (items.Any(x => itemsLookedFor.Contains(x.m_dropPrefab.name.Trim().ToUpperInvariant())))
                {
                    return false;
                }
            }

            Log.LogTrace($"Ignoring world config {config.Name} due to not finding any of the items on nearby players.");
            return true;
        }
    }
}
