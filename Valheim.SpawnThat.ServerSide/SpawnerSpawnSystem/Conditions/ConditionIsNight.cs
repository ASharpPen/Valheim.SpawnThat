
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionIsNight : ISpawnCondition
    {
        public const string ZdoConditionNight = "spawnthat_condition_daytime_night";

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            return EnvMan.instance.IsNight();
        }
    }
}
