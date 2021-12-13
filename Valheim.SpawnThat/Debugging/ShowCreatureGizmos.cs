#if DEBUG
using System;
using HarmonyLib;
using UnityEngine;
using Valheim.SpawnThat.Debugging.Gizmos;

namespace Valheim.SpawnThat.Debugging;

[HarmonyPatch]
internal static class ShowCreatureGizmos
{
    [HarmonyPatch(typeof(Character), nameof(Character.Awake))]
    [HarmonyPostfix]
    private static void SetLineGizmoOnCreature(BaseAI __instance)
    {
        LineGizmo.Create(__instance.gameObject.transform.position, Color.blue, TimeSpan.FromSeconds(30));
        LineGizmo.Create(__instance.gameObject, Color.yellow);
    }
}
#endif