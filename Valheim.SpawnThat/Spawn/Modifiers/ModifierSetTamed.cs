using UnityEngine;
using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawn.Modifiers;

public class ModifierSetTamed : ISpawnModifier
{
    public ModifierSetTamed(bool tamed)
    {
        Tamed = tamed;
    }

    public bool Tamed { get; }

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
