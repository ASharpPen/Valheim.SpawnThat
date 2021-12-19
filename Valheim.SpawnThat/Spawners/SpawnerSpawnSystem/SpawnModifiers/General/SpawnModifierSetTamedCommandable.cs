using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Core;

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

            var character = ComponentCache.Get<Character>(context.Spawn);

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
            var tameable = ComponentCache.Get<Tameable>(context.Spawn);

            if (tameable is not null && tameable)
            {
                tameable.m_commandable = true;
            }

            ComponentCache.GetZdo(context.Spawn)?.Set(ZdoFeature, true);
        }
    }
}
