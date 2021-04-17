using CreatureLevelControl;
using System;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Spawners.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.ModSpecific.CLLC
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

        public void Modify(GameObject spawn, CreatureSpawnerConfig spawnerConfig)
        {
            if (spawn is null)
            {
                return;
            }

            if (spawnerConfig.TryGet(CreatureSpawnerConfigCLLC.ModName, out Config modConfig))
            {
                if (modConfig is CreatureSpawnerConfigCLLC config && config.SetBossAffix.Value.Length > 0)
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
