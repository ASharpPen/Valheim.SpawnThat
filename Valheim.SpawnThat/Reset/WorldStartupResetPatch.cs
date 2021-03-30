using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using Valheim.SpawnThat.ConfigurationCore;
using Valheim.SpawnThat.ConfigurationTypes;
using Valheim.SpawnThat.SpawnerSpawnSystem;

namespace Valheim.SpawnThat.Reset
{
    [HarmonyPatch(typeof(FejdStartup), "OnWorldStart")]
    public static class WorldStartupResetPatch
    {
        private static void Postfix()
        {
            //Check for singleplayer.
            if (ZNet.instance == null)
            {
                Log.LogDebug("Resetting configurations");

                StateResetter.Reset();

                ConfigurationManager.LoadAllConfigurations();
            }
        }
    }
}
