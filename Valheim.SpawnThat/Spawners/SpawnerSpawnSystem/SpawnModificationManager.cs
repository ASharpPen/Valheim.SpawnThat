using System.Collections.Generic;
using UnityEngine;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General;
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

            SpawnModifiers.Add(SpawnModiferSetFaction.Instance);
            SpawnModifiers.Add(SpawnModifierRelentless.Instance);
            SpawnModifiers.Add(SpawnModifierDespawnOnConditionsInvalid.Instance);

            SpawnModifiers.Add(SpawnModifierLoaderCLLC.BossAffix);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.ExtraEffect);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.Infusion);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.SetLevel);
        }

        public void ApplyModifiers(SpawnSystem spawnSystem, GameObject spawn, SpawnSystem.SpawnData spawner)
        {
            var spawnData = SpawnSystemConfigCache.Get(spawner);

            if (spawnData?.Config is null)
            {
                Log.LogTrace($"Found no config for {spawn}.");
                return;
            }

            Log.LogTrace($"Applying modifiers to spawn {spawn.name}");

            foreach (var modifier in SpawnModifiers)
            {
                if (!spawn || spawn is null || spawner is null || spawnData?.Config is null)
                {
                    if (modifier is not null)
                    {
                        Log.LogWarning($"Skipping modifier {modifier.GetType().Name}");
                    }
                    continue;
                }

                modifier?.Modify(new SpawnContext
                {
                    SpawnSystem = spawnSystem,
                    Spawner = spawner,
                    Spawn = spawn,
                    Config = spawnData.Config
                });
            }
        }
    }
}
