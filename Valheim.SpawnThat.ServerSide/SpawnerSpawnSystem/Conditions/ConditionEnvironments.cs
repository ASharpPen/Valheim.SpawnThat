using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionEnvironments : ISpawnCondition
    {
        public const string ZdoCondition = "spawnthat_condition_environments";

        private List<string> RequiredEnvironments { get; }

        public ConditionEnvironments(List<string> requiredEnvironments)
        {
            if (requiredEnvironments is null)
            {
                RequiredEnvironments = new(0);
            }
            else
            {
                RequiredEnvironments = requiredEnvironments
                    .Select(x => x.Trim().ToUpperInvariant())
                    .ToList();
            }
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (RequiredEnvironments.Count == 0)
            {
                return true;
            }

            var currentEnv = EnvMan.instance.GetCurrentEnvironment()?.m_name?.Trim()?.ToUpperInvariant();

            return RequiredEnvironments.Any(x => x == currentEnv);
        }
    }
}
