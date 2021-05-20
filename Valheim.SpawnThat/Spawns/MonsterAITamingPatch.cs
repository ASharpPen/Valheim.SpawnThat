using HarmonyLib;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawns.Caches;

namespace Valheim.SpawnThat.Spawns
{
    /*
    [HarmonyPatch(typeof(MonsterAI))]
    public static class MonsterAITamingPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        private static void SetTaming(MonsterAI __instance)
        {
            if (!__instance || __instance is null)
            {
#if DEBUG
                Log.LogWarning("Trying to patch into start of a null MonsterAI??");
#endif
            }

            var zdo = SpawnCache.GetZDO(__instance.gameObject);

            if(!zdo.GetBool("tamed", false))
            {
                return;
            }

            var tameableComponent = SpawnCache.GetTameable(__instance.gameObject);

            // Add tameable component if missing.
            if (!tameableComponent || tameableComponent is null)
            {
                var active = __instance.gameObject.activeSelf;

                try
                {
                    __instance.gameObject.SetActive(false);
                    tameableComponent = __instance.gameObject.AddComponent<Tameable>();
                }
                finally
                {
                    __instance.gameObject.SetActive(active);
                }
            }

            // Set commandable.
            //if (zdo.GetBool("spawnthat_tamed_commandable", false))
            //{
            //    tameableComponent.m_commandable = true;
            //}
        }
    }
    */
}
