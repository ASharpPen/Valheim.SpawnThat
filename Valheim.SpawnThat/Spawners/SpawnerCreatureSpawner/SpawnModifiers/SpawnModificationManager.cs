using System;
using System.Collections.Generic;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.General;
using Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.ModSpecific;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers
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

            SpawnModifiers.Add(SpawnModifierSetFaction.Instance);
            SpawnModifiers.Add(SpawnModifierSetTamed.Instance);
            SpawnModifiers.Add(SpawnModifierSetTamedCommandable.Instance);

            SpawnModifiers.Add(SpawnModifierLoaderCLLC.BossAffix);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.ExtraEffect);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.Infusion);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.SetLevel);

            SpawnModifiers.Add(SpawnModifierLoaderMobAI.SetAI);

            SpawnModifiers.RemoveWhere(x => x is null);
        }

        public void ApplyModifiers(GameObject spawn, CreatureSpawnerConfig config)
        {
            if (config is null)
            {
                Log.LogTrace($"Found no config for {spawn}.");
                return;
            }

            Log.LogTrace($"Applying modifiers to spawn {spawn.name}");

            foreach (var modifier in SpawnModifiers)
            {
                if (modifier is not null)
                {
                    try
                    {
                        modifier.Modify(spawn, config);
                    }
                    catch(Exception e)
                    {
                        Log.LogWarning($"Error while attempting to apply modifier '{modifier?.GetType()?.Name}' to spawn '{spawn?.name}'", e);                        //Log.LogWarning($"Error while attempting to apply modifier '{modifier.GetType().Name}' to spawn {spawn.name}", e);
                    }
                }
                else
                {
#if DEBUG
                    Log.LogDebug($"CreatureSpawn modifier is null. Skipping." );
#endif
                }
            }
        }
    }
}
