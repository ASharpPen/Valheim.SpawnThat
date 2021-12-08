using UnityEngine;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.ServerSide.SpawnModifiers;

public class SpawnModifierSetHuntPlayer : ISpawnModifier
{
    private bool HuntPlayer { get; }

    public SpawnModifierSetHuntPlayer(bool huntPlayer = true)
    {
        HuntPlayer = huntPlayer;
    }

    public void Apply(GameObject entity, ZDO entityZdo)
    {
        var baseAI = ComponentCache.GetComponent<BaseAI>(entity);

        if (baseAI != null)
        {
            baseAI.SetHuntPlayer(HuntPlayer);
        }
    }
}
