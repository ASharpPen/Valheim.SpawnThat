using UnityEngine;

namespace Valheim.SpawnThat.Spawn.Modifiers;

public interface ISpawnModifier
{
    void Modify(GameObject entity, ZDO entityZdo);
}
