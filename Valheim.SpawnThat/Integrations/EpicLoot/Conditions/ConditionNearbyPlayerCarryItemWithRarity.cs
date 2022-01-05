using System;
using System.Collections.Generic;
using System.Linq;
using EpicLoot;
using ExtendedItemDataFramework;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Integrations.EpicLoot.Models;
using Valheim.SpawnThat.Spawn.Conditions;
using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Integrations.EpicLoot.Conditions;

public class ConditionNearbyPlayerCarryItemWithRarity : ISpawnCondition
{
    private int SearchDistance { get; }
    private List<EpicLootRarity> RaritiesRequired { get; set; }

    public bool CanRunClientSide { get; } = true;
    public bool CanRunServerSide { get; } = false;

    public ConditionNearbyPlayerCarryItemWithRarity(int distanceToSearch, IEnumerable<EpicLootRarity> rarities)
    {
        SearchDistance = distanceToSearch;
        RaritiesRequired = rarities.ToList();
    }

    public ConditionNearbyPlayerCarryItemWithRarity(int distanceToSearch, IEnumerable<string> rarities)
    {
        SearchDistance = distanceToSearch;

        RaritiesRequired = new();

        foreach(var rarityName in rarities)
        {
            if (Enum.TryParse(rarityName, true, out EpicLootRarity rarity))
            {
                RaritiesRequired.Add(rarity);
            }
            else
            {
                Log.LogWarning($"Unable to parse EpicLoot rarity '{rarity}'. Verify spelling.");
            }
        }
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

            if (items.Any(x =>
                {
                    var magicComponent = x?.Extended()?.GetComponent<MagicItemComponent>();

                    if (magicComponent is null)
                    {
                        return false;
                    }

                    return RaritiesRequired.Contains(magicComponent.MagicItem.Rarity.ToRarity());
                }))
            {
                return true;
            }
        }

        return false;
    }
}