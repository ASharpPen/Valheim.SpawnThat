using HarmonyLib;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Options.Modifiers.Patches;

[HarmonyPatch]
internal static class SetFactionPatch
{
    [HarmonyPatch(typeof(Character), nameof(Character.Start))]
    [HarmonyPostfix]
    private static void AssignFaction(Character __instance)
    {
        var zdo = __instance.m_nview?.GetZDO();

        if (zdo is null)
        {
            return;
        }

        var faction = zdo.GetFaction();

        if (faction is not null)
        {
            __instance.m_faction = faction.Value;
        }
    }
}
