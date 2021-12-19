using HarmonyLib;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawns
{
    [HarmonyPatch(typeof(Tameable))]
    public class TameableSetCommandablePatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Awake")]
        private static void SetCommandable(Tameable __instance)
        { 
            var zdo = ComponentCache.GetZdo(__instance);

            if(zdo.GetBool("spawnthat_tamed_commandable", false))
            {
                __instance.m_commandable = true;
            }
        }
    }
}
