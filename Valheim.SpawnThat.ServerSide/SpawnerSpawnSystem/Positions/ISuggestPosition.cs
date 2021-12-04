using UnityEngine;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions;

public interface ISuggestPosition
{
    Vector3? SuggestPosition(SpawnContext context);

    bool IsValidPosition(SpawnContext contet, Vector3 position);
}
