using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawn.Conditions;

public class ConditionCloseToPlayer : ISpawnCondition
{
    public ConditionCloseToPlayer(float withinDistance)
    {
        WithinDistance = withinDistance;
    }

    public float WithinDistance { get; }
    public bool CanRunClientSide => true;
    public bool CanRunServerSide => true;

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