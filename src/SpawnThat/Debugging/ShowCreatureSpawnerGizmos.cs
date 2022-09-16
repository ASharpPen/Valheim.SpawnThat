#if DEBUG && FALSE
using HarmonyLib;
using SpawnThat.Debugging.Gizmos;
using UnityEngine;

namespace SpawnThat.Debugging;

[HarmonyPatch]
internal static class ShowCreatureSpawnerGizmos
{
    [HarmonyPatch(typeof(CreatureSpawner), nameof(CreatureSpawner.Awake))]
    [HarmonyPostfix]
    private static void CreateGizmo(CreatureSpawner __instance)
    {
        SphereGizmo.Create(__instance.transform.position, 3, Color.yellow);
    }
}
#endif