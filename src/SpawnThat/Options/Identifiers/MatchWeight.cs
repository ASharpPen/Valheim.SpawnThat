namespace SpawnThat.Options.Identifiers;

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