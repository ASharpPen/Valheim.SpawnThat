using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpawnThat.Spawners.Contexts;
using SpawnThat.World.Maps;

namespace SpawnThat.Options.Conditions;

public class ConditionAreaIds : ISpawnCondition
{
    public List<int> RequiredAreaIds { get; set; } = new();

    internal ConditionAreaIds()
    { }

    public ConditionAreaIds(List<int> requiredAreaIds = null)
    {
        RequiredAreaIds = requiredAreaIds ?? new(0);
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (RequiredAreaIds.Count == 0)
        {
            return true;
        }

        var areaId = MapManager.GetAreaId(context.SpawnerZdo.GetPosition());

        if (RequiredAreaIds.Any(x => x == areaId))
        {
            return true;
        }

        return false;
    }

    public bool IsValid(Vector3 position)
    {
        if (RequiredAreaIds.Count == 0)
        {
            return true;
        }

        var areaId = MapManager.GetAreaId(position);

        if (RequiredAreaIds.Any(x => x == areaId))
        {
            return true;
        }

        return false;
    }

    public bool IsValid(int areaId)
    {
        if (RequiredAreaIds.Count == 0)
        {
            return true;
        }

        if (RequiredAreaIds.Any(x => x == areaId))
        {
            return true;
        }

        return false;
    }
}