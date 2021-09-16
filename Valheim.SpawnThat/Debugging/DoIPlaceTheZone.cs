using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Debugging
{
    [HarmonyPatch(typeof(ZoneSystem))]
    public static class DoIPlaceTheZone
    {
        [HarmonyPatch("PlaceZoneCtrl")]
        [HarmonyPrefix]
        public static void DetectPlacingZone(Vector2i zoneID, Vector3 zoneCenterPos, ZoneSystem.SpawnMode mode, List<GameObject> spawnedObjects)
        {
            Log.LogDebug("Placing zone: " + zoneID + ", Pos: " + zoneCenterPos);
        }
    }
}
