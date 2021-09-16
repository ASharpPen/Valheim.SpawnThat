using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Maps.Managers;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionAreaIds : ISpawnCondition
    {
        private List<string> RequiredAreaIds { get; }

        public ConditionAreaIds(List<string> requiredAreaIds = null)
        {
            RequiredAreaIds = requiredAreaIds ?? new(0);
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (RequiredAreaIds.Count == 0)
            {
                return true;
            }

            var areaId = MapManager.GetAreaId(context.SpawnSystemZDO.GetPosition())
                .ToString()
                .ToUpperInvariant()
                .Trim();

            if (RequiredAreaIds.Any(x => x == areaId))
            {
                return true;
            }

            return false;
        }
    }
}
