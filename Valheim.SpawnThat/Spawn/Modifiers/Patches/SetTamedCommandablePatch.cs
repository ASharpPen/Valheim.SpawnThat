using HarmonyLib;
using Valheim.SpawnThat.Caches;

namespace Valheim.SpawnThat.Spawn.Modifiers.Patches;

[HarmonyPatch]
internal static class SetTamedCommandablePatch
{
    [HarmonyPatch(typeof(Tameable), nameof(Tameable.Awake))]
    [HarmonyPostfix]
    private static void SetCommandable(Tameable __instance)
    {
        var zdo = ComponentCache.GetZdo(__instance);

        if (zdo is null)
        {
            return;
        }

        if (zdo.GetBool("spawnthat_tamed_commandable", false))
        {
            __instance.m_commandable = true;
        }
    }
}
