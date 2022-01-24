using System.Linq;
using UnityEngine;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities;

namespace SpawnThat.Options.Conditions;

public class ConditionCloseToPlayer : ISpawnCondition
{
    public float WithinDistance { get; set; }

    public ConditionCloseToPlayer()
    { }

    public ConditionCloseToPlayer(float withinDistance)
    {
        WithinDistance = withinDistance;
    }

    public bool IsValid(SpawnSessionContext sessionContext)
    {
        var point = sessionContext.SpawnerZdo.GetPosition();
        var nearbyPlayers = PlayerUtils.GetPlayerZdosInRadius(point, WithinDistance);

        if (nearbyPlayers.Count == 0)
        {
            return false;
        }

        return nearbyPlayers.Any(x => Vector3.Distance(x.GetPosition(), point) <= WithinDistance);
    }
}