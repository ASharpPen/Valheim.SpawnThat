using HarmonyLib;

namespace SpawnThat.Lifecycle;

[HarmonyPatch(typeof(ZNet))]
internal static class ZNet_OnNewConnection_TriggerSync_Patch
{
    [HarmonyPatch(nameof(ZNet.OnNewConnection))]
    [HarmonyPostfix]
    private static void TriggerLifecycle(ZNetPeer peer)
    {
        SyncManager.SetupConfigSyncForPeer(peer);
    }
}
