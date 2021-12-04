
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionIsDay : ISpawnCondition
    {
        public const string ZdoConditionDay = "spawnthat_condition_daytime_day";

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            return EnvMan.instance.IsDay();
        }
    }
}
