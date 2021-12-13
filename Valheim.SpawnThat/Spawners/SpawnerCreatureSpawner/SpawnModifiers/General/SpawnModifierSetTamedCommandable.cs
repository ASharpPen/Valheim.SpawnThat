using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawns.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.General
{
    public class SpawnModifierSetTamedCommandable : ISpawnModifier
    {
        private const string ZdoFeature = "spawnthat_tamed_commandable";

        private static SpawnModifierSetTamedCommandable _instance;

        public static SpawnModifierSetTamedCommandable Instance
        {
            get
            {
                return _instance ??= new SpawnModifierSetTamedCommandable();
            }
        }

        public void Modify(GameObject spawn, CreatureSpawnerConfig config)
        {
            if (!spawn || spawn is null)
            {
                return;
            }

            var character = SpawnCache.GetCharacter(spawn);

            if (!character || character is null)
            {
                return;
            }

            if (config.SetTamedCommandable?.Value != true)
            {
                return;
            }

#if DEBUG
            Log.LogDebug($"Setting tamed commandable");
#endif
            var tameable = SpawnCache.GetTameable(spawn);

            if (tameable is not null && tameable)
            {
                tameable.m_commandable = true;

                SpawnCache.GetZDO(character).Set(ZdoFeature, true);
            }
        }
    }
}
