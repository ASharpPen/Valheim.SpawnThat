using HarmonyLib;
using Valheim.SpawnThat.ConfigurationCore;
using Valheim.SpawnThat.ConfigurationTypes;

namespace Valheim.SpawnThat.Reset
{
    [HarmonyPatch(typeof(FejdStartup))]
    public static class WorldStartupResetPatch
    {
        /*
        [HarmonyPatch("LoadMainScene")]
        [HarmonyPrefix]
        private static void ResetState()
        {
            Log.LogDebug("Resetting configurations");
            StateResetter.Reset();

            //Check for singleplayer.
            if (ZNet.instance == null )
            {
                ConfigurationManager.LoadAllConfigurations();
            }
        }
        */

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
