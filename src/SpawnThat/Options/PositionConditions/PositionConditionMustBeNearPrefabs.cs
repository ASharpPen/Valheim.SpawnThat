using System.Collections.Generic;
using System.Linq;
using SpawnThat.Spawners.Contexts;
using SpawnThat.World.Queries;
using UnityEngine;

namespace SpawnThat.Options.PositionConditions;

public class PositionConditionMustBeNearPrefabs : ISpawnPositionCondition
{
    private HashSet<int> _prefabHashes;
    private List<string> _prefabs;

    private HashSet<int> PrefabHashes => _prefabHashes ?? _prefabs
                .Select(x => x.GetStableHashCode())
                .ToHashSet();

    public List<string> Prefabs
    {
        get => _prefabs;
        set
        {
            _prefabs = value
                .Select(x => x?.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            _prefabHashes = null;
        }
    }

    public int Distance { get; set; } = 32;

    public PositionConditionMustBeNearPrefabs()
    {
    }

    public PositionConditionMustBeNearPrefabs(
        List<string> prefabNames,
        int? distance = 32)
    {
        Distance = distance ?? 32;

        _prefabs = prefabNames
            .Select(x => x?.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();
    }

    public bool IsValid(SpawnSessionContext context, Vector3 position)
    {
        if ((Prefabs?.Count ?? 0) == 0 ||
            Distance <= 0)
        {
            return true;
        }

        var query = new PrefabQuery(position, Distance);
        return query.HasAny(PrefabHashes);
    }

    private class PrefabQuery : BaseZdoQuery
    {
        public PrefabQuery(Vector3 center, int range) : base(center, range)
        {
        }

        public bool HasAny(HashSet<int> prefabHashes)
        {
            Initialize();

            return Zdos.Any(x => 
                prefabHashes.Contains(x.m_prefab) && 
                IsWithinRange(x));
        }
    }
}
