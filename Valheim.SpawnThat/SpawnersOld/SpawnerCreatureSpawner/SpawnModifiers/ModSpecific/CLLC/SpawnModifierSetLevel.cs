using UnityEngine;
using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core.Configuration;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.ModSpecific.CLLC
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

        public void Modify(GameObject spawn, CreatureSpawnerConfig spawnerConfig)
        {
            if (spawn is null)
            {
                return;
            }

            if (spawnerConfig.TryGet(CreatureSpawnerConfigCLLC.ModName, out Config modConfig))
            {
                if (modConfig is CreatureSpawnerConfigCLLC config && config.UseDefaultLevels.Value)
                {
                    var character = ComponentCache.Get<Character>(spawn);

                    if (character is null)
                    {
                        return;
                    }

                    var level = spawnerConfig.LevelMin.Value;

                    for (int i = 0; i < spawnerConfig.LevelMax.Value - spawnerConfig.LevelMin.Value; ++i)
                    {
                        if (UnityEngine.Random.Range(0, 100) > spawnerConfig.LevelUpChance.Value)
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
