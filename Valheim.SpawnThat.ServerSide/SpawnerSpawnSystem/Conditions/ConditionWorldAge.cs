
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionWorldAge : ISpawnCondition
    {
        private int? MinDays { get; }

        private int? MaxDays { get; }

        public ConditionWorldAge(int? minDays, int maxDays)
        {
            MinDays = minDays > 0
                ? minDays
                : null;

            MaxDays = maxDays > 0
                ? maxDays
                : null;
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (MinDays is null && MaxDays is null)
            {
                return true;
            }

            int day = EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds());

            if (MinDays is not null
                && day < MinDays)
            {
                return false;
            }

            if (MaxDays is not null
                && day > MaxDays)
            {
                return false;
            }

            return true;
        }
    }
}
