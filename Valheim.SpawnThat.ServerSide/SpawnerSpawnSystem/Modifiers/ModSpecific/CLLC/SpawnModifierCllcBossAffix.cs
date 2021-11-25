using CreatureLevelControl;
using System;
using UnityEngine;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers.ModSpecific.CLLC;

public class SpawnModifierBossAffix : ISpawnModifier
{
    private BossAffix? Affix { get; }

    public SpawnModifierBossAffix(string bossAffixName)
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

    public SpawnModifierBossAffix(BossAffix bossAffix)
    {
        Affix = bossAffix;
    }

    public void Modify(Models.SpawnContext context, GameObject entity, ZDO entityZdo)
    {
        if (Affix is null)
        {
            return;
        }

        var character = ComponentCache.GetComponent<Character>(entity);

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
