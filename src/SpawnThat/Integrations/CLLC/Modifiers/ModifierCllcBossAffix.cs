using System;
using CreatureLevelControl;
using UnityEngine;
using SpawnThat.Caches;
using SpawnThat.Core;
using SpawnThat.Options.Modifiers;

namespace SpawnThat.Integrations.CLLC.Modifiers;

public class ModifierCllcBossAffix : ISpawnModifier
{
    public BossAffix? Affix { get; set; }

    internal ModifierCllcBossAffix()
    { }

    public ModifierCllcBossAffix(string bossAffixName)
    {
        if (Enum.TryParse(bossAffixName, true, out BossAffix bossAffix))
        {
            Affix = bossAffix;
        }
        else
        {
            Log.LogWarning($"CLLC Boss Affix '{bossAffixName}' formatted wrong. Ignoring spawn modifier.");
        }
    }

    public ModifierCllcBossAffix(BossAffix bossAffix)
    {
        Affix = bossAffix;
    }

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        if (Affix is null)
        {
            return;
        }

        var character = ComponentCache.Get<Character>(entity);

        if (character is null)
        {
            return;
        }

        if (!character.IsBoss())
        {
            return;
        }

        Log.LogTrace($"Setting boss affix '{Affix}' for '{entity.name}'.");
        API.SetAffixBoss(character, Affix.Value);
    }
}

