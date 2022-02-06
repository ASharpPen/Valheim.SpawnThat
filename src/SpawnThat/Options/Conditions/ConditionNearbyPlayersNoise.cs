using System.Collections.Generic;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Options.Conditions;

public class ConditionNearbyPlayersNoise : ISpawnCondition
{
    public int SearchDistance { get; set; }
    public float NoiseThreshold { get; set; }

    internal ConditionNearbyPlayersNoise()
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
            return true;
        }

        if (NoiseThreshold <= 0)
        {
            return true;
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