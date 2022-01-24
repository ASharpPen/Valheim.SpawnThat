using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.Conditions;

public interface ISpawnCondition
{
    bool IsValid(SpawnSessionContext sessionContext);
}
