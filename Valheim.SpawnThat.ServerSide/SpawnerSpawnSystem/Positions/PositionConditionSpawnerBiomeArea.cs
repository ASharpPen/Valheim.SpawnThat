using Valheim.SpawnThat.ServerSide.Map;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    /// <summary>
    /// This seems stupidly volatile.
    /// </summary>
    public class PositionConditionSpawnerBiomeArea : ISpawnPositionCondition
    {
        public PositionConditionSpawnerBiomeArea()
        {
        }

        public bool IsValid(PositionContext context)
        {
            var spawnerZone = WorldData.GetZone(context.SessionContext.SpawnSystemZDO.m_sector);

            var spawnZone = WorldData.GetZone(context.Point);

            return spawnerZone.BiomeArea == spawnZone.BiomeArea;
        }
    }
}
