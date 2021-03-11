using HarmonyLib;
using Valheim.SpawnThat.ConfigurationTypes;
using Valheim.SpawnThat.Debugging;

namespace Valheim.SpawnThat.Patches
{
    [HarmonyPatch(typeof(ZoneSystem), "Start")]
    public static class ZoneSystemPatch
    {
        /// <summary>
        /// Patch for scanning all the possible CreatureSpawner's of ZoneSystem.ZoneLocations and dumping as a file that can be directly loaded into configuration.
        /// </summary>
        private static void Postfix(ZoneSystem __instance)
        {
            if (ConfigurationManager.GeneralConfig.WriteCreatureSpawnersToFile.Value)
            {
                CreatureSpawnerFileDumper.WriteToFile(__instance.m_locations);
            }
        }
    }
}
