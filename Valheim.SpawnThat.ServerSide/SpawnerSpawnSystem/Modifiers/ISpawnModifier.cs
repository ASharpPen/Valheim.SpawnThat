using UnityEngine;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers
{
    public interface ISpawnModifier
    {
        void Modify(SpawnContext context, GameObject entity, ZDO entityZdo);
    }
}
