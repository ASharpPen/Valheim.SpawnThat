using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Locations;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionLocation : ISpawnCondition
    {
        private List<string> Locations { get; }

        public ConditionLocation(params string[] requireOneOfLocations)
        {
            Locations = requireOneOfLocations
                .Select(x => x.Trim().ToUpperInvariant())
                .ToList();
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (Locations.Count == 0)
            {
                return true;
            }

            var location = LocationHelper
                .FindLocation(context.SpawnSystemZDO.GetPosition())?
                .LocationName?
                .Trim()?
                .ToUpperInvariant();

            if (location is null)
            {
                return false;
            }

            return Locations.Any(x => x == location);
        }
    }
}
