using HarmonyLib;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.World.Zone.Patches;

[HarmonyPatch]
internal sealed class ZoneManager_Heightmap_Patch
{
    [HarmonyPatch(typeof(Heightmap), nameof(Heightmap.Regenerate))]
    [HarmonyPostfix]
    private static void Record(Heightmap __instance)
    {
        // Sometimes distant heightmaps are generated, we need to skip those.
        // Not sure whats going on here, but they get generated in inconsistent positions
        // that do not fit on the zone grid.
        if (!__instance.m_isDistantLod)
        {
            return;
        }

        var zoneId = __instance.gameObject.transform.position.GetZoneId();

        ZoneManager.HeightmapsLoaded[zoneId] = new ZoneHeightmap(__instance);
#if DEBUG && FALSE
            LineGizmo.Create(__instance.transform.position, Color.green);
#endif
    }

    [HarmonyPatch(typeof(Heightmap), nameof(Heightmap.OnDestroy))]
    [HarmonyPostfix]
    private static void RemoveRecord(Heightmap __instance)
    {
        var zoneId = __instance.gameObject.transform.position.GetZoneId();
        ZoneManager.HeightmapsLoaded.Remove(zoneId);
    }
}
