using Valheim.SpawnThat.Spawners.Contexts;

namespace Valheim.SpawnThat.Spawn.Conditions;

public class ConditionWorldAge : ISpawnCondition
{
    private int? MinDays { get; }
    private int? MaxDays { get; }

    public bool CanRunClientSide { get; } = true;
    public bool CanRunServerSide { get; } = true;

    public ConditionWorldAge(int? minDays, int? maxDays)
    {
        MinDays = minDays > 0
            ? minDays
            : null;

        MaxDays = maxDays > 0
            ? maxDays
            : null;
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (MinDays is null && MaxDays is null)
        {
            return true;
        }

        int day = EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds());

        if (MinDays is not null
            && day < MinDays)
        {
            return false;
        }

        if (MaxDays is not null
            && day > MaxDays)
        {
            return false;
        }

        return true;
    }
}
