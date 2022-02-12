# Epic Loot Integration

Additional options for [Epic Loot](https://valheim.thunderstore.io/package/RandyKnapp/EpicLoot/).
See the mod page for more in-depth documentation.

# World Spawner Options

Mod-specific configs can be added to each world spawner as `[WorldSpawner.Index.EpicLoot]`

| Setting | Type | Default | Example | Description |
| --- | --- | --- | --- | --- |
| ConditionNearbyPlayerCarryLegendaryItem | string | | HeimdallLegs, RagnarLegs | Checks if any nearby player has one of the listed epic loot legendary id's in inventory |
| ConditionNearbyPlayerCarryItemWithRarity | string | | Magic, Legendary | Checks if any nearby player has an item of the listed rarities |

# Rarity Values
- Magic
- Rare
- Epic
- Legendary