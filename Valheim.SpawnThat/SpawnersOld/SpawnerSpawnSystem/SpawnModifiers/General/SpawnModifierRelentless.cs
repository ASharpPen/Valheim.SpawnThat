
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General
{
    public class SpawnModifierRelentless : ISpawnModifier
    {
        public const string ZdoFeature = "spawnthat_relentless";

        private static SpawnModifierRelentless _instance;

        public static SpawnModifierRelentless Instance
        {
            get
            {
                return _instance ??= new SpawnModifierRelentless();
            }
        }

        public void Modify(SpawnContext context)
        {
            if(context.Config is null)
            {   
            	return;
            }

			if (context.Config.SetRelentless.Value)
			{
                Log.LogDebug("Setting relentless");

                var znetview = context.Spawn.GetComponent<ZNetView>();
                if (!znetview || znetview is null)
                {
#if DEBUG
                    Log.LogDebug("Unable to find znet component.");
#endif
                    return;
                }

                znetview.GetZDO().Set(ZdoFeature, true);
            }
        }
    }
}
