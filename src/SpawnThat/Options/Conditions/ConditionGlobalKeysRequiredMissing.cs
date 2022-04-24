using System.Collections.Generic;
using System.Linq;
using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.Conditions;

/// <summary>
/// TODO: Integrate with Enhanced Progress Tracker when server-side.
/// </summary>
public class ConditionGlobalKeysRequiredMissing : ISpawnCondition
{
    public HashSet<string> RequiredMissing { get; set; }

    public ConditionGlobalKeysRequiredMissing()
    { }

    public ConditionGlobalKeysRequiredMissing(params string[] requiredMissing)
    {
        RequiredMissing = requiredMissing.ToHashSet();
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
