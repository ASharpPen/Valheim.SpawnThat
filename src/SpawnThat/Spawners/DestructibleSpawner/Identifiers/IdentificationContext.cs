using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnThat.Spawners.DestructibleSpawner.Identifiers;

internal class IdentificationContext
{
    /// <summary>
    /// Object being identified.
    /// </summary>
    public GameObject Target { get; set; }

    /// <summary>
    /// ZDO of <c>Target</c>.
    /// </summary>
    public ZDO Zdo { get; set; }

    private Dictionary<Type, Dictionary<long, bool>> IdentifierResultCache = new();

    internal bool TryGetCached(ICacheableIdentifier identifier, out bool cached)
    {
        if (IdentifierResultCache.TryGetValue(identifier.GetType(), out var identifierCache))
        {
            long parameterHash = identifier.GetParameterHash();

            if (identifierCache.TryGetValue(parameterHash, out cached))
            {
                return true;
            }
        }

        cached = false;
        return false;
    }    

    internal void CacheIdentifierResult(ICacheableIdentifier identifier, bool result)
    {
        Type identifierType = identifier.GetType();
        long identifierHash = identifier.GetParameterHash();

        if (IdentifierResultCache.TryGetValue(identifierType, out var identifierCache))
        {
            identifierCache[identifierHash] = result;
        }
        else
        {
            IdentifierResultCache[identifierType] = new(){{ identifierHash, result }};
        }
    }
}
