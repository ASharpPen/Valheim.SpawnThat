using System.Collections.Generic;
using System.Linq;
using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.Conditions;

/// <summary>
/// TODO: Integrate with Enhanced Progress Tracker when server-side.
/// </summary>
public class ConditionGlobalKeysRequiredMissing : ISpawnCondition
{
    public List<string> RequiredMissing { get; set; }

    internal ConditionGlobalKeysRequiredMissing()
    { }

    public ConditionGlobalKeysRequiredMissing(params string[] requiredMissing)
    {
        RequiredMissing = requiredMissing.ToList();
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
