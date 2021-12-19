using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General
{
    public class SpawnModifierSetTemplateId : ISpawnModifier
    {
        public const string ZdoFeature = "spawn_template_id";

        private static SpawnModifierSetTemplateId _instance;

        public static SpawnModifierSetTemplateId Instance
        {
            get
            {
                return _instance ??= new SpawnModifierSetTemplateId();
            }
        }

        public void Modify(SpawnContext context)
        {
            if (context.Config is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(context.Config.TemplateId.Value))
            {
                return;
            }

            var zdo = ComponentCache.GetZdo(context.Spawn);

            if (zdo is null)
            {
                return;
            }

            Log.LogTrace($"Setting template id {context.Config.TemplateId.Value}");
            zdo.Set(ZdoFeature, context.Config.TemplateId.Value);
        }
    }
}
