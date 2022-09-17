using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpawnThat.Spawners.Contexts;
using SpawnThat.World.Locations;

namespace SpawnThat.Options.Conditions;

public class ConditionLocation : ISpawnCondition
{
    public List<string> Locations { get; set; }

    public ConditionLocation()
    { }

    public ConditionLocation(params string[] requireOneOfLocations)
    {
        Locations = requireOneOfLocations?
            .Select(x => x.Trim().ToUpperInvariant())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if ((Locations?.Count ?? 0) == 0)
        {
            return true;
        }

        var location = LocationManager
            .GetLocation(context.SpawnerZdo.GetPosition())?
            .LocationName?
            .Trim()?
            .ToUpperInvariant();

        if (location is null)
        {
            return false;
        }

        return Locations.Any(x => x == location);
    }

    public bool IsValid(Vector3 position)
    {
        if ((Locations?.Count ?? 0) == 0)
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

        return Locations.Any(x => x == location);
    }
}
