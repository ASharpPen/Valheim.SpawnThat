using Valheim.SpawnThat.Maps.Managers;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionAreaSpawnChance : ISpawnCondition
    {
        private double AreaChance { get; }

        public ConditionAreaSpawnChance(double areaChance )
        {
            AreaChance = areaChance;
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (AreaChance <= 0)
            {
                return false;
            }
            else if (AreaChance >= 100)
            {
                return true;
            }

            var areaChance = MapManager.GetAreaChance(context.SpawnSystemZDO.GetPosition(), template.Index);

            return areaChance * 100 > (100 - AreaChance);
        }
    }
}
