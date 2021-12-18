using Valheim.SpawnThat.ServerSide.Contexts;

namespace Valheim.SpawnThat.ServerSide.SpawnConditions;

public class ConditionSpawnDuringDay : ISpawnCondition
{
    public const string ZdoConditionDay = "spawnthat_condition_daytime_day";

    public bool AllowedDuringDay { get; }
    public bool CanRunClientSide => true;
    public bool CanRunServerSide => true;

    public ConditionSpawnDuringDay(bool allowedDuringDay = true)
    {
        AllowedDuringDay = allowedDuringDay;
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (!AllowedDuringDay && EnvMan.instance.IsDay())
        {
            return false;
        }

        return true;
    }
}
