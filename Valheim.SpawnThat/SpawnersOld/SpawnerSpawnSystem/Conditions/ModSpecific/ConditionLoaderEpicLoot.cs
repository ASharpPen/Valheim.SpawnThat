using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Integrations.EpicLoot.Conditions;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions.ModSpecific.EpicLoot;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions.ModSpecific
{
    public static class ConditionLoaderEpicLoot
    {
        public static bool InstalledEpicLoot { get; } = Type.GetType("EpicLoot.EpicLoot, EpicLoot") is not null;

        public static ConditionNearbyPlayerCarryItemWithRarity ConditionNearbyPlayerCarryItemWithRarity
        {
            get
            {
                if (InstalledEpicLoot) return ConditionNearbyPlayerCarryItemWithRarity.Instance;

#if DEBUG
                if (!InstalledEpicLoot) Log.LogDebug("Epic Loot not found.");
#endif

                return null;
            }
        }

        public static ConditionNearbyPlayerCarryLegendaryItem ConditionNearbyPlayerCarryLegendaryItem
        {
            get
            {
                if (InstalledEpicLoot) return ConditionNearbyPlayerCarryLegendaryItem.Instance;

#if DEBUG
                if (!InstalledEpicLoot) Log.LogDebug("Epic Loot not found.");
#endif

                return null;
            }
        }
    }
}
