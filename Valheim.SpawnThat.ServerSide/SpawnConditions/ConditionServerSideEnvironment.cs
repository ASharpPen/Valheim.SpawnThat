using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.ServerSide.Contexts;
using Valheim.SpawnThat.ServerSide.Utilities.World;
using Valheim.SpawnThat.Utilities.World;

namespace Valheim.SpawnThat.ServerSide.SpawnConditions;

public class ConditionServerSideEnvironment : ISpawnCondition
{
    private List<string> RequiredEnvironments { get; }

    /// <summary>
    /// Technically it can, but thats kinda silly.
    /// </summary>
    public bool CanRunClientSide => false;
    public bool CanRunServerSide => true;

    public ConditionServerSideEnvironment(List<string> requiredEnvironments)
    {
        if (requiredEnvironments is null)
        {
            RequiredEnvironments = new(0);
        }
        else
        {
            RequiredEnvironments = requiredEnvironments
                .Select(x => x.Trim().ToUpperInvariant())
                .ToList();
        }
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (RequiredEnvironments.Count == 0)
        {
            return true;
        }

        var position = context.SpawnerZdo.GetPosition();
        var currentEnv = Environment.GetCurrent(position)?
            .m_name?
            .Trim()?
            .ToUpperInvariant();

        return RequiredEnvironments.Any(x => x == currentEnv);
    }
}
