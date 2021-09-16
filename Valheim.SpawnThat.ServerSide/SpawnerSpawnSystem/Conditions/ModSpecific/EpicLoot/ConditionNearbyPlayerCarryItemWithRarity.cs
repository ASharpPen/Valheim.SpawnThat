using EpicLoot;
using ExtendedItemDataFramework;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions.ModSpecific.EpicLoot
{
    public class ConditionNearbyPlayerCarryItemWithRarity : ISpawnCondition
    {
        private int SearchDistance { get; }

        private List<string> RaritiesRequired { get; set; }

        public ConditionNearbyPlayerCarryItemWithRarity(int distanceToSearch, IEnumerable<string> rarities)
        {
            SearchDistance = distanceToSearch;

            RaritiesRequired = rarities
                .Select(x => x.Trim().ToUpperInvariant())
                .ToList();
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (RaritiesRequired.Count == 0)
            {
                return true;
            }

            if (SearchDistance <= 0)
            {
                return false;
            }

            List<Player> players = PlayerUtils.GetPlayersInRadius(context.SpawnSystemZDO.GetPosition(), SearchDistance);

            foreach (var player in players)
            {
                var items = player?.GetInventory()?.GetAllItems() ?? new(0);

                if (items.Any(
                    x =>
                    {
                        var magicComponent = x?.Extended()?.GetComponent<MagicItemComponent>();

                        if (magicComponent is null)
                        {
                            return false;
                        }

                        return RaritiesRequired.Contains(magicComponent.MagicItem.Rarity.ToString().ToUpperInvariant());
                    }))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
