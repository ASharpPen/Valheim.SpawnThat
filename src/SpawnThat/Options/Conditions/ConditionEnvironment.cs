using System;
using System.Collections.Generic;
using System.Linq;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Options.Conditions;

public class ConditionEnvironment : ISpawnCondition
{
    public string[] Environments { get; set; }

    public ConditionEnvironment()
    { }

    public ConditionEnvironment(params string[] requiredEnvironments)
    {
        Environments = requiredEnvironments
            .Select(x => x.Trim())
            .ToArray();
    }

    public ConditionEnvironment(IEnumerable<string> requiredEnvironments)
    {
        Environments = requiredEnvironments
            .Select(x => x.Trim())
            .ToArray();
    }

    public bool IsValid(SpawnSessionContext sessionContext)
    {
        if ((Environments?.Length ?? 0) == 0)
        {
            return true;
        }

        if (EnvMan.instance.IsNull())
        {
            return true;
        }

        var currentEnv = EnvMan.instance
            .GetCurrentEnvironment()?
            .m_name?
            .Trim();

        if (currentEnv is null)
        {
            return false;
        }

        return Environments.Any(x => string.Equals(x, currentEnv, StringComparison.InvariantCultureIgnoreCase));
    }
}
