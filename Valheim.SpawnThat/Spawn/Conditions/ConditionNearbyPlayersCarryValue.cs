using System.Collections.Generic;
using Valheim.SpawnThat.Utilities;
using Valheim.SpawnThat.Spawners.Contexts;

namespace Valheim.SpawnThat.Spawn.Conditions;

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
            return false;
        }

        List<Player> players = PlayerUtils.GetPlayersInRadius(context.SpawnerZdo.GetPosition(), SearchDistance);

        var valueSum = 0;

        foreach (var player in players)
        {
            var items = player?.GetInventory()?.GetAllItems() ?? new(0);

            foreach (var item in items)
            {
                if (item?.m_shared is null)
                {
                    continue;
                }

                valueSum += item.GetValue();
            }
        }

        return valueSum <= RequiredValue;
    }
}
