using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Utilities;

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
            if (spawn is null)
            {
                return;
            }

            var character = ComponentCache.Get<Character>(spawn);

            if (character is null)
            {
                return;
            }

            if (!config.SetTamedCommandable.Value)
            {
                return;
            }

#if DEBUG
            Log.LogDebug($"Setting tamed commandable");
#endif
            var tameable = ComponentCache.Get<Tameable>(spawn);

            if (tameable is not null && tameable)
            {
                tameable.m_commandable = true;
            }


            ComponentCache.GetZdo(spawn)?.Set(ZdoFeature, true);
        }
    }
}
