using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.World.Maps;

namespace Valheim.SpawnThat.Spawners.Conditions;

public class ConditionAreaSpawnChance : ISpawnCondition
{
    private double AreaChance { get; }
    public int EntityId { get; }

    public bool CanRunClientSide { get; } = true;
    public bool CanRunServerSide { get; } = true;

    public ConditionAreaSpawnChance(double areaChance, int entityId)
    {
        AreaChance = areaChance;
        EntityId = entityId;
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (AreaChance <= 0)
        {
            return false;
        }
        else if (AreaChance >= 100)
        {
            return true;
        }

        var areaChance = MapManager.GetAreaChance(context.SpawnerZdo.GetPosition(), EntityId);

        return areaChance * 100 > 100 - AreaChance;
    }
}
