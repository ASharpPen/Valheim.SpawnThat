using CreatureLevelControl;
using System;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Spawners.Caches;

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

        public void Modify(GameObject spawn, SpawnSystem.SpawnData spawner, SpawnConfiguration spawnerConfig)
        {
            if (spawn is null)
            {
                return;
            }

            if (spawnerConfig.TryGet(SpawnSystemConfigCLLC.ModName, out Config modConfig))
            {
                if (modConfig is SpawnSystemConfigCLLC config && config.SetExtraEffect.Value.Length > 0)
                {
                    var character = SpawnCache.GetCharacter(spawn);

                    if (character is null)
                    {
                        return;
                    }

                    if (Enum.TryParse(config.SetExtraEffect.Value, true, out CreatureExtraEffect extraEffect))
                    {
                        Log.LogTrace($"Setting extra effect {extraEffect} for {spawn.name}.");
                        CreatureLevelControl.API.SetExtraEffectCreature(character, extraEffect);
                    }
                }
            }
        }
    }
}
