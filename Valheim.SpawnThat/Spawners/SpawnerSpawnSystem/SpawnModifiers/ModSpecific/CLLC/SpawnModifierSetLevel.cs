using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Spawners.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.ModSpecific.CLLC
{
    internal class SpawnModifierSetLevel : ISpawnModifier
    {
        private static SpawnModifierSetLevel _instance;

        public static SpawnModifierSetLevel Instance
        {
            get
            {
                return _instance ??= new SpawnModifierSetLevel();
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
                if (modConfig is SpawnSystemConfigCLLC config && config.UseDefaultLevels.Value)
                {
                    var character = SpawnCache.GetCharacter(spawn);

                    if (character is null)
                    {
                        return;
                    }

                    var level = spawnerConfig.LevelMin.Value;

                    for (int i = 0;  i < spawnerConfig.LevelMax.Value - spawnerConfig.LevelMin.Value; ++i)
                    {
                        if (UnityEngine.Random.Range(0, 100) > 10)
                        {
                            break;
                        }

                        ++level;
                    }

                    character.SetLevel(level);
                }
            }
        }
    }
}
