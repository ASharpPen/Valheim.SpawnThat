using System;
using CreatureLevelControl;
using UnityEngine;
using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawn.Modifiers;

namespace Valheim.SpawnThat.Integrations.CLLC.Modifiers;

public class ModifierCllcInfusion : ISpawnModifier
{
    public CreatureInfusion? Infusion { get; set; }

    public ModifierCllcInfusion()
    { }

    public ModifierCllcInfusion(string infusionName)
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

    public ModifierCllcInfusion(CreatureInfusion infusion)
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
        API.SetInfusionCreature(character, Infusion.Value);
    }
}
