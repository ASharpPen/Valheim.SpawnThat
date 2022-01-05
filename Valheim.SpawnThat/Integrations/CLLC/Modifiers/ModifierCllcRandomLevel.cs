using CreatureLevelControl;
using UnityEngine;
using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Spawn.Modifiers;

namespace Valheim.SpawnThat.Integrations.CLLC.Modifiers;

public class ModifierCllcRandomLevel : ISpawnModifier
{
    public void Modify(GameObject entity, ZDO entityZdo)
    {
        var character = ComponentCache.Get<Character>(entity);

        if (character != null)
        {
            API.LevelRand(character);
        }
    }
}
