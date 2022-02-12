using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.Conditions;

public class ConditionWorldAge : ISpawnCondition
{
    public int? MinDays { get; set; }
    public int? MaxDays { get; set; }

    internal ConditionWorldAge()
    { }

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
