using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    /// <summary>
    /// Require player within distance to have one of the listed status effects.
    /// </summary>
    public class ConditionNearbyPlayersStatus : ISpawnCondition
    {
        private int SearchDistance { get; }

        private HashSet<string> RequiredStatusEffects { get; }

        public ConditionNearbyPlayersStatus(int distanceToSearch, params string[] requireOneOfStatusEffects)
        {
            SearchDistance = distanceToSearch;

            RequiredStatusEffects = requireOneOfStatusEffects
                .Select(x => x.Trim())
                .ToHashSet();
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (RequiredStatusEffects.Count == 0)
            {
                return true;
            }

            if (SearchDistance <= 0)
            {
                return false;
            }

            List<Player> players = PlayerUtils.GetPlayersInRadius(context.SpawnSystemZDO.GetPosition(), SearchDistance);

            foreach(var player in players)
            {
                SEMan statusEffectManager = player.GetSEMan();

                if (RequiredStatusEffects.Any(x => statusEffectManager.HaveStatusEffect(x)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
