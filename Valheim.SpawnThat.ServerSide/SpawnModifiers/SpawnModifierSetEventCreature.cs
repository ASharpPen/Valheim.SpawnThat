using UnityEngine;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.ServerSide.SpawnModifiers;

public class SpawnModifierSetEventCreature : ISpawnModifier
{
    public bool EventCreature { get; }

    public SpawnModifierSetEventCreature(bool eventCreature = true)
    {
        EventCreature = eventCreature;
    }

    public void Apply(GameObject entity, ZDO entityZdo)
    {
        var component = ComponentCache.GetComponent<MonsterAI>(entity);

        if (component != null && component)
        {
            component.SetEventCreature(EventCreature);
        }
    }
}
