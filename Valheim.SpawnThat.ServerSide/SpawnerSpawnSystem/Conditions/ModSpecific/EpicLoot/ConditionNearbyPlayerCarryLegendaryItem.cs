using EpicLoot;
using ExtendedItemDataFramework;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions.ModSpecific.EpicLoot
{
    public class ConditionNearbyPlayerCarryLegendaryItem : ISpawnCondition
    {
        private int SearchDistance { get; }

        private HashSet<string> LegendaryIds { get; }

        public ConditionNearbyPlayerCarryLegendaryItem(int distanceToSearch, params string[] legendaryIdsToSearch)
        {
            SearchDistance = distanceToSearch;

            LegendaryIds = legendaryIdsToSearch
                .Select(x => x.Trim().ToUpperInvariant())
                .ToHashSet();
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (LegendaryIds.Count == 0)
            {
                return true;
            }

            if (SearchDistance <= 0)
            {
                return false;
            }

            List<Player> players = PlayerUtils.GetPlayersInRadius(context.SpawnSystemZDO.GetPosition(), SearchDistance);

            foreach (var player in players)
            {
                var items = player?.GetInventory()?.GetAllItems() ?? new(0);

                if (items.Any(
                    x =>
                    {
                        var magicComponent = x?.Extended()?.GetComponent<MagicItemComponent>();

                        if (magicComponent is null)
                        {
                            return false;
                        }

                        return LegendaryIds.Contains(magicComponent.MagicItem.LegendaryID?.Trim()?.ToUpperInvariant());
                    }))
                {
                    return true;
                }
            }

            return false;
        }
    }
}