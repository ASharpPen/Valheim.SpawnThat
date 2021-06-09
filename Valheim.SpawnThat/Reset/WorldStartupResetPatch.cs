using HarmonyLib;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Managers;

namespace Valheim.SpawnThat.Reset
{
    [HarmonyPatch(typeof(FejdStartup))]
    public static class WorldStartupResetPatch
    {
        /// <summary>
        /// Singleplayer
        /// </summary>
        [HarmonyPatch("OnWorldStart")]
        [HarmonyPrefix]
        private static void ResetState()
        {
            Log.LogDebug("Resetting configurations");
            StateResetter.Reset();
            ConfigurationManager.LoadAllConfigurations();
            SpawnSystemConfigManager.Wait = false;
            CreatureSpawnerConfigManager.Wait = false;
        }

        /// <summary>
        /// Multiplayer
        /// </summary>
        [HarmonyPatch("JoinServer")]
        [HarmonyPrefix]
        private static void ResetStateMultiplayer()
        {
            Log.LogDebug("Resetting configurations");
            StateResetter.Reset();
        }

        /// <summary>
        /// Server
        /// </summary>
        [HarmonyPatch("ParseServerArguments")]
        [HarmonyPrefix]
        private static void ResetStateServer()
        {
            Log.LogDebug("Resetting configurations");
            StateResetter.Reset();
            ConfigurationManager.LoadAllConfigurations();
            SpawnSystemConfigManager.Wait = false;
            CreatureSpawnerConfigManager.Wait = false;
        }
    }
}
