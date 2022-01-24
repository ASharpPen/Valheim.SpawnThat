using UnityEngine;
using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.PositionConditions;

public interface ISpawnPositionCondition
{
    bool IsValid(SpawnSessionContext context, Vector3 position);
}
