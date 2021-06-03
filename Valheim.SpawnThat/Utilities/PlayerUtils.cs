using System.Collections.Generic;
using UnityEngine;

namespace Valheim.SpawnThat.Utilities
{
    public static class PlayerUtils
    {
        public static List<Player> GetPlayersInRadius(Vector3 point, float range)
        {
            List<Player> players = new();
            foreach (Player player in Player.GetAllPlayers())
            {
                if (Utils.DistanceXZ(player.transform.position, point) < range)
                {
                    players.Add(player);
                }
            }

            return players;
        }
    }
}
