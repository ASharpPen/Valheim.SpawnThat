namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    internal class PositionConditionForestOutside : ISpawnPositionCondition
    {
        public bool IsValid(PositionContext context)
        {
            return !WorldGenerator.InForest(context.Point);
        }
    }
}
