using SpawnThat.Core.Network;
using SpawnThat.Core;
using System;
using SpawnThat.Lifecycle;

namespace SpawnThat.Spawners.DestructibleSpawner.Sync;

internal static class DestructibleSpawnerSyncSetup
{
    public static void Configure()
    {
        SyncManager.RegisterSyncHandlers(
            nameof(RPC_SpawnThat_ReceiveDestructibleSpawnerConfigs),
            GeneratePackage,
            RPC_SpawnThat_ReceiveDestructibleSpawnerConfigs);
    }

    private static ZPackage GeneratePackage() => new DestructibleSpawnerConfigPackage().Pack();

    private static void RPC_SpawnThat_ReceiveDestructibleSpawnerConfigs(ZRpc rpc, ZPackage pkg)
    {
        Log.LogTrace("Received destructible spawner config package.");
        try
        {
            CompressedPackage.Unpack<DestructibleSpawnerConfigPackage>(pkg);
        }
        catch (Exception e)
        {
            Log.LogError("Error while attempting to read received config package.", e);
        }
    }
}
