using System;
using Valheim.SpawnThat.Core.Network;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Lifecycle;
using Valheim.SpawnThat.Locations;

namespace Valheim.SpawnThat.World.Locations;

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
