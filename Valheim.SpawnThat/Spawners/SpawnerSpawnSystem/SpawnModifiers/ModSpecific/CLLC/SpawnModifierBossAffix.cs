using CreatureLevelControl;
using System;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;

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

        public void Modify(GameObject spawn, SpawnSystem.SpawnData spawner, SpawnConfiguration spawnerConfig)
        {
            if (spawn is null)
            {
                return;
            }

            if (spawnerConfig.TryGet(SpawnSystemConfigCLLC.ModName, out Config modConfig))
            {
                if (modConfig is SpawnSystemConfigCLLC config && config.SetBossAffix.Value.Length > 0)
                {
                    var character = SpawnCache.GetCharacter(spawn);

                    if(character is null)
                    {
                        return;
                    }

                    if(!character.IsBoss())
                    {
                        return;
                    }

                    if (Enum.TryParse(config.SetBossAffix, true, out BossAffix bossAffix))
                    {
                        Log.LogTrace($"Setting boss affix {bossAffix} for {spawn.name}.");
                        CreatureLevelControl.API.SetAffixBoss(character, bossAffix);
                    }
                }
            }
        }
    }
}
