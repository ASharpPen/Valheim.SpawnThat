
namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions;

public class PositionConditionForest : ISpawnPositionCondition
{
    public PositionConditionForest(bool spawnInForest, bool spawnOutsideForest)
    {
        SpawnInForest = spawnInForest;
        SpawnOutsideForest = spawnOutsideForest;
    }

    public bool SpawnInForest { get; }
    public bool SpawnOutsideForest { get; }

    public bool IsValid(PositionContext context)
    {
        bool inForest = WorldGenerator.InForest(context.Point);

        if (!SpawnInForest && inForest)
        {
            return false;
        }

        if (!SpawnOutsideForest && !inForest)
        {
            return false;
        }

        return true;
    }
}
