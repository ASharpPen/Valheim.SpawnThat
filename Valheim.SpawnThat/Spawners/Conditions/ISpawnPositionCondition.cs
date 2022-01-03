using UnityEngine;
using Valheim.SpawnThat.Spawners.Contexts;

namespace Valheim.SpawnThat.Spawners.Conditions;

public interface ISpawnPositionCondition
{
    bool IsValid(SpawnSessionContext context, Vector3 position);
}
