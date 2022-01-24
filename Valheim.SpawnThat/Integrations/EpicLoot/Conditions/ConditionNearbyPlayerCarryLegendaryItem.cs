using EpicLoot;
using ExtendedItemDataFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Spawn.Conditions;
using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Integrations.EpicLoot.Conditions;

public class ConditionNearbyPlayerCarryLegendaryItem : ISpawnCondition
{
    public int SearchDistance { get; set; }
    public HashSet<string> LegendaryIds { get; set; }

    public ConditionNearbyPlayerCarryLegendaryItem()
    { }

    public ConditionNearbyPlayerCarryLegendaryItem(int distanceToSearch, IEnumerable<string> legendaryIdsToSearch)
    {
        SearchDistance = distanceToSearch;

        LegendaryIds = legendaryIdsToSearch
            .Select(x => x.Trim().ToUpperInvariant())
            .ToHashSet();
    }

    public ConditionNearbyPlayerCarryLegendaryItem(int distanceToSearch, params string[] legendaryIdsToSearch)
    {
        SearchDistance = distanceToSearch;

        LegendaryIds = legendaryIdsToSearch
            .Select(x => x.Trim().ToUpperInvariant())
            .ToHashSet();
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (LegendaryIds.Count == 0)
        {
            return true;
        }

        if (SearchDistance <= 0)
        {
            return false;
        }

        List<Player> players = PlayerUtils.GetPlayersInRadius(context.SpawnerZdo.GetPosition(), SearchDistance);

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