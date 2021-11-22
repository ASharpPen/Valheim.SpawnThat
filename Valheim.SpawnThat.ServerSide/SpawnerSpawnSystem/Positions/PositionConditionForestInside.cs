namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    public class PositionConditionForestInside : ISpawnPositionCondition
    {
        public bool IsValid(PositionContext context)
        {
            return WorldGenerator.InForest(context.Point);
        }
    }
}
