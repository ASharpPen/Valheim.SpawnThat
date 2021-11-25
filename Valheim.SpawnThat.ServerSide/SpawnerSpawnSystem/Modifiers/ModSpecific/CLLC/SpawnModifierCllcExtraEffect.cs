using CreatureLevelControl;
using System;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Cache;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem;
using Valheim.SpawnThat.Spawns.Caches;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers.ModSpecific.CLLC;

internal class SpawnModifierExtraEffect : ISpawnModifier
{
    private CreatureExtraEffect? ExtraEffect { get; }

    public SpawnModifierExtraEffect(string extraEffectName)
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

    public SpawnModifierExtraEffect(CreatureExtraEffect extraEffect)
    {
        ExtraEffect = extraEffect;
    }

    public void Modify(Models.SpawnContext context, GameObject entity, ZDO entityZdo)
    {
        if (ExtraEffect is null)
        {
            return;
        }

        var character = ComponentCache.GetComponent<Character>(entity);

        if (character is null)
        {
            return;
        }

        Log.LogTrace($"Setting extra effect '{ExtraEffect}' for '{entity.name}'.");
        API.SetExtraEffectCreature(character, ExtraEffect.Value);
    }
}
