using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Spawners.Conditions;
using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Integrations.EpicLoot.Conditions;

public class ConditionNearbyPlayerCarryItemWithRarity : ISpawnCondition
{
    private int SearchDistance { get; }
    private List<string> RaritiesRequired { get; set; }

    public bool CanRunClientSide { get; } = true;
    public bool CanRunServerSide { get; } = false;

    public ConditionNearbyPlayerCarryItemWithRarity(int distanceToSearch, IEnumerable<string> rarities)
    {
        SearchDistance = distanceToSearch;

        RaritiesRequired = rarities
            .Select(x => x.Trim().ToUpperInvariant())
            .ToList();
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (RaritiesRequired.Count == 0)
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
                    // TODO: set up epic loot references and re-enable.
                    throw new NotImplementedException();
                    /*
                    var magicComponent = x?.Extended()?.GetComponent<MagicItemComponent>();

                    if (magicComponent is null)
                    {
                        return false;
                    }

                    return RaritiesRequired.Contains(magicComponent.MagicItem.Rarity.ToString().ToUpperInvariant());
                    */
                }))
            {
                return true;
            }
        }

        return false;
    }
}