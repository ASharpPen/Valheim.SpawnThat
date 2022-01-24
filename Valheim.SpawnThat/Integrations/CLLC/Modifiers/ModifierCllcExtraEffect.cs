using System;
using CreatureLevelControl;
using UnityEngine;
using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawn.Modifiers;

namespace Valheim.SpawnThat.Integrations.CLLC.Modifiers;

internal class ModifierCllcExtraEffect : ISpawnModifier
{
    public CreatureExtraEffect? ExtraEffect { get; set; }

    public ModifierCllcExtraEffect()
    { }

    public ModifierCllcExtraEffect(string extraEffectName)
    {
        if (Enum.TryParse(extraEffectName, true, out CreatureExtraEffect extraEffect))
        {
            ExtraEffect = extraEffect;
        }
        else
        {
            Log.LogWarning($"CLLC Creature Effect Affix '{extraEffectName}' formatted wrong. Ignoring spawn modifier.");
        }
    }

    public ModifierCllcExtraEffect(CreatureExtraEffect extraEffect)
    {
        ExtraEffect = extraEffect;
    }

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        if (ExtraEffect is null)
        {
            return;
        }

        var character = ComponentCache.Get<Character>(entity);

        if (character is null)
        {
            return;
        }

        Log.LogTrace($"Setting extra effect '{ExtraEffect}' for '{entity.name}'.");
        API.SetExtraEffectCreature(character, ExtraEffect.Value);
    }
}
