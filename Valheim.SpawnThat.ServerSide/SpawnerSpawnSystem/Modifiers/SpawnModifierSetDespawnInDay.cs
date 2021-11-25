using UnityEngine;
using Valheim.SpawnThat.Core.Cache;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers;

public class SpawnModifierSetDespawnInDay : ISpawnModifier
{
    public bool DespawnAtDay { get; }

    public SpawnModifierSetDespawnInDay(bool despawnAtDay = true)
    {
        DespawnAtDay = despawnAtDay;
    }

    public void Modify(SpawnContext context, GameObject entity, ZDO entityZdo)
    {
        var component = ComponentCache.GetComponent<MonsterAI>(entity);

        if (component != null)
        {
            component.SetDespawnInDay(DespawnAtDay);
        }
    }
}
