using RagnarsRokare.MobAI;
using System;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.ModSpecific.MobAI
{
    internal class SpawnModifierSetAI : ISpawnModifier
    {
        private static SpawnModifierSetAI _instance;

        public static SpawnModifierSetAI Instance
        {
            get
            {
                return _instance ??= new SpawnModifierSetAI();
            }
        }

        public void Modify(GameObject spawn, CreatureSpawnerConfig spawnerConfig)
        {
            if (!spawn || spawn is null || spawnerConfig is null)
            {
#if DEBUG
                Log.LogDebug("SpawnModifierSetAI had null input.");
#endif
                return;
            }

            if (spawnerConfig.TryGet(CreatureSpawnerConfigMobAI.ModName, out Config modConfig))
            {
                if (modConfig is CreatureSpawnerConfigMobAI config && !string.IsNullOrWhiteSpace(config.SetAI.Value))
                {
                    var character = ComponentCache.Get<Character>(spawn);

                    if (character is null)
                    {
#if DEBUG
                        Log.LogDebug("SpawnModifierSetAI found no Character.");
#endif

                        return;
                    }

                    string aiConfig = "{}";

                    if (!string.IsNullOrWhiteSpace(config.AIConfigFile.DefaultValue))
                    {
                        aiConfig = config.AIConfigFile.DefaultValue;
                    }

                    try
                    {
                        MobManager.RegisterMob(character, Guid.NewGuid().ToString(), config.SetAI.Value, aiConfig);

                        Log.LogTrace($"Set AI {config.SetAI.Value} for spawn {spawnerConfig.SectionKey}");
                    }
                    catch (Exception e)
                    {
                        Log.LogError($"Failure while attempting to set AI {config.SetAI.Value} for spawn {spawnerConfig.SectionKey}", e);
                    }
                }
                else
                {
#if DEBUG
                    Log.LogDebug("SpawnModifierSetAI unable to find SetAI value or config.");
#endif
                }
            }
            else
            {
#if DEBUG
                Log.LogDebug("SpawnModifierSetAI unable to retrieve mod config.");
#endif
            }
        }
    }
}
