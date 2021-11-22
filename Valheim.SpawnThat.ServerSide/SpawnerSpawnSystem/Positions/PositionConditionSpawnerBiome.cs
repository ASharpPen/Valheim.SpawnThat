using Valheim.SpawnThat.Utilities.World;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    public class PositionConditionSpawnerBiome : ISpawnPositionCondition
    {
        public PositionConditionSpawnerBiome()
        {
        }

        public bool IsValid(PositionContext context)
        {
            var spawnerBiome = WorldData.GetZone(context.SessionContext.SpawnSystemZDO.m_sector);

            var posBiome = WorldData.GetZone(context.Point);

            return spawnerBiome == posBiome;
        }
    }
}
