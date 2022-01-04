using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Spawners.Contexts;

namespace Valheim.SpawnThat.Spawners.Conditions;

/// <summary>
/// TODO: Integrate with Enhanced Progress Tracker when server-side.
/// </summary>
public class ConditionGlobalKeysRequireMissing : ISpawnCondition
{
    private IList<string> RequiredMissing { get; set; }

    public bool CanRunClientSide { get; } = true;
    public bool CanRunServerSide { get; } = false;

    public ConditionGlobalKeysRequireMissing(params string[] requiredMissing)
    {
        RequiredMissing = requiredMissing;
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (RequiredMissing.Count == 0)
        {
            return true;
        }

        return !RequiredMissing.Any(x => ZoneSystem.instance.GetGlobalKey(x));
    }
}
