using Valheim.SpawnThat.ServerSide.Contexts;

namespace Valheim.SpawnThat.ServerSide.SpawnConditions;

public interface ISpawnCondition
{
    bool IsValid(SpawnSessionContext sessionContext);
}
