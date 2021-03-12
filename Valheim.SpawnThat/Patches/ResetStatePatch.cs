using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.ConfigurationCore;
using Valheim.SpawnThat.ConfigurationTypes;

namespace Valheim.SpawnThat.Patches
{

    [HarmonyPatch(typeof(FejdStartup), "OnWorldStart")]
    public static class ResetSpawnSystemPatch
    {
        private static void Postfix()
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
