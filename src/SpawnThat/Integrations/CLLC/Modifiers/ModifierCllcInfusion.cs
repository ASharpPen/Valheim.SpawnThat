using System;
using CreatureLevelControl;
using UnityEngine;
using SpawnThat.Caches;
using SpawnThat.Core;
using SpawnThat.Options.Modifiers;
using SpawnThat.Integrations.CLLC.Models;

namespace SpawnThat.Integrations.CLLC.Modifiers;

public class ModifierCllcInfusion : ISpawnModifier
{
    public CllcCreatureInfusion? Infusion { get; set; }

    public ModifierCllcInfusion()
    { }

    public ModifierCllcInfusion(string infusionName)
    {
        if (Enum.TryParse(infusionName, true, out CllcCreatureInfusion infusion))
        {
            Infusion = infusion;
        }
        else
        {
            Log.LogWarning($"CLLC infusion '{infusion}' formatted wrong. Ignoring spawn modifier.");
        }
    }

    public ModifierCllcInfusion(CllcCreatureInfusion? infusion)
    {
        Infusion = infusion;
    }

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        if (Infusion is null)
        {
            return;
        }

        var character = ComponentCache.Get<Character>(entity);

        if (character is null)
        {
            return;
        }

        Log.LogTrace($"Setting infusion '{Infusion}' for '{entity.name}'.");
        API.SetInfusionCreature(character, Infusion.Value.Convert());
    }
}
