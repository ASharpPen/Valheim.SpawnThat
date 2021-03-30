using HarmonyLib;
using System.Collections.Generic;
using Valheim.SpawnThat.ConfigurationCore;
using Valheim.SpawnThat.ConfigurationTypes;
using Valheim.SpawnThat.Debugging;

namespace Valheim.SpawnThat.Reset
{
    [HarmonyPatch(typeof(ZoneSystem))]
    public static class ZoneSystemScanPatch
    {
        /// <summary>
        /// Patch for scanning all the possible CreatureSpawner's of ZoneSystem.ZoneLocations and dumping as a file that can be directly loaded into configuration.
        /// </summary>
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void ScanLocations(ZoneSystem __instance)
        {
            if (ConfigurationManager.GeneralConfig.WriteCreatureSpawnersToFile.Value)
            {
                //CreatureSpawnerFileDumper.WriteToFile(__instance.m_locations);
            }
        }
    }

    [HarmonyPatch(typeof(DungeonDB))]
    public static class DungeonRoomsScanPatch
    { 
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void ScanRooms(DungeonDB __instance, List<DungeonDB.RoomData> ___m_rooms)
        {
#if DEBUG
            Log.LogDebug("Starting CreatureSpawner scan.");
#endif

            if(ConfigurationManager.GeneralConfig.WriteCreatureSpawnersToFile.Value)
            {
#if DEBUG
                Log.LogDebug($"Starting CreatureSpawner file writing for {ZoneSystem.instance.m_locations.Count} location and {___m_rooms.Count} rooms.");
#endif

                CreatureSpawnerFileDumper.WriteToFile(ZoneSystem.instance.m_locations);

                RoomCreatureSpawnerFileDumper.WriteToFile(___m_rooms);
            }
        }
    }
}
