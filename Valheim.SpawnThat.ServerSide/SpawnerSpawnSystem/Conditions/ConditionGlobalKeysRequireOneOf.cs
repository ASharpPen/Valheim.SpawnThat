using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions;

/// <summary>
/// TODO: Integrate with Enhanced Progress Tracker.
/// </summary>
public class ConditionGlobalKeysRequireOneOf : ISpawnCondition
{
    public IReadOnlyList<string> Keys { get; }

    public ConditionGlobalKeysRequireOneOf(List<string> requireOneOf)
    {
        Keys = (requireOneOf ?? new(0)).AsReadOnly();
    }

    public ConditionGlobalKeysRequireOneOf(params string[] keys)
    {
        Keys = keys.ToList().AsReadOnly();
    }

    public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
    {
        if (Keys.Count == 0)
        {
            return true;
        }

        return Keys.Any(x => ZoneSystem.instance.GetGlobalKey(x));
    }
}
