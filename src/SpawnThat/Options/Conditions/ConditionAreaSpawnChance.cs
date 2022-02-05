using SpawnThat.Spawners.Contexts;
using SpawnThat.World.Maps;

namespace SpawnThat.Options.Conditions;

public class ConditionAreaSpawnChance : ISpawnCondition
{
    public float AreaChance { get; set; }
    public int EntityId { get; set; }

    internal ConditionAreaSpawnChance()
    { }

    public ConditionAreaSpawnChance(float areaChance, int entityId)
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
