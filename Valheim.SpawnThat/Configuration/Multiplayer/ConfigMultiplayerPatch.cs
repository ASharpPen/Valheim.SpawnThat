using HarmonyLib;
using System;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Network;

namespace Valheim.SpawnThat.Configuration.Multiplayer;

[HarmonyPatch(typeof(ZNet))]
internal static class ConfigMultiplayerPatch
{
    [HarmonyPatch(nameof(ZNet.OnNewConnection))]
    [HarmonyPostfix]
    private static void SyncConfigs(ZNet __instance, ZNetPeer peer)
    {
        if (ZNet.instance.IsServer())
        {
            Log.LogDebug("Registering server RPC for sending configs on request from client.");
            peer.m_rpc.Register(nameof(RPC_RequestConfigsSpawnThat), new ZRpc.RpcMethod.Method(RPC_RequestConfigsSpawnThat));
        }
        else
        {
            Log.LogDebug("Registering client RPC for receiving configs from server.");
            peer.m_rpc.Register<ZPackage>(nameof(RPC_ReceiveConfigsSpawnThat), new Action<ZRpc, ZPackage>(RPC_ReceiveConfigsSpawnThat));

            Log.LogDebug("Requesting configs from server.");
            peer.m_rpc.Invoke(nameof(RPC_RequestConfigsSpawnThat));
        }
    }

    private static void RPC_RequestConfigsSpawnThat(ZRpc rpc)
    {
        try
        {
            if (!ZNet.instance.IsServer())
            {
                Log.LogWarning("Non-server instance received request for configs. Ignoring request.");
            }

            Log.LogInfo("Received request for configs.");

            DataTransferService.Service.AddToQueueAsync(() => new GeneralConfigPackage().Pack(), nameof(RPC_ReceiveConfigsSpawnThat), rpc);
            DataTransferService.Service.AddToQueueAsync(() => new CreatureSpawnerConfigPackage().Pack(), nameof(RPC_ReceiveConfigsSpawnThat), rpc);
            DataTransferService.Service.AddToQueueAsync(() => new SpawnSystemConfigPackage().Pack(), nameof(RPC_ReceiveConfigsSpawnThat), rpc);

            Log.LogTrace("Sending config packages.");
        }
        catch (Exception e)
        {
            Log.LogError("Unexpected error while attempting to create and send config packages from server to client.", e);
        }
    }

    private static void RPC_ReceiveConfigsSpawnThat(ZRpc rpc, ZPackage pkg)
    {
        Log.LogTrace("Received package.");
        try
        {
            CompressedPackage.Unpack(pkg);
        }
        catch (Exception e)
        {
            Log.LogError("Error while attempting to read received config package.", e);
        }
    }
}
