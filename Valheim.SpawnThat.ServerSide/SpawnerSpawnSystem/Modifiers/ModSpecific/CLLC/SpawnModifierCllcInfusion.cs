using CreatureLevelControl;
using System;
using UnityEngine;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers.ModSpecific.CLLC;

public class SpawnModifierInfusion : ISpawnModifier
{
    public CreatureInfusion? Infusion { get; }

    public SpawnModifierInfusion(string infusionName)
    {
        if (Enum.TryParse(infusionName, true, out CreatureInfusion infusion))
        {
            Infusion = infusion;
        }
        else
        {
            Log.LogWarning($"CLLC infusion '{infusion}' formatted wrong. Ignoring spawn modifier.");
        }
    }

    public SpawnModifierInfusion(CreatureInfusion infusion)
    {
        Infusion = infusion;
    }

    public void Modify(Models.SpawnContext context, GameObject entity, ZDO entityZdo)
    {
        if (Infusion is null)
        {
            return;
        }

        var character = ComponentCache.GetComponent<Character>(entity);

        if (character is null)
        {
            return;
        }

        Log.LogTrace($"Setting infusion '{Infusion}' for '{entity.name}'.");
        API.SetInfusionCreature(character, Infusion.Value);
    }
}
