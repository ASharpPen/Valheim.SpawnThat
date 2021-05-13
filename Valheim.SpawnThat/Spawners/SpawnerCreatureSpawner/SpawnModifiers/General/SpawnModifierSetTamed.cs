using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawns.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers.General
{
    public class SpawnModifierSetTamed : ISpawnModifier
    {
        private static SpawnModifierSetTamed _instance;

        public static SpawnModifierSetTamed Instance
        {
            get
            {
                return _instance ??= new SpawnModifierSetTamed();
            }
        }

        public void Modify(GameObject spawn, CreatureSpawnerConfig config)
        {
            if (spawn is null)
            {
                return;
            }

            var character = SpawnCache.GetCharacter(spawn);

            if (character is null)
            {
                return;
            }

            if (!config.SetTamed.Value)
            {
                return;
            }

#if DEBUG
            Log.LogDebug($"Setting tamed");
#endif
            character.SetTamed(true);
        }
    }
}
