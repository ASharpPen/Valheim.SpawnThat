using CreatureLevelControl;
using System;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.ModSpecific.CLLC
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

        public void Modify(GameObject spawn, CreatureSpawnerConfig spawnerConfig)
        {
            if (spawn is null)
            {
                return;
            }

            if (spawnerConfig.TryGet(CreatureSpawnerConfigCLLC.ModName, out Config modConfig))
            {
                if (modConfig is CreatureSpawnerConfigCLLC config && config.SetExtraEffect.Value.Length > 0)
                {
                    var character = ComponentCache.Get<Character>(spawn);

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
