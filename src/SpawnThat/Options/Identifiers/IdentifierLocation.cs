using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using SpawnThat.Core;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities.Extensions;
using SpawnThat.World.Locations;

namespace SpawnThat.Options.Identifiers;

public class IdentifierLocation : ISpawnerIdentifier, ICacheableIdentifier
{
    private long _hash;
    private List<string> _locations;

    public List<string> Locations
    {
        get { return _locations; }
        set 
        { 
            _locations = value
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().ToUpperInvariant())
                .ToList();

            SetHash(_locations); 
        }
    }

    internal IdentifierLocation()
    {
    }

    public IdentifierLocation(IEnumerable<string> requireOneOfLocations)
    {
        Locations = requireOneOfLocations?.ToList() ?? new();
    }

    public bool IsValid(IdentificationContext context)
    {
        if ((Locations?.Count ?? 0) == 0)
        {
            return true;
        }

        var location = LocationManager
            .GetLocation(context.Zdo.GetPosition())?
            .LocationName?
            .Trim()?
            .ToUpperInvariant();

#if DEBUG
        Log.LogTrace($"Is location '{location}' in '{_locations.Join()}': {Locations.Any(x => x == location)}");
#endif

        if (location is null)
        {
            return false;
        }

        return Locations.Any(x => x == location);
    }

    private void SetHash(IEnumerable<string> locations)
    {
        _hash = locations.Hash();
    }

    public long GetParameterHash()
    {
        return _hash;
    }

    public int GetMatchWeight() => MatchWeight.Normal;

    public bool Equals(ISpawnerIdentifier other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return 
            other is IdentifierLocation otherIdentifier &&
            _hash == otherIdentifier.GetParameterHash();
    }
}
