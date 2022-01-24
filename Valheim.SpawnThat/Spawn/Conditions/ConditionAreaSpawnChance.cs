using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.World.Maps;

namespace Valheim.SpawnThat.Spawn.Conditions;

public class ConditionAreaSpawnChance : ISpawnCondition
{
    public double AreaChance { get; set; }
    public int EntityId { get; set; }

    public ConditionAreaSpawnChance()
    { }

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
