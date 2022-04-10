using System.Collections.Generic;
using System.Linq;
using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.Conditions;

/// <summary>
/// Global keys required to allow spawning.
/// All listed keys must be present.
/// </summary>
public class ConditionGlobalKeysRequired : ISpawnCondition
{
    public string[] Required { get; set; }

    public ConditionGlobalKeysRequired()
    { }

    public ConditionGlobalKeysRequired(params string[] requiredKeys)
    {
        Required = requiredKeys
            .Distinct()
            .ToArray();
    }

    public ConditionGlobalKeysRequired(IEnumerable<string> requiredKeys)
    {
        Required = requiredKeys
            .Distinct()
            .ToArray();
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (Required.Length == 0)
        {
            return true;
        }

        return !Required.All(x => ZoneSystem.instance.GetGlobalKey(x));
    }
}
