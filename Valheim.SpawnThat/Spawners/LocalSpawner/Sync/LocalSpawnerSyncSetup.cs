using System;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Network;
using Valheim.SpawnThat.Lifecycle;

namespace Valheim.SpawnThat.Spawners.LocalSpawner.Sync;

internal static class LocalSpawnerSyncSetup
{
    public static void Configure()
    {
        SyncManager.RegisterSyncHandlers(
            nameof(RPC_SpawnThat_ReceiveLocalSpawnerConfigs),
            GeneratePackage,
            RPC_SpawnThat_ReceiveLocalSpawnerConfigs);
    }

    private static ZPackage GeneratePackage() => new LocalSpawnerConfigPackage().Pack();

    private static void RPC_SpawnThat_ReceiveLocalSpawnerConfigs(ZRpc rpc, ZPackage pkg)
    {
        Log.LogTrace("Received local spawner config package.");
        try
        {
            CompressedPackage.Unpack<LocalSpawnerConfigPackage>(pkg);
        }
        catch (Exception e)
        {
            Log.LogError("Error while attempting to read received config package.", e);
        }
    }
}
