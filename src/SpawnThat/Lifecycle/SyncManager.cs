using System;
using System.Collections.Generic;
using SpawnThat.Core;
using SpawnThat.Core.Network;

namespace SpawnThat.Lifecycle;

public static class SyncManager
{
    private class HandlerPair
    {
        public string UnpackerRpcName { get; }
        public Func<ZPackage> Packer { get; }
        public Action<ZRpc, ZPackage> Unpacker { get; }

        public HandlerPair(string unpackerRpcName, Func<ZPackage> packer, Action<ZRpc, ZPackage> unpacker)
        {
            UnpackerRpcName = unpackerRpcName;
            Packer = packer;
            Unpacker = unpacker;
        }
    }

    private static Dictionary<string, HandlerPair> PackageHandlers { get; } = new();

    public static void RegisterSyncHandlers(string unpackerRpc, Func<ZPackage> packer, Action<ZRpc, ZPackage> unpacker)
    {
        PackageHandlers[unpackerRpc] = new(unpackerRpc, packer, unpacker);
    }

    internal static void SetupConfigSyncForPeer(ZNetPeer peer)
    {
        try
        {
            if (ZNet.instance.IsServer())
            {
                Log.LogDebug("Registering server RPC for sending configs on request from client.");
                peer.m_rpc.Register(nameof(RPC_RequestConfigsSpawnThat), new ZRpc.RpcMethod.Method(RPC_RequestConfigsSpawnThat));
            }
            else
            {
                Log.LogDebug("Registering client RPCs for receiving config packages from server.");
                foreach (var handler in PackageHandlers.Values)
                {
                    peer.m_rpc.Register(handler.UnpackerRpcName, handler.Unpacker);
                }

                Log.LogDebug("Requesting configs from server.");
                peer.m_rpc.Invoke(nameof(RPC_RequestConfigsSpawnThat));
            }
        }
        catch (Exception e)
        {
            Log.LogError("Error during setup of config sync", e);
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

            Log.LogDebug("Received request for configs.");

            foreach(var handler in PackageHandlers.Values)
            {
                DataTransferService.Service.AddToQueueAsync(handler.Packer, handler.UnpackerRpcName, rpc);
            }

            Log.LogTrace("Sending config packages.");
        }
        catch (Exception e)
        {
            Log.LogError("Unexpected error while attempting to create and send config packages from server to client.", e);
        }
    }
}
