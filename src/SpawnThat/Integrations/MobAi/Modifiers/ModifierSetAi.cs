using System;
using RagnarsRokare.MobAI;
using UnityEngine;
using SpawnThat.Caches;
using SpawnThat.Core;
using SpawnThat.Options.Modifiers;

namespace SpawnThat.Integrations.MobAi.Modifiers;

public class ModifierSetAI : ISpawnModifier
{
    public string AiName { get; set; }
    public string Config { get; set; }

    public ModifierSetAI()
    { }

    public ModifierSetAI(string aiName, string config)
    {
        AiName = aiName;
        Config = config;
    }

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        if (string.IsNullOrWhiteSpace(AiName))
        {
            return;
        }

        var character = ComponentCache.Get<Character>(entity);

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