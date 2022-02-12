using HarmonyLib;
using SpawnThat.Configuration;

namespace SpawnThat.Debugging;

[HarmonyPatch(typeof(ZoneSystem))]
internal static class LocationFileDumper
{
    /// <summary>
    /// Patch for scanning all the possible CreatureSpawner's of ZoneSystem.ZoneLocations and dumping as a file that can be directly loaded into configuration.
    /// </summary>
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    private static void ScanAndWriteLocations(ZoneSystem __instance)
    {
        if (__instance.m_locations is null)
        {
            return;
        }

        if (ConfigurationManager.GeneralConfig?.WriteCreatureSpawnersToFile?.Value == true)
        {
            CreatureSpawnerFileDumper.WriteToFile(__instance.m_locations);
        }
    }
}
