using EpicLoot;
using ExtendedItemDataFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions.ModSpecific.EpicLoot
{
    public class ConditionNearbyPlayerCarryItemWithRarity : IConditionOnSpawn
    {
        private static ConditionNearbyPlayerCarryItemWithRarity instance;

        public static ConditionNearbyPlayerCarryItemWithRarity Instance => instance ??= new();

        public bool ShouldFilter(SpawnConditionContext context)
        {
            if (IsValid(context.Position, context.Config))
            {
                return false;
            }

            Log.LogTrace($"Ignoring world config {context.Config.Name} due to no players in area carrying item with required rarity.");
            return true;
        }

        public bool IsValid(Vector3 center, SpawnConfiguration spawnConfig)
        {
            if ((spawnConfig.DistanceToTriggerPlayerConditions?.Value ?? 0) <= 0)
            {
                return true;
            }

            if (!spawnConfig.TryGet(SpawnSystemConfigEpicLoot.ModName, out Config modConfig) || modConfig is not SpawnSystemConfigEpicLoot config)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(config.ConditionNearbyPlayerCarryItemWithRarity?.Value))
            {
                return true;
            }

            List<Player> players = PlayerUtils.GetPlayersInRadius(center, spawnConfig.DistanceToTriggerPlayerConditions.Value);

            var raritiesLookedFor = config.ConditionNearbyPlayerCarryItemWithRarity.Value.SplitByComma().ToHashSet();

            foreach (var player in players.Where(x => x is not null && x))
            {
                var items = player.GetInventory()?.GetAllItems();

                if (items is null)
                {
                    continue;
                }

                if (items.Any(
                    x =>
                    {
                        var magicComponent = x?.Extended()?.GetComponent<MagicItemComponent>();

                        if (magicComponent is null)
                        {
                            return false;
                        }

                        return raritiesLookedFor.Contains(magicComponent.MagicItem.Rarity.ToString());
                    }))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
