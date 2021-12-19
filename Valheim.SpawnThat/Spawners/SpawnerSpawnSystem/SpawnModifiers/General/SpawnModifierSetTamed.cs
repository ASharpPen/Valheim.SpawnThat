using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General
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

        public void Modify(SpawnContext context)
        {
            if (context is null || !context.Spawn || context.Spawn is null || context.Config is null)
            {
                return;
            }

            var character = ComponentCache.Get<Character>(context.Spawn);

            if (character is null)
            {
                return;
            }

            if (!context.Config.SetTamed.Value)
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
