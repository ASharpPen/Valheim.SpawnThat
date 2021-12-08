using UnityEngine;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.ServerSide.SpawnModifiers;

public class SpawnModifierSetLevel : ISpawnModifier
{
    public int Level { get; }

    public SpawnModifierSetLevel(int level)
    {
        Level = level;
    }

    public void Apply(GameObject entity, ZDO entityZdo)
    {
        if (Level <= 0)
        {
            return;
        }

        var character = ComponentCache.GetComponent<Character>(entity);

        if (character != null)
        {
            character.SetLevel(Level);
        }
    }
}
