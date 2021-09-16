#if DEBUG
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SpawnThat.ServerSide.Debug
{

    [HarmonyPatch(typeof(ZNet))]
    internal static class WhoIsConnecting
    {
        [HarmonyPatch("OnNewConnection")]
        [HarmonyPostfix]
        private static void SyncConfigs(ZNet __instance, ZNetPeer peer)
        {
            Log.LogDebug($"Connecting -> Player: {peer.m_playerName}, ID: {peer.m_uid}");
        }
    }
}
#endif