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

            SpawnModifiers.Add(SpawnModiferSetFaction.Instance);

            SpawnModifiers.Add(SpawnModifierLoaderCLLC.BossAffix);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.ExtraEffect);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.Infusion);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.SetLevel);
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
                    modifier.Modify(spawn, config);
                }
            }
        }
    }
}
