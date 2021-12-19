using Valheim.SpawnThat.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General
{
    public class SpawnModifierDespawnOnAlert : ISpawnModifier
    {
        public const string ZdoFeature = "spawnthat_despawn_on_alert";

        private static SpawnModifierDespawnOnAlert _instance;

        public static SpawnModifierDespawnOnAlert Instance
        {
            get
            {
                return _instance ??= new SpawnModifierDespawnOnAlert();
            }
        }

        public void Modify(SpawnContext context)
        {
            if (context.Config is null)
            {
                return;
            }

            if (!context.Config.SetTryDespawnOnAlert.Value)
            {
                return;
            }

            var zdo = ComponentCache.GetZdo(context.Spawn);

            if (zdo is null)
            {
                return;
            }

            zdo.Set(ZdoFeature, true);
        }
    }
}
