using UnityEngine;

namespace Valheim.SpawnThat.ServerSide.SpawnModifiers;

public interface ISpawnModifier
{
    void Apply(GameObject entity, ZDO entityZdo);
}
