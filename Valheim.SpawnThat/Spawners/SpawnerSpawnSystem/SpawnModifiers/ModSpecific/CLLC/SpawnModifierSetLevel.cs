using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Spawns.Caches;

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

        public void Modify(SpawnContext context)
        {
            if (context.Spawn is null)
            {
                return;
            }

            if (context.Config.TryGet(SpawnSystemConfigCLLC.ModName, out Config modConfig))
            {
                if (modConfig is SpawnSystemConfigCLLC config && config.UseDefaultLevels.Value)
                {
                    var character = SpawnCache.GetCharacter(context.Spawn);

                    if (character is null)
                    {
                        return;
                    }

                    var level = context.Config.LevelMin.Value;

                    for (int i = 0;  i < context.Config.LevelMax.Value - context.Config.LevelMin.Value; ++i)
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
