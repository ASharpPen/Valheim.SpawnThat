using System;
using SpawnThat.Core.Network;
using SpawnThat.Core;
using SpawnThat.Lifecycle;
using SpawnThat.Locations;

namespace SpawnThat.World.Locations;

internal static class ZoneSystemSyncSetup
{
    public static void Configure()
    {
        SyncManager.RegisterSyncHandlers(
            nameof(RPC_ReceiveLocationsSpawnThat),
            GeneratePackage,
            RPC_ReceiveLocationsSpawnThat);
    }

    private static ZPackage GeneratePackage() => new SimpleLocationPackage().Pack();

    private static void RPC_ReceiveLocationsSpawnThat(ZRpc rpc, ZPackage pkg)
    {
        Log.LogInfo("Received locations package.");
        try
        {
            CompressedPackage.Unpack<SimpleLocationPackage>(pkg);
        }
        catch (Exception e)
        {
            Log.LogError("Error while attempting to read received locations package.", e);
        }
    }
}
