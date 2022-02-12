using System;
using UnityEngine;
using SpawnThat.Caches;
using SpawnThat.Core;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Options.Modifiers;

public class ModifierSetFaction : ISpawnModifier
{
    public Character.Faction? Faction { get; set; }

    public ModifierSetFaction()
    { }

    internal ModifierSetFaction(Character.Faction faction)
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
