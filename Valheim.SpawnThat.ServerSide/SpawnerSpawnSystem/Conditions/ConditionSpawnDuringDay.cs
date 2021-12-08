
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionSpawnDuringDay : ISpawnCondition
    {
        public const string ZdoConditionDay = "spawnthat_condition_daytime_day";

        public ConditionSpawnDuringDay(bool allowedDuringDay)
        {
            AllowedDuringDay = allowedDuringDay;
        }

        public bool AllowedDuringDay { get; }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (!AllowedDuringDay && EnvMan.instance.IsDay())
            {
                return false;
            }

            return true;
        }
    }
}
