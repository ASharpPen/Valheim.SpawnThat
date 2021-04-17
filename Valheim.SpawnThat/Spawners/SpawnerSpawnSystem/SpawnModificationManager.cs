using System.Collections.Generic;
using UnityEngine;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.ModSpecific;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem
{
    public class SpawnModificationManager
    {
        private HashSet<ISpawnModifier> SpawnModifiers = new HashSet<ISpawnModifier>();

        private static SpawnModificationManager _instance;

        public static SpawnModificationManager Instance
        {
            get
            {
                return _instance ??= new SpawnModificationManager();
            }
        }

        SpawnModificationManager()
        {
            StateResetter.Subscribe(() =>
            {
                _instance = null;
            });

            SpawnModifiers.Add(SpawnModifierLoaderCLLC.BossAffix);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.ExtraEffect);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.Infusion);
        }

        public void ApplyModifiers(GameObject spawn, SpawnSystem.SpawnData spawner)
        {
            var spawnData = SpawnSystemCache.Get(spawner);

            if (spawnData?.Config is null)
            {
                Log.LogTrace($"Found no config for {spawn}.");
                return;
            }

            Log.LogTrace($"Applying modifiers to spawn {spawn.name}");

            foreach(var modifier in SpawnModifiers)
            {
                if(modifier is not null)
                {
                    modifier.Modify(spawn, spawner, spawnData.Config);
                }
            }
        }
    }
}
