using RagnarsRokare.MobAI;
using System;
using UnityEngine;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Modifiers.ModSpecific.MobAI;

public class SpawnModifierSetAI : ISpawnModifier
{
    public string AiName { get; }
    public string Config { get; }

    public SpawnModifierSetAI(string aiName, string config)
    {
        AiName = aiName;
        Config = config;
    }

    public void Modify(Models.SpawnContext context, GameObject entity, ZDO entityZdo)
    {
        var character = ComponentCache.GetComponent<Character>(entity);

        if (character is null)
        {
            return;
        }

        string aiConfig = "{}";

        if (!string.IsNullOrWhiteSpace(Config))
        {
            aiConfig = Config;
        }

        try
        {
            MobManager.RegisterMob(character, Guid.NewGuid().ToString(), AiName, aiConfig);

            Log.LogTrace($"Set AI '{AiName}' for spawn '{entity.name}'.");
        }
        catch (Exception e)
        {
            Log.LogError($"Failure while attempting to set AI '{AiName}' for spawn '{entity.name}'", e);
        }
    }
}
