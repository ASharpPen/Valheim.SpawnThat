using System.Collections.Generic;
using System.Linq;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Spawners.DestructibleSpawner.Identifiers;

internal class IdentifierName : ISpawnerIdentifier, ICacheableIdentifier
{
    private int _hash;
    private ICollection<string> _names;

    public ICollection<string> Names 
    { 
        get { return _names; }
        set { SetHash(_names = value); }
    }

    internal IdentifierName()
    {
    }

    public IdentifierName(params string[] names)
    {
        Names = names
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .ToArray();
    }

    public bool IsValid(IdentificationContext context)
    {
        // Ignore if empty.
        if ((Names?.Count ?? 0) == 0)
        {
            return true;
        }

        var cleanedName = context.Target.GetCleanedName();
        var match = Names.Any(x => x == cleanedName);

        return match;
    }

    private void SetHash(ICollection<string> names)
    {
        _hash = 0;
        
        foreach (var name in names)
        {
            unchecked
            {
                _hash += name.GetStableHashCode();
            }
        }
    }

    public long GetParameterHash()
    {
        return _hash;
    }

    public int GetMatchWeight() => MatchWeight.Normal;
}
