using CreatureLevelControl;
using System;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Spawns.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.ModSpecific.CLLC
{
    internal class SpawnModifierExtraEffect : ISpawnModifier
    {
        private static SpawnModifierExtraEffect _instance;

        public static SpawnModifierExtraEffect Instance
        {
            get
            {
                return _instance ??= new SpawnModifierExtraEffect();
            }
        }

        public void Modify(SpawnContext context)
        {
            if (!context.Spawn || context.Spawn is null || context.Config is null)
            {
                return;
            }

            if (context.Config.TryGet(SpawnSystemConfigCLLC.ModName, out Config modConfig))
            {
                if (modConfig is SpawnSystemConfigCLLC config && config.SetExtraEffect.Value.Length > 0)
                {
                    var character = SpawnCache.GetCharacter(context.Spawn);

                    if (character is null)
                    {
                        return;
                    }

                    if (Enum.TryParse(config.SetExtraEffect.Value, true, out CreatureExtraEffect extraEffect))
                    {
                        Log.LogTrace($"Setting extra effect {extraEffect} for {context.Spawn.name}.");
                        CreatureLevelControl.API.SetExtraEffectCreature(character, extraEffect);
                    }
                }
            }
        }
    }
}
