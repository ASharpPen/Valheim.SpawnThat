using UnityEngine;
using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.PositionConditions;

/// <summary>
/// Spawn position conditions are specialized conditions that must all be met, for a template to be allowed to spawn.
/// 
/// Typically these are checked multiple times, while the game samples multiple spots in the world for a valid position.
/// </summary>
public interface ISpawnPositionCondition
{
    bool IsValid(SpawnSessionContext context, Vector3 position);
}
