using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionNearbyPlayersStatus : IConditionOnSpawn
    {
        private static ConditionNearbyPlayersStatus _instance;

        public static ConditionNearbyPlayersStatus Instance => _instance ??= new();

        public bool ShouldFilter(SpawnConditionContext context)
        {
            var config = context.Config;

            if ((config.DistanceToTriggerPlayerConditions?.Value ?? 0) <= 0)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(config.ConditionNearbyPlayersStatus.Value))
            {
                return false;
            }

            List<Player> players = PlayerUtils.GetPlayersInRadius(context.Position, config.DistanceToTriggerPlayerConditions.Value);

            if (players.Count == 0)
            {
                return true;
            }

            var statusEffects = config.ConditionNearbyPlayersStatus.Value.SplitByComma();

#if DEBUG
            Log.LogDebug($"Searching for effects: {statusEffects.Join()}");
#endif


            foreach (var player in players.Where(x => x && x is not null))
            {
#if DEBUG
                Log.LogDebug($"Checking status of player {player.GetPlayerName()}");
#endif

                SEMan statusEffectManager = player.GetSEMan();

                if(statusEffects.Any(x => statusEffectManager.HaveStatusEffect(x)))
                {
#if DEBUG
                    Log.LogDebug($"Found status effect on {player.GetPlayerName()}.");
#endif
                    return false;
                }
            }

            Log.LogTrace($"Ignoring world config {config.Name} due to not having any nearby players with a required status effect.");
            return true;
        }
    }
}
