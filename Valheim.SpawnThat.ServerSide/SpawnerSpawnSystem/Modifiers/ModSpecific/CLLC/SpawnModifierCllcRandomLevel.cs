using System;
using CreatureLevelControl;
using UnityEngine;
using Valheim.SpawnThat.Core.Cache;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers.ModSpecific.CLLC;

public class SpawnModifierCllcRandomLevel : ISpawnModifier
{
    public void Modify(SpawnContext context, GameObject entity, ZDO entityZdo)
    {
        var character = ComponentCache.GetComponent<Character>(entity);

        if (character != null)
        {
            API.LevelRand(character);
        }
    }
}
