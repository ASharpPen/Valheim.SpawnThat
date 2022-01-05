using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.World.Maps;

namespace Valheim.SpawnThat.Spawn.Conditions;

public class ConditionAreaIds : ISpawnCondition
{
    private List<int> RequiredAreaIds { get; }
    public bool CanRunClientSide { get; } = true;
    public bool CanRunServerSide { get; } = true;

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
}