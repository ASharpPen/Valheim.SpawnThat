using Valheim.SpawnThat.Spawners.Contexts;

namespace Valheim.SpawnThat.Spawn.Conditions;

public class ConditionDistanceToCenter : ISpawnCondition
{
    public double? MinDistance { get; }
    public double? MaxDistance { get; }

    public bool CanRunClientSide { get; } = true;
    public bool CanRunServerSide { get; } = true;

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
}
