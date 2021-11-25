using UnityEngine;
using Valheim.SpawnThat.Core.Cache;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers;

public class SpawnModifierSetEventCreature : ISpawnModifier
{
    public bool EventCreature { get; }

    public SpawnModifierSetEventCreature(bool eventCreature = true)
    {
        EventCreature = eventCreature;
    }

    public void Modify(SpawnContext context, GameObject entity, ZDO entityZdo)
    {
        var component = ComponentCache.GetComponent<MonsterAI>(entity);

        if (component != null)
        {
            component.SetEventCreature(EventCreature);
        }
    }
}
