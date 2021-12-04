
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions.ModSpecific.CLLC
{
    public class ConditionWorldLevel : ISpawnCondition
    {
        private int? MinWorldLevel { get; }

        private int? MaxWorldLevel { get; }

        public ConditionWorldLevel(int? minWorldLevel, int? maxWorldLevel)
        {
            MinWorldLevel = minWorldLevel > 0
                ? minWorldLevel
                : null;

            MaxWorldLevel = maxWorldLevel > 0
                ? maxWorldLevel
                : null;
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (MinWorldLevel is null && MaxWorldLevel is null)
            {
                return true;
            }

            int worldLevel = CreatureLevelControl.API.GetWorldLevel();

            if (MinWorldLevel is not null
                && worldLevel < MinWorldLevel)
            {
                return false;
            }

            if (MaxWorldLevel is not null
                && worldLevel > MaxWorldLevel)
            {
                return false;
            }

            return true;
        }
    }
}
