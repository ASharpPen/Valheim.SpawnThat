using Valheim.SpawnThat.ServerSide.Contexts;

namespace Valheim.SpawnThat.ServerSide.SpawnConditions;

public class ConditionSpawnDuringNight : ISpawnCondition
{
    public const string ZdoConditionNight = "spawnthat_condition_daytime_night";

    public ConditionSpawnDuringNight(bool allowedDuringNight = true)
    {
        AllowedDuringNight = allowedDuringNight;
    }

    public bool AllowedDuringNight { get; }

    public bool IsValid(SpawnSessionContext context)
    {
        if (!AllowedDuringNight && EnvMan.instance.IsNight())
        {
            return false;
        }

        return true;
    }
}
