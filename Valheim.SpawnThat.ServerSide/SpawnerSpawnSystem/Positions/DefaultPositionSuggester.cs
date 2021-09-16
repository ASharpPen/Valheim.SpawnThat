using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.Utilities;
using Valheim.SpawnThat.Utilities.Extensions;
using Random = UnityEngine.Random;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    public class DefaultPositionSuggester
    {
        public uint MaxAttempts { get; set; } = 20;

        public uint MinDistance { get; set; } = 40;

        public uint MaxDistance { get; set; } = 80;

        public DefaultPositionSuggester()
        {
        }

        public Vector3? SuggestPosition(SpawnContext context)
        {
            // Find nearby players
            var nearbyPlayers = PlayerUtils.GetPlayersInRadius(context.SpawnSystemZdo.m_position, 64);

            // Repeat x times:
            for(int i = 0; i < MaxAttempts; ++i)
            {
                // Select random nearby
                var player = nearbyPlayers.Random();

                // Select random point surrounding player
                var position =
                    player.transform.position +
                    Quaternion.Euler(0f, Random.Range(0, 360), 0f)
                    * Vector3.forward
                    * Random.Range(MinDistance, MaxDistance);

                // Verify position conditions


            }


            return null;
        }
    }
}
