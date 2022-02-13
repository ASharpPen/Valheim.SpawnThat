using UnityEngine;
using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.Conditions;

public class ConditionDistanceToCenter : ISpawnCondition
{
    public double? MinDistance { get; set; }
    public double? MaxDistance { get; set; }

    public ConditionDistanceToCenter()
    { }

    public ConditionDistanceToCenter(double? minDistanceRequired, double? maxDistanceRequired)
    {
        MinDistance = minDistanceRequired > 0
            ? minDistanceRequired
            : null;
        MaxDistance = maxDistanceRequired > 0
            ? maxDistanceRequired
            : null;
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (MinDistance is null && MaxDistance is null)
        {
            return true;
        }

        var distance = context.SpawnerZdo.GetPosition().magnitude;

        if (MinDistance is not null
            && distance < MinDistance)
        {
            return false;
        }

        if (MaxDistance is not null
            && distance > MaxDistance)
        {
            return false;
        }

        return true;
    }

    public bool IsValid(Vector3 position)
    {
        if (MinDistance is null && MaxDistance is null)
        {
            return true;
        }

        var distance = position.magnitude;

        if (MinDistance is not null
            && distance < MinDistance)
        {
            return false;
        }

        if (MaxDistance is not null
            && distance > MaxDistance)
        {
            return false;
        }

        return true;
    }
}
