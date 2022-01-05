using Valheim.SpawnThat.Spawn.Conditions;
using Valheim.SpawnThat.Spawners.Contexts;

namespace Valheim.SpawnThat.Integrations.CLLC.Conditions;

public class ConditionWorldLevel : ISpawnCondition
{
    private int? MinWorldLevel { get; }
    private int? MaxWorldLevel { get; }

    public bool CanRunClientSide { get; } = true;
    public bool CanRunServerSide { get; } = true;

    public ConditionWorldLevel(int? minWorldLevel, int? maxWorldLevel)
    {
        MinWorldLevel = minWorldLevel > 0
            ? minWorldLevel
            : null;

        MaxWorldLevel = maxWorldLevel > 0
            ? maxWorldLevel
            : null;
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (MinWorldLevel is null && MaxWorldLevel is null)
        {
            return true;
        }

        int worldLevel = CreatureLevelControl.API.GetWorldLevel();

        if (MinWorldLevel is not null
            && worldLevel < MinWorldLevel)
        {
            return false;
        }

        if (MaxWorldLevel is not null
            && worldLevel > MaxWorldLevel)
        {
            return false;
        }

        return true;
    }
}
