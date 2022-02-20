using EpicLoot;
using SpawnThat.Integrations.EpicLoot.Models;

namespace SpawnThat.Integrations.EpicLoot;

internal static class EpicLootRarityExtensions
{
    public static ItemRarity ToRarity(this EpicLootRarity rarity)
        => rarity switch
        {
            EpicLootRarity.Magic => ItemRarity.Magic,
            EpicLootRarity.Rare => ItemRarity.Rare,
            EpicLootRarity.Epic => ItemRarity.Epic,
            EpicLootRarity.Legendary => ItemRarity.Legendary,
        };

    public static EpicLootRarity ToRarity(this ItemRarity rarity)
        => rarity switch
        {
            ItemRarity.Magic => EpicLootRarity.Magic,
            ItemRarity.Rare => EpicLootRarity.Rare,
            ItemRarity.Epic => EpicLootRarity.Epic,
            ItemRarity.Legendary => EpicLootRarity.Legendary,
        };
}