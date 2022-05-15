using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using SpawnThat.Core;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Options.Identifiers;

public class IdentifierName : ISpawnerIdentifier
{
    private long _hash;
    private ICollection<string> _names;

    public ICollection<string> Names
    {
        get { return _names; }
        set 
        {
            _names = value
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToList();

            SetHash(_names);
        }
    }

    internal IdentifierName()
    {
    }

    public IdentifierName(params string[] names)
    {
        Names = names ?? new string[0];
    }

    public IdentifierName(ICollection<string> names)
    {
        Names = names ?? new List<string>(0);
    }

    public bool IsValid(IdentificationContext context)
    {
        // Ignore if empty.
        if ((Names?.Count ?? 0) == 0)
        {
            return true;
        }

        var cleanedName = context.Target.GetCleanedName();

#if DEBUG
        Log.LogTrace($"Is name '{cleanedName}' in '{Names.Join()}': {Names.Any(x => x == cleanedName)}");
#endif

        var match = Names.Any(x => x == cleanedName);

        return match;
    }

    private void SetHash(ICollection<string> names)
    {
        _hash = names.Hash();
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

        return other is IdentifierName otherIdentifier &&
            _hash == otherIdentifier.GetParameterHash();
    }
}
