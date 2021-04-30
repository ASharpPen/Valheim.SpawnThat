
using Valheim.SpawnThat.Spawns.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General
{
    public class SpawnModifierDespawnOnConditionsInvalid : ISpawnModifier
    {
        public const string ZdoFeature = "spawnthat_despawn_on_invalid";

        private static SpawnModifierDespawnOnConditionsInvalid _instance;

        public static SpawnModifierDespawnOnConditionsInvalid Instance
        {
            get
            {
                return _instance ??= new SpawnModifierDespawnOnConditionsInvalid();
            }
        }

        public void Modify(SpawnContext context)
        {
            if(context.Config is null)
            {
                return;
            }

            if (!context.Config.SetTryDespawnOnConditionsInvalid.Value)
            {
                return;
            }

            var zdo = SpawnCache.GetZDO(context.Spawn);

            if(zdo is null)
            {
                return;
            }

            zdo.Set(ZdoFeature, true);
            zdo.Set(Conditions.ConditionDaytime.ZdoConditionDay, context.Config.SpawnDuringDay.Value);
            zdo.Set(Conditions.ConditionDaytime.ZdoConditionNight, context.Config.SpawnDuringNight.Value);
            zdo.Set(Conditions.ConditionEnvironments.ZdoCondition, context.Config.RequiredEnvironments.Value);
        }
    }
}
