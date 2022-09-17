using SpawnThat.Options.Modifiers;

namespace SpawnThat.Spawners;

public interface IHaveSpawnModifiers
{
    /// <summary>
    /// Adds an ISpawnModifier to the builder.
    /// If a modifier with the same type already exists, it will be replaced by this one.
    /// </summary>
    void SetModifier<TModifier>(TModifier modifier)
        where TModifier : class, ISpawnModifier;
}
