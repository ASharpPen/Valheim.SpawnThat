using UnityEngine;
using Valheim.SpawnThat.Spawners.Contexts;

namespace Valheim.SpawnThat.Spawn.PositionConditions;

public interface ISpawnPositionCondition
{
    bool IsValid(SpawnSessionContext context, Vector3 position);
}
