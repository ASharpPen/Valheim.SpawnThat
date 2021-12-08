using UnityEngine;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.ServerSide.SpawnModifiers;

public class SpawnModifierSetPatrol : ISpawnModifier
{
    public void Apply(GameObject entity, ZDO entityZdo)
    {
        var baseAI = ComponentCache.GetComponent<BaseAI>(entity);

        if (baseAI != null && baseAI)
        {
            baseAI.SetPatrolPoint();
        }
    }
}
