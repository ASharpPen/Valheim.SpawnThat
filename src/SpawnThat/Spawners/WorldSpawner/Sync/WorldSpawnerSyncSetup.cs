using System;
using SpawnThat.Core;
using SpawnThat.Core.Network;
using SpawnThat.Lifecycle;

namespace SpawnThat.Spawners.WorldSpawner.Sync;

internal static class WorldSpawnerSyncSetup
{
    public static void Configure()
    {
        SyncManager.RegisterSyncHandlers(
            nameof(RPC_SpawnThat_ReceiveWorldSpawnerConfigs),
            GeneratePackage,
            RPC_SpawnThat_ReceiveWorldSpawnerConfigs);
    }

    private static ZPackage GeneratePackage() => new WorldSpawnerConfigPackage().Pack();

    private static void RPC_SpawnThat_ReceiveWorldSpawnerConfigs(ZRpc rpc, ZPackage pkg)
    {
        Log.LogTrace("Received world spawner config package.");
        try
        {
            CompressedPackage.Unpack<WorldSpawnerConfigPackage>(pkg);
        }
        catch (Exception e)
        {
            Log.LogError("Error while attempting to read received config package.", e);
        }
    }
}
