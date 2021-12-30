using UnityEngine;

namespace Valheim.SpawnThat.Spawners.Modifiers;

public interface ISpawnModifier
{
    bool CanRun();

    void Apply(GameObject entity, ZDO entityZdo);
}
