using Valheim.SpawnThat.ServerSide.Contexts;

namespace Valheim.SpawnThat.ServerSide.SpawnConditions;

public interface ISpawnCondition
{
    bool CanRunClientSide { get; }

    bool CanRunServerSide { get; }

    bool IsValid(SpawnSessionContext sessionContext);
}
