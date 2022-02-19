using SpawnThat.Options.Identifiers;

namespace SpawnThat.Spawners;

public interface IHaveSpawnerIdentifiers
{
    /// <summary>
    /// Set spawner identifier.
    /// If an identifier with the same type already exists, it will be replaced by this one.
    /// </summary>
    void SetIdentifier<T>(T identifier)
        where T : class, ISpawnerIdentifier;
}
