using System;
using UnityEngine;
using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.Spawn.Modifiers;

public class ModifierSetFaction : ISpawnModifier
{
    private Character.Faction? Faction { get; }

    public ModifierSetFaction(Character.Faction faction)
    {
        Faction = faction;
    }

    public ModifierSetFaction(string factionName)
    {
        if (Enum.TryParse(factionName.Trim(), out Character.Faction faction))
        {
            Faction = faction;
        }
        else
        {
            Log.LogWarning($"Failed to parse faction '{factionName}'");
        }
    }

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        if (entity is null)
{
return;
}

        var character = ComponentCache.Get<Character>(entity);

        if (character is null)
        {
            return;
        }

        if (Faction is null)
        {
            return;
        }

#if DEBUG
            Log.LogDebug($"Setting faction {Faction}");
#endif
        character.m_faction = Faction.Value;
        entityZdo?.SetFaction(Faction.Value);
    }
}
