using CreatureLevelControl;
using UnityEngine;
using SpawnThat.Caches;
using SpawnThat.Options.Modifiers;

namespace SpawnThat.Integrations.CLLC.Modifiers;

public class ModifierCllcRandomLevel : ISpawnModifier
{
    public ModifierCllcRandomLevel()
    { }

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        var character = ComponentCache.Get<Character>(entity);

        if (character != null)
        {
            API.LevelRand(character);
        }
    }
}
