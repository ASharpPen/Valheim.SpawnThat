using UnityEngine;

namespace SpawnThat.Spawners.DestructibleSpawner.Identifiers;

internal interface ISpawnerIdentifier
{
    /// <summary>
    /// When identifier is evaluated as a match, it will add this value to the match score.
    /// 
    /// Intended to select the more specific match, when multiple potential templates are
    /// identified for a spawner.
    /// </summary>
    int GetMatchWeight();

    bool IsValid(IdentificationContext context);
}

internal interface ICacheableIdentifier
{
    long GetParameterHash();
}

public static class MatchWeight
{
    /// <summary>
    /// Weight for generic identifiers, such as biome.
    /// </summary>
    public const int Low = 50;

    public const int Normal = 100;

    /// <summary>
    /// Weight for precise identifiers, such as room.
    /// </summary>
    public const int High = 200;
}