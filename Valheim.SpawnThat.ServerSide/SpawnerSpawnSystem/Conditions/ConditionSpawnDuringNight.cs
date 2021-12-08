
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionSpawnDuringNight : ISpawnCondition
    {
        public const string ZdoConditionNight = "spawnthat_condition_daytime_night";

        public ConditionSpawnDuringNight(bool allowedDuringNight)
        {
            AllowedDuringNight = allowedDuringNight;
        }

        public bool AllowedDuringNight { get; }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (!AllowedDuringNight && EnvMan.instance.IsNight())
            {
                return false;
            }

            return true;
        }
    }
}
