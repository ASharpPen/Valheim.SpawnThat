using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.World.Maps;

namespace Valheim.SpawnThat.Spawners.Conditions;

public class ConditionAreaIds : ISpawnCondition
{
    private List<string> RequiredAreaIds { get; }
    public bool CanRunClientSide { get; } = true;
    public bool CanRunServerSide { get; } = true;

    public ConditionAreaIds(List<string> requiredAreaIds = null)
    {
        RequiredAreaIds = requiredAreaIds ?? new(0);
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (RequiredAreaIds.Count == 0)
        {
            return true;
        }

        var areaId = MapManager.GetAreaId(context.SpawnerZdo.GetPosition())
            .ToString()
            .ToUpperInvariant()
            .Trim();

        if (RequiredAreaIds.Any(x => x == areaId))
        {
            return true;
        }

        return false;
    }
}