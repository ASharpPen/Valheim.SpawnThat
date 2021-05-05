using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawns.Caches;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionNearbyPlayersNoise : IConditionOnSpawn
    {
        private static ConditionNearbyPlayersNoise _instance;

        public static ConditionNearbyPlayersNoise Instance
        {
            get
            {
                return _instance ??= new ConditionNearbyPlayersNoise();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration config)
        {
            if (!spawner || !spawner.transform || spawner is null || config is null || spawner.transform?.position is null)
            {
                return false;
            }

            if ((config.DistanceToTriggerPlayerConditions?.Value ?? 0) <= 0)
            {
                return false;
            }

            if (config.ConditionNearbyPlayersNoiseThreshold.Value == 0)
            {
                return false;
            }

            List<Player> players = new List<Player>();
            Player.GetPlayersInRange(spawner.transform.position, config.DistanceToTriggerPlayerConditions.Value, players);

            foreach (var player in players.Where(x => x && x is not null))
            {
                Log.LogTrace($"Checking noise of player {player.GetPlayerName()}");

                var zdo = SpawnCache.GetZDO(player);
                if (zdo is not null)
                {
                    var noise = zdo.GetFloat("noise", 0);

                    if(noise >= config.ConditionNearbyPlayersNoiseThreshold.Value)
                    {
                        return false;
                    }
                    else
                    {
                        Log.LogTrace($"Player {player.GetPlayerName()} have accumulated noise of {noise}");
                    }
                }
            }

            Log.LogTrace($"Ignoring world config {config.Name} due to not having any nearby noisy players.");
            return true;
        }
    }
}
