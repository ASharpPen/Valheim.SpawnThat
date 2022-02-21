using SpawnThat.Options.Conditions;

namespace SpawnThat.Spawners;

public interface IHaveSpawnConditions
{
    /// <summary>
    /// Adds an ISpawnCondition to the builder.
    /// If a condition with the same type already exists, it will be replaced by this one.
    /// </summary>
    void SetCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnCondition;
}
