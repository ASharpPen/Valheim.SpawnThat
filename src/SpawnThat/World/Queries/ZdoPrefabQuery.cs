﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpawnThat.World.Queries;

public class ZdoPrefabQuery : BaseZdoQuery
{
    private Dictionary<int, int> CachedPrefabResults = new();

    public ZdoPrefabQuery(Vector3 center, int range) : base(center, range)
    {
    }

    public int CountEntities(int prefabHash, Predicate<ZDO> condition = null)
    {
        Initialize();

        if (CachedPrefabResults.TryGetValue(prefabHash, out int cachedCount))
        {
            return cachedCount;
        }

        int instances = 0;

        // Search zdo's with same prefab
        if (condition is null)
        {
            instances = Zdos.Count(x =>
                IsWithinRange(x, prefabHash));
        }
        else
        {
            instances = Zdos.Count(x =>
                IsWithinRange(x, prefabHash) &&
                condition(x));
        }

        CachedPrefabResults[prefabHash] = instances;

        return instances;
    }

    public bool HasAny(int prefabHash)
    {
        Initialize();

        if (CachedPrefabResults.TryGetValue(prefabHash, out int cachedCount))
        {
            return cachedCount > 0;
        }

        return Zdos.Any(x => IsWithinRange(x, prefabHash));
    }

    private bool IsWithinRange(ZDO zdo, int prefabId)
    {
        if (zdo.m_prefab != prefabId)
        {
            return false;
        }

        return IsWithinRange(zdo);
    }
}
