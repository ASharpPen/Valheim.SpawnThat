using UnityEngine;

namespace SpawnThat.Options.Modifiers;

public interface ISpawnModifier
{
    void Modify(GameObject entity, ZDO entityZdo);
}
