using UnityEngine;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers;

public class SpawnModifierSetTamed : ISpawnModifier
{
    private bool Tamed { get; }

    public SpawnModifierSetTamed(bool tamed = true)
    {
        Tamed = tamed;
    }

    public void Modify(Models.SpawnContext context, GameObject entity, ZDO entityZdo)
    {
        var character = ComponentCache.GetComponent<Character>(entity);

        if (character != null)
        {
#if DEBUG
            Log.LogDebug($"Setting tamed = " + Tamed);
#endif

            character.SetTamed(Tamed);
        }
    }
}
