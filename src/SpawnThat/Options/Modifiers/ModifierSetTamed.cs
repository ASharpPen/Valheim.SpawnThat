using UnityEngine;
using SpawnThat.Caches;
using SpawnThat.Core;

namespace SpawnThat.Options.Modifiers;

public class ModifierSetTamed : ISpawnModifier
{
    public bool Tamed { get; set; }

    internal ModifierSetTamed()
    { }

    public ModifierSetTamed(bool tamed)
    {
        Tamed = tamed;
    }

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        var character = ComponentCache.Get<Character>(entity);

        if (character is null)
        {
            return;
        }

#if DEBUG
        Log.LogDebug($"Setting tamed");
#endif
        character.SetTamed(Tamed);
    }
}
