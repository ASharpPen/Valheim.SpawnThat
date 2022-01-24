using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.World.Locations;

namespace Valheim.SpawnThat.Spawn.PositionConditions;

public class PositionConditionLocation : ISpawnPositionCondition
{
    public List<string> LocationNames { get; set; }

    public PositionConditionLocation()
    { }

    public PositionConditionLocation(IEnumerable<string> locationNames)
    {
        LocationNames = locationNames?
            .Select(x => x.Trim().ToUpperInvariant())?
            .ToList();
    }

    public PositionConditionLocation(params string[] locationNames)
    {
        LocationNames = locationNames?
            .Select(x => x.Trim().ToUpperInvariant())?
            .ToList();
    }

    public bool IsValid(SpawnSessionContext context, Vector3 position)
    {
        if ((LocationNames?.Count ?? 0) == 0)
        {
            return true;
        }

        var location = LocationManager
             .GetLocation(position)?
             .LocationName?
             .Trim()?
             .ToUpperInvariant();

        if (location is null)
        {
            return false;
        }

        return LocationNames.Any(x => x == location);
    }
}
