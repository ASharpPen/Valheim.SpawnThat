using UnityEngine;

namespace SpawnThat.Options.Modifiers;

/// <summary>
/// Modifiers are run after an entity is spawned.
/// 
/// Intended to allow for post-spawn changes.
/// </summary>
public interface ISpawnModifier
{
    void Modify(GameObject entity, ZDO entityZdo);
}
