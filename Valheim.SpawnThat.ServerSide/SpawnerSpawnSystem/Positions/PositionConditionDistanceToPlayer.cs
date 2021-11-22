
namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    public class PositionConditionDistanceToPlayer : ISpawnPositionCondition
    {
        private int Distance { get; }

        public PositionConditionDistanceToPlayer(int distance)
        {
            Distance = distance;
        }

        public bool IsValid(PositionContext context)
        {
            foreach (ZDO player in ZNet.instance.GetAllCharacterZDOS())
            {
                if (Utils.DistanceXZ(player.GetPosition(), context.Point) < Distance)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
