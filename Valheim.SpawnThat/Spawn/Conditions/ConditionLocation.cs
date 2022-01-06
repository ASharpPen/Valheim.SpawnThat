using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using UnityEngine;
using Valheim.SpawnThat.Spawners.Contexts;
using Valheim.SpawnThat.World.Locations;

namespace Valheim.SpawnThat.Spawn.Conditions;

public class ConditionLocation : ISpawnCondition
{
    private List<string> Locations { get; }

    public bool CanRunClientSide { get; } = true;
    public bool CanRunServerSide { get; } = true;

    public ConditionLocation(params string[] requireOneOfLocations)
    {
        Locations = requireOneOfLocations
            .Select(x => x.Trim().ToUpperInvariant())
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
