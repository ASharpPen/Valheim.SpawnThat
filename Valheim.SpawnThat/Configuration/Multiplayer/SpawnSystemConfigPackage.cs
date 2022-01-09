using System;
using System.Linq;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Network;
using Valheim.SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;

namespace Valheim.SpawnThat.Configuration.Multiplayer;

[Serializable]
internal class SpawnSystemConfigPackage : CompressedPackage
{
    public SpawnSystemConfigurationFile SpawnSystemConfig;
    public SimpleConfigurationFile SimpleConfig = SpawnSystemConfigurationManager.SimpleConfig;

    protected override void BeforePack()
    {
        SpawnSystemConfig = SpawnSystemConfigurationManager.SpawnSystemConfig;
        SimpleConfig = SpawnSystemConfigurationManager.SimpleConfig;

        Log.LogDebug($"Packaged world spawner configurations: {SpawnSystemConfig?.Subsections?.Count ?? 0}");
        Log.LogDebug($"Packaged simple world spawner configurations: {SimpleConfig?.Subsections?.Count ?? 0}");
    }

    protected override void AfterUnpack(object obj)
    {
        if (obj is SpawnSystemConfigPackage configPackage)
        {
            Log.LogDebug("Received and deserialized world spawner config package");

            SpawnSystemConfigurationManager.SpawnSystemConfig = configPackage.SpawnSystemConfig;
            SpawnSystemConfigurationManager.SimpleConfig = configPackage.SimpleConfig;

            Log.LogDebug($"Unpacked world spawner configurations: {SpawnSystemConfigurationManager.SpawnSystemConfig?.Subsections?.Count ?? 0}");
            Log.LogDebug($"Unpacked simple world spawner configurations: {SpawnSystemConfigurationManager.SimpleConfig?.Subsections?.Count ?? 0}");

            Log.LogInfo("Successfully unpacked world spawner configs.");
        }
        else
        {
            Log.LogWarning("Received bad config package. Unable to load.");
        }
    }
}
