// #define VERBOSE

using System;
using System.Collections.Generic;
using System.Linq;
using EpicLoot;
using ExtendedItemDataFramework;
using HarmonyLib;
using SpawnThat.Core;
using SpawnThat.Integrations.EpicLoot.Models;
using SpawnThat.Options.Conditions;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Integrations.EpicLoot.Conditions;

public class ConditionNearbyPlayerCarryItemWithRarity : ISpawnCondition
{
    public int SearchDistance { get; set; }
    public List<EpicLootRarity> RaritiesRequired { get; set; }

    public ConditionNearbyPlayerCarryItemWithRarity()
    { }

    public ConditionNearbyPlayerCarryItemWithRarity(int distanceToSearch, IEnumerable<EpicLootRarity> rarities)
    {
        SearchDistance = distanceToSearch;
        RaritiesRequired = rarities?.ToList();
    }

    public ConditionNearbyPlayerCarryItemWithRarity(int distanceToSearch, IEnumerable<string> rarities)
    {
        SearchDistance = distanceToSearch;

        RaritiesRequired = new();

        if (rarities is not null)
        {
            foreach (var rarityName in rarities)
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
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if ((RaritiesRequired?.Count ?? 0) == 0)
        {
            return true;
        }

        if (SearchDistance <= 0)
        {
            return true;
        }

        List<Player> players = PlayerUtils.GetPlayersInRadius(context.SpawnerZdo.GetPosition(), SearchDistance);

        foreach (var player in players)
        {
            if (player.IsNull())
            {
                continue;
            }

            var items = player.GetInventory()?.GetAllItems() ?? new(0);

#if DEBUG && VERBOSE
            Log.LogTrace($"Player '{player.m_name}': {items.Join(x => x.m_shared.m_name + ":" + x?.Extended()?.GetComponent<MagicItemComponent>()?.MagicItem?.Rarity.ToRarity().ToString() ?? "")}");
#endif

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