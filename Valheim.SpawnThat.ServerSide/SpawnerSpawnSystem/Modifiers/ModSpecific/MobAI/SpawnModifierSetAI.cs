using RagnarsRokare.MobAI;
using System;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Spawns.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.ModSpecific.MobAI
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

        public void Modify(SpawnContext context)
        {
            if (context.Spawn is null)
            {
                return;
            }

            if (context.Config.TryGet(SpawnSystemConfigMobAI.ModName, out Config modConfig))
            {
                if (modConfig is SpawnSystemConfigMobAI config && !string.IsNullOrWhiteSpace(config.SetAI.Value))
                {
                    var character = SpawnCache.GetCharacter(context.Spawn);

                    if (character is null)
                    {
                        return;
                    }

                    string aiConfig = "{}";
                    
                    if(!string.IsNullOrWhiteSpace(config.AIConfigFile.DefaultValue))
                    {
                        aiConfig = config.AIConfigFile.DefaultValue;
                    }

                    try
                    {
                        MobManager.RegisterMob(character, Guid.NewGuid().ToString(), config.SetAI.Value, aiConfig);

                        Log.LogTrace($"Set AI {config.SetAI.Value} for spawn {context.Config.Name}");
                    }
                    catch(Exception e)
                    {
                        Log.LogError($"Failure while attempting to set AI {config.SetAI.Value} for spawn {context.Config.Name}", e);
                    }
                }
            }
        }
    }
}
