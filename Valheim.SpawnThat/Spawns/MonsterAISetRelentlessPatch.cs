using HarmonyLib;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General;

namespace Valheim.SpawnThat.Spawns
{
    [HarmonyPatch(typeof(MonsterAI))]
    public static class MonsterAISetRelentlessPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void StartAlerted(MonsterAI __instance, ZNetView ___m_nview, ref bool ___m_alerted)
        {
            var zdo = ___m_nview.GetZDO();
            var forceAlert = zdo.GetBool(SpawnModifierRelentless.ZdoFeature, false);

            if (forceAlert)
            {
                Log.LogTrace($"Initializing forced alertness for {__instance.gameObject.name}");
                __instance.Alert();
            }
        }
    }

    [HarmonyPatch(typeof(BaseAI))]
    public static class BaseAISetRelentlessPatch
    { 
        [HarmonyPatch("SetAlerted")]
        [HarmonyPrefix]
        private static void ForceAlert(BaseAI __instance, ref bool alert)
        {
            var znview = __instance.gameObject.GetComponent<ZNetView>();

            if (!znview || znview is null)
            {
                return;
            }

            var forceAlert = znview.GetZDO().GetBool(SpawnModifierRelentless.ZdoFeature, false);

            if (!alert && forceAlert)
            {
#if DEBUG
                Log.LogTrace($"Forcing awakenes for {__instance.gameObject.name}");
#endif
                alert = true;
            }
        }
    }
}
