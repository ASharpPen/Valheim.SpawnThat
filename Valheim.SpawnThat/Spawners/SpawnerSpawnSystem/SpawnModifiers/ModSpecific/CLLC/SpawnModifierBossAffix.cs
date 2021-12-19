using CreatureLevelControl;
using System;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.ModSpecific.CLLC
{
    internal class SpawnModifierBossAffix : ISpawnModifier
    {
        private static SpawnModifierBossAffix _instance;

        public static SpawnModifierBossAffix Instance
        {
            get
            {
                return _instance ??= new SpawnModifierBossAffix();
            }
        }

        public void Modify(SpawnContext context)
        {
            if (context.Spawn is null)
            {
                return;
            }

            if (context.Config.TryGet(SpawnSystemConfigCLLC.ModName, out Config modConfig))
            {
                if (modConfig is SpawnSystemConfigCLLC config && config.SetBossAffix.Value.Length > 0)
                {
                    var character = ComponentCache.Get<Character>(context.Spawn);

                    if(character is null)
                    {
                        return;
                    }

                    if(!character.IsBoss())
                    {
                        return;
                    }

                    if (Enum.TryParse(config.SetBossAffix.Value, true, out BossAffix bossAffix))
                    {
                        Log.LogTrace($"Setting boss affix {bossAffix} for {context.Spawn.name}.");
                        CreatureLevelControl.API.SetAffixBoss(character, bossAffix);
                    }
                }
            }
        }
    }
}
