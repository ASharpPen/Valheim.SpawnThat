using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General
{
    public class SpawnModifierRelentless : ISpawnModifier
    {
        public const string ZdoFeature = "spawnthat_relentless";

        private static SpawnModifierRelentless _instance;

        public static SpawnModifierRelentless Instance
        {
            get
            {
                return _instance ??= new SpawnModifierRelentless();
            }
        }

        public void Modify(GameObject spawn, SpawnSystem.SpawnData spawner, SpawnConfiguration config)
        {
            if (config.SetRelentless.Value)
            {
                Log.LogDebug("Setting relentless");

                var znetview = spawn.GetComponent<ZNetView>();
                if (!znetview || znetview is null)
                {
#if DEBUG
                    Log.LogDebug("Unable to find znet component.");
#endif
                    return;
                }

                spawn.GetComponent<ZNetView>().GetZDO().Set(ZdoFeature, true);
            }
        }
    }
}
