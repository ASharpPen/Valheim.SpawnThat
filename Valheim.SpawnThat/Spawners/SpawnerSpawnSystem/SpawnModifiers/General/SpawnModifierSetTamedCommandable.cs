using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawns.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General
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

        public void Modify(SpawnContext context)
        {
            if (context.Spawn is null)
            {
                return;
            }

            var character = SpawnCache.GetCharacter(context.Spawn);

            if (character is null)
            {
                return;
            }

            if (!context.Config.SetTamedCommandable.Value)
            {
                return;
            }

#if DEBUG
            Log.LogDebug($"Setting tamed commandable");
#endif
            var tameable = SpawnCache.GetTameable(context.Spawn);

            if (tameable is not null && tameable)
            {
                tameable.m_commandable = true;
            }

            SpawnCache.GetZDO(character).Set(ZdoFeature, true);
        }
    }
}
