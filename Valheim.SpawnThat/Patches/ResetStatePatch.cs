using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using Valheim.SpawnThat.ConfigurationCore;
using Valheim.SpawnThat.ConfigurationTypes;
using Valheim.SpawnThat.SpawnerSpawnSystem;

namespace Valheim.SpawnThat.Patches
{
    [HarmonyPatch(typeof(FejdStartup), "OnWorldStart")]
    public static class ResetSpawnSystemPatch
    {
        private static void Postfix()
        {
            //Check for singleplayer.
            if (ZNet.instance == null)
            {
                Log.LogDebug("Resetting configurations");

                CreatureSpawnerPatch.AppliedConfigs = new HashSet<Vector3>();
                CreatureSpawnerPatch.ConfigLookupTable = null;

                SpawnSystemPatch.AppliedConfigs = new HashSet<Vector3>();
                SpawnSystemPatch.FirstApplication = true;
                SpawnSystemPatch.Configs = null;
                SpawnSystemPatch.SimpleConfigTable = null;

                ConfigurationManager.LoadAllConfigurations();
            }
        }
    }
}
