using System.Collections.Generic;
using System.Linq;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities;

namespace SpawnThat.Options.Conditions;

public class ConditionNearbyPlayersCarryItem : ISpawnCondition
{
    public int SearchDistance { get; set; }
    public HashSet<string> ItemsSearchedFor { get; set; }

    public ConditionNearbyPlayersCarryItem()
    { }

    public ConditionNearbyPlayersCarryItem(int distanceToSearch, params string[] itemPrefabNames)
    {
        SearchDistance = distanceToSearch;
        ItemsSearchedFor = itemPrefabNames
            .Select(x => x.Trim().ToUpperInvariant())
            .ToHashSet();
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (ItemsSearchedFor.Count == 0)
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

            foreach (var item in items)
            {
                string itemName = item?.m_dropPrefab?.name?.Trim()?.ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(itemName))
                {
                    continue;
                }

                if (ItemsSearchedFor.Contains(itemName))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
