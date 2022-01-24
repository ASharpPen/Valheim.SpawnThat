using System;
using Valheim.SpawnThat.Core.Network;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Lifecycle;

namespace Valheim.SpawnThat.Configuration.Sync;

internal class GeneralConfigSyncSetup
{
    public static void Configure()
    {
        SyncManager.RegisterSyncHandlers(
            nameof(RPC_SpawnThat_ReceiveGeneralConfigs),
            GeneratePackage,
            RPC_SpawnThat_ReceiveGeneralConfigs);
    }

    private static ZPackage GeneratePackage() => new GeneralConfigPackage().Pack();

    private static void RPC_SpawnThat_ReceiveGeneralConfigs(ZRpc rpc, ZPackage pkg)
    {
        Log.LogTrace("Received general config package.");
        try
        {
            CompressedPackage.Unpack<GeneralConfigPackage>(pkg);
        }
        catch (Exception e)
        {
            Log.LogError("Error while attempting to read received config package.", e);
        }
    }
}
