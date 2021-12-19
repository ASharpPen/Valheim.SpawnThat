using HarmonyLib;
using System;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Network;
using Valheim.SpawnThat.Startup;

namespace Valheim.SpawnThat.Locations;

[HarmonyPatch(typeof(ZNet))]
internal static class ZoneSystemMultiplayerPatch
{
    private static bool HaveReceivedLocations = false;

    static ZoneSystemMultiplayerPatch()
    {
        StateResetter.Subscribe(() =>
        {
            HaveReceivedLocations = false;
        });
    }

    [HarmonyPatch(nameof(ZNet.OnNewConnection))]
    [HarmonyPostfix]
    private static void TransferLocationData(ZNet __instance, ZNetPeer peer)
    {
        if (ZNet.instance.IsServer())
        {
            Log.LogDebug("Registering server RPC for sending location data on request from client.");
            peer.m_rpc.Register(nameof(RPC_RequestLocationsSpawnThat), new ZRpc.RpcMethod.Method(RPC_RequestLocationsSpawnThat));
        }
        else
        {
            Log.LogDebug("Registering client RPC for receiving location data from server.");
            peer.m_rpc.Register<ZPackage>(nameof(RPC_ReceiveLocationsSpawnThat), new Action<ZRpc, ZPackage>(RPC_ReceiveLocationsSpawnThat));

            Log.LogDebug("Requesting location data from server.");
            peer.m_rpc.Invoke(nameof(RPC_RequestLocationsSpawnThat));
        }
    }

    private static void RPC_RequestLocationsSpawnThat(ZRpc rpc)
    {
        try
        {
            if (!ZNet.instance.IsServer())
            {
                Log.LogWarning("Non-server instance received request for location data. Ignoring request.");
                return;
            }

            Log.LogInfo($"Received request for location data.");

            if (ZoneSystem.instance.m_locationInstances is null)
            {
                Log.LogWarning("Unable to get locations from zonesystem to send to client.");
                return;
            }

            DataTransferService.Service.AddToQueueAsync(() => new SimpleLocationPackage().Pack(), nameof(RPC_ReceiveLocationsSpawnThat), rpc);

            Log.LogTrace($"Sending location data.");
        }
        catch (Exception e)
        {
            Log.LogError("Unexpected error while attempting to create and send locations package from server to client.", e);
        }
    }

    private static void RPC_ReceiveLocationsSpawnThat(ZRpc rpc, ZPackage pkg)
    {
        Log.LogInfo("Received locations package.");
        try
        {
            if (HaveReceivedLocations)
            {
                Log.LogDebug("Already received locations previously. Skipping.");
                return;
            }

            CompressedPackage.Unpack(pkg);
            HaveReceivedLocations = true;
        }
        catch (Exception e)
        {
            Log.LogError("Error while attempting to read received locations package.", e);
        }
    }
}
