using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.Conditions;

public class ConditionNearbyPlayersCarryItem : ISpawnCondition
{
    private int SearchDistance { get; }
    private HashSet<string> ItemsSearchedFor { get; }

    public bool CanRunClientSide { get; } = true;
    public bool CanRunServerSide { get; } = false;

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
