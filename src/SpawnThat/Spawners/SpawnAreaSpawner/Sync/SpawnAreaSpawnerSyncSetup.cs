using SpawnThat.Core.Network;
using SpawnThat.Core;
using System;
using SpawnThat.Lifecycle;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Sync;

internal static class SpawnAreaSpawnerSyncSetup
{
    public static void Configure()
    {
        SyncManager.RegisterSyncHandlers(
            nameof(RPC_SpawnThat_ReceiveSpawnAreaSpawnerConfigs),
            GeneratePackage,
            RPC_SpawnThat_ReceiveSpawnAreaSpawnerConfigs);
    }

    private static ZPackage GeneratePackage() => new SpawnAreaSpawnerConfigPackage().Pack();

    private static void RPC_SpawnThat_ReceiveSpawnAreaSpawnerConfigs(ZRpc rpc, ZPackage pkg)
    {
        Log.LogTrace("Received SpawnArea spawner config package.");
        try
        {
            CompressedPackage.Unpack<SpawnAreaSpawnerConfigPackage>(pkg);
        }
        catch (Exception e)
        {
            Log.LogError("Error while attempting to read received config package.", e);
        }
    }
}
