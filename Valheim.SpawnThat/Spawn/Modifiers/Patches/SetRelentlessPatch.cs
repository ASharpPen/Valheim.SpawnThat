using HarmonyLib;
using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawn.Modifiers.Patches;

[HarmonyPatch]
internal static class SetRelentlessPatch
{
    [HarmonyPatch(typeof(MonsterAI), nameof(MonsterAI.Start))]
    [HarmonyPostfix]
    private static void StartAlerted(MonsterAI __instance)
    {
        var zdo = ComponentCache.GetZdo(__instance);

        if (zdo is null)
        {
            return;
        }

        var forceAlert = zdo.GetBool(ModifierSetRelentless.ZdoFeatureHash, false);

        if (forceAlert)
        {
            Log.LogTrace($"Initializing forced alertness for {__instance.gameObject.name}");
            __instance.Alert();
        }
    }

    [HarmonyPatch(typeof(BaseAI), nameof(BaseAI.SetAlerted))]
    [HarmonyPrefix]
    private static void ForceAlert(BaseAI __instance, ref bool alert)
    {
        var zdo = ComponentCache.GetZdo(__instance);

        if (zdo is null)
        {
            return;
        }

        var forceAlert = zdo.GetBool(ModifierSetRelentless.ZdoFeatureHash, false);

        if (!alert && forceAlert)
        {
            __instance.Alert();
            alert = true;
        }
    }
}
