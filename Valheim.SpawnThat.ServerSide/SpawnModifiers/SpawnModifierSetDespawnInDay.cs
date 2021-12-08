using UnityEngine;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.ServerSide.SpawnModifiers;

public class SpawnModifierSetDespawnInDay : ISpawnModifier
{
    public bool DespawnAtDay { get; }

    public SpawnModifierSetDespawnInDay(bool despawnAtDay = true)
    {
        DespawnAtDay = despawnAtDay;
    }

    public void Apply(GameObject entity, ZDO entityZdo)
    {
        var component = ComponentCache.GetComponent<MonsterAI>(entity);

        if (component != null && component)
        {
            component.SetDespawnInDay(DespawnAtDay);
        }
    }
}
