using Valheim.SpawnThat.Spawners.Contexts;

namespace Valheim.SpawnThat.Spawn.Conditions;

public interface ISpawnCondition
{
    bool CanRunClientSide { get; }

    bool CanRunServerSide { get; }

    bool IsValid(SpawnSessionContext sessionContext);
}
