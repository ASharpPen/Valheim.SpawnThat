using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SpawnThat.Locations
{
    [HarmonyPatch(typeof(ZoneSystem))]
    public static class ZoneSystemStartPatch
    {
        [HarmonyPatch(nameof(ZoneSystem.Load))]
        [HarmonyPostfix]
        private static void LoadLocations(Dictionary<Vector2i, ZoneSystem.LocationInstance> ___m_locationInstances)
        {
            if (___m_locationInstances is not null && ___m_locationInstances.Count > 0)
            {
                LocationHelper.SetLocationInstances(___m_locationInstances);
            }
        }
    }
}
