using HarmonyLib;
using System.Collections.Generic;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Debugging;

namespace SpawnThat.Spawners.SpawnerCreatureSpawner.Patches;

[HarmonyPatch(typeof(DungeonDB))]
public static class DungeonCreatureSpawnerFileDumper
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    private static void ScanRooms(DungeonDB __instance, List<DungeonDB.RoomData> ___m_rooms)
    {
#if DEBUG
        Log.LogDebug("Starting CreatureSpawner scan.");
#endif

        if (___m_rooms is null)
        {
            return;
        }

        if (ConfigurationManager.GeneralConfig?.WriteCreatureSpawnersToFile?.Value == true)
        {
#if DEBUG
            Log.LogDebug($"Starting CreatureSpawner file writing for {ZoneSystem.instance.m_locations.Count} location and {___m_rooms.Count} rooms.");
#endif
            RoomCreatureSpawnerFileDumper.WriteToFile(___m_rooms);
        }
    }
}
