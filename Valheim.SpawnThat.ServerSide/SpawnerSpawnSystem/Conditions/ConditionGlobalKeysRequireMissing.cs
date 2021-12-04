using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    /// <summary>
    /// TODO: Integrate with Enhanced Progress Tracker.
    /// </summary>
    public class ConditionGlobalKeysRequireMissing : ISpawnCondition
    {
        private IList<string> RequiredMissing { get; set; }

        public ConditionGlobalKeysRequireMissing(params string[] requiredMissing)
        {
            RequiredMissing = requiredMissing;
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (RequiredMissing.Count == 0)
            {
                return true;
            }

            return !RequiredMissing.Any(x => ZoneSystem.instance.GetGlobalKey(x));
        }
    }
}
