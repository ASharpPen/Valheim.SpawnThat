using UnityEngine;
using Valheim.SpawnThat.Core.Cache;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers;

public class SpawnModifierSetHuntPlayer : ISpawnModifier
{
    private bool HuntPlayer { get; }

    public SpawnModifierSetHuntPlayer(bool huntPlayer = true)
    {
        HuntPlayer = huntPlayer;
    }

    public void Modify(SpawnContext context, GameObject entity, ZDO entityZdo)
    {
        var baseAI = ComponentCache.GetComponent<BaseAI>(entity);

        if (baseAI != null)
        {
            baseAI.SetHuntPlayer(HuntPlayer);
        }
    }
}
