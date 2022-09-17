//#define VERBOSE

using System.Collections.Generic;
using SpawnThat.Utilities;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities.Extensions;
using System.Linq;

namespace SpawnThat.Options.Conditions;

public class ConditionNearbyPlayersCarryValue : ISpawnCondition
{
    public int SearchDistance { get; set; }
    public int RequiredValue { get; set; }

    public ConditionNearbyPlayersCarryValue()
    { }

    public ConditionNearbyPlayersCarryValue(int distanceToSearch, int combinedValueRequired)
    {
        SearchDistance = distanceToSearch;

        RequiredValue = combinedValueRequired;
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (RequiredValue <= 0)
        {
            return true;
        }

        if (SearchDistance <= 0)
        {
            return true;
        }

        List<Player> players = PlayerUtils.GetPlayersInRadius(context.SpawnerZdo.GetPosition(), SearchDistance);

        var valueSum = 0;

        foreach (var player in players)
        {
            if (player.IsNull())
            {
                continue;
            }

            var items = player.GetInventory()?.GetAllItems() ?? Enumerable.Empty<ItemDrop.ItemData>();

#if DEBUG && VERBOSE
            Log.LogTrace($"Player '{player.m_name}': {items.Join(x => x.m_shared?.m_name)}");
#endif

            foreach (var item in items)                     
            {
                valueSum += item?.GetValue() ?? 0;
            }
        }

        return valueSum >= RequiredValue;
    }
}
