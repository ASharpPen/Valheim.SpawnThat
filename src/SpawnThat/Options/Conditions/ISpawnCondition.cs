using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.Conditions;

/// <summary>
/// Conditions are requirements that must all be met, for a template to be allowed to spawn.
/// </summary>
public interface ISpawnCondition
{
    bool IsValid(SpawnSessionContext sessionContext);
}
