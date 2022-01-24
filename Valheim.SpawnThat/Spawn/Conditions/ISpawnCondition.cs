using Valheim.SpawnThat.Spawners.Contexts;
using YamlDotNet.Serialization;

namespace Valheim.SpawnThat.Spawn.Conditions;

public interface ISpawnCondition
{
    bool IsValid(SpawnSessionContext sessionContext);
}
