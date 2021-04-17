using CreatureLevelControl;
using System;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Spawners.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.ModSpecific.CLLC
{
    internal class SpawnModifierInfusion : ISpawnModifier
    {
        private static SpawnModifierInfusion _instance;

        public static SpawnModifierInfusion Instance
        {
            get
            {
                return _instance ??= new SpawnModifierInfusion();
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
                if (modConfig is CreatureSpawnerConfigCLLC config && config.SetInfusion.Value.Length > 0)
                {
                    var character = SpawnCache.GetCharacter(spawn);

                    if (character is null)
                    {
                        return;
                    }

                    if (Enum.TryParse(config.SetInfusion, true, out CreatureInfusion infusion))
                    {
                        Log.LogTrace($"Setting infusion {infusion} for {spawn.name}.");
                        CreatureLevelControl.API.SetInfusionCreature(character, infusion);
                    }
                }
            }
        }
    }
}
