using System.Collections.Generic;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionNearbyPlayersNoise : ISpawnCondition
    {
        private int SearchDistance { get; }

        private int NoiseThreshold { get; }

        public ConditionNearbyPlayersNoise(int distanceToSearch, int noiseRequired)
        {
            SearchDistance = distanceToSearch;
            NoiseThreshold = noiseRequired;
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (SearchDistance <= 0)
            {
                return false;
            }

            if (NoiseThreshold <= 0)
            {
                return false;
            }

            List<Player> players = SpawnThat.Utilities.PlayerUtils.GetPlayersInRadius(context.SpawnSystemZDO.GetPosition(), SearchDistance);

            foreach (var player in players)
            {
                var playerZDO = player?.m_nview?.GetZDO();

                if (playerZDO is null)
                {
                    continue;
                }

                if (playerZDO.GetNoise() >= NoiseThreshold)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
