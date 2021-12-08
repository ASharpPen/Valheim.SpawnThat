using UnityEngine;
using Valheim.SpawnThat.ServerSide.Contexts;

namespace Valheim.SpawnThat.ServerSide.SpawnPositionConditions;

public interface ISpawnPositionCondition
{
    bool IsValid(SpawnSessionContext context, Vector3 position);
}
