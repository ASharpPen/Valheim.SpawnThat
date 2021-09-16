using System.Collections.Generic;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionNearbyPlayersCarryValue : ISpawnCondition
    {
        private int SearchDistance { get; }

        private int RequiredValue { get; }

        public ConditionNearbyPlayersCarryValue(int distanceToSearch, int combinedValueRequired)
        {
            SearchDistance = distanceToSearch;

            RequiredValue = combinedValueRequired;
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (RequiredValue <= 0)
            {
                return true;
            }

            if (SearchDistance <= 0)
            {
                return false;
            }

            List<Player> players = PlayerUtils.GetPlayersInRadius(context.SpawnSystemZDO.GetPosition(), SearchDistance);

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
}
