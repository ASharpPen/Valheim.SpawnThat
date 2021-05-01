using CreatureLevelControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Spawns.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.ModSpecific.CLLC
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

        public void Modify(SpawnContext context)
        {
            if (!context.Spawn || context.Spawn is null)
            {
                return;
            }

            if (context.Config.TryGet(SpawnSystemConfigCLLC.ModName, out Config modConfig))
            {
                if (modConfig is SpawnSystemConfigCLLC config && config.SetInfusion.Value.Length > 0)
                {
                    var character = SpawnCache.GetCharacter(context.Spawn);

                    if (character is null)
                    {
                        return;
                    }

                    if (Enum.TryParse(config.SetInfusion.Value, true, out CreatureInfusion infusion))
                    {
                        Log.LogTrace($"Setting infusion {infusion} for {context.Spawn.name}.");
                        CreatureLevelControl.API.SetInfusionCreature(character, infusion);
                    }
                }
            }
        }
    }
}
