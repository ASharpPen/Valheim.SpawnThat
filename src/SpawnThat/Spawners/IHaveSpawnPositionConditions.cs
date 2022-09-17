using SpawnThat.Options.PositionConditions;

namespace SpawnThat.Spawners;

public interface IHaveSpawnPositionConditions
{
    /// <summary>
    /// Adds an ISpawnPositionCondition to the builder.
    /// If a condition with the same type already exists, it will be replaced by this one.
    /// </summary>
    void SetPositionCondition<TCondition>(TCondition condition)
        where TCondition : class, ISpawnPositionCondition;
}
