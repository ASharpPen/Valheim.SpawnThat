using EpicLoot;
using SpawnThat.Integrations.EpicLoot.Models;

namespace SpawnThat.Integrations.EpicLoot;

internal static class EpicLootRarityExtensions
{
    public static EpicLootRarity ToRarity(this ItemRarity rarity)
        => rarity switch
        {
            ItemRarity.Magic => EpicLootRarity.Magic,
            ItemRarity.Rare => EpicLootRarity.Rare,
            ItemRarity.Epic => EpicLootRarity.Epic,
            ItemRarity.Legendary => EpicLootRarity.Legendary,
            ItemRarity.Mythic => EpicLootRarity.Mythic,
            _ => EpicLootRarity.None
        };
}