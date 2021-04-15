using HarmonyLib;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Reset
{
    [HarmonyPatch(typeof(FejdStartup))]
    public static class WorldStartupResetPatch
    {
        [HarmonyPatch("OnWorldStart")]
        [HarmonyPrefix]
        private static void ResetState()
        {
            Log.LogDebug("Resetting configurations");
            StateResetter.Reset();
            ConfigurationManager.LoadAllConfigurations();
        }

        [HarmonyPatch("JoinServer")]
        [HarmonyPrefix]
        private static void ResetStateMultplayer()
        {
            Log.LogDebug("Resetting configurations");
            StateResetter.Reset();
        }
    }
}
