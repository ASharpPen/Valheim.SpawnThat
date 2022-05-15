using System.Collections.Generic;
using System.Linq;
using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.Conditions;

public class ConditionGlobalKeysAny : ISpawnCondition
{
    public string[] Keys { get; set; }

    public ConditionGlobalKeysAny()
    { }

    public ConditionGlobalKeysAny(params string[] globalKeys)
    {
        Keys = globalKeys?
            .Distinct()
            .ToArray();
    }

    public ConditionGlobalKeysAny(IEnumerable<string> globalKeys)
    {
        Keys = globalKeys?
            .Distinct()
            .ToArray();
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if ((Keys?.Length ?? 0) == 0)
        {
            return true;
        }

        return Keys.Any(x => ZoneSystem.instance.GetGlobalKey(x));
    }
}
