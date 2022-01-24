using System.Collections.Generic;
using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.Utilities;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.Spawn.Conditions;

public class ConditionNearbyPlayersNoise : ISpawnCondition
{
    public int SearchDistance { get; set; }
    public float NoiseThreshold { get; set; }

    public ConditionNearbyPlayersNoise()
    { }

    public ConditionNearbyPlayersNoise(int distanceToSearch, float noiseRequired)
    {
        SearchDistance = distanceToSearch;
        NoiseThreshold = noiseRequired;
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (SearchDistance <= 0)
        {
            return false;
        }

        if (NoiseThreshold <= 0)
        {
            return false;
        }

        List<ZDO> players = PlayerUtils.GetPlayerZdosInRadius(context.SpawnerZdo.GetPosition(), SearchDistance);

        foreach (var player in players)
        {
            if (player is null)
            {
                continue;
            }

            if (player.GetNoise() >= NoiseThreshold)
            {
                return true;
            }
        }

        return false;
    }
}