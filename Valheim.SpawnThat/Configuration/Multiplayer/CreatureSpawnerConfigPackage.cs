using System;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Network;

namespace Valheim.SpawnThat.Configuration.Multiplayer;

[Serializable]
internal class CreatureSpawnerConfigPackage : CompressedPackage
{
    public CreatureSpawnerConfigurationFile CreatureSpawnerConfig;

    protected override void BeforePack()
    {
        CreatureSpawnerConfig = ConfigurationManager.CreatureSpawnerConfig;

        Log.LogDebug($"Packaged local spawner configurations: {CreatureSpawnerConfig?.Subsections?.Count ?? 0}");
    }

    protected override void AfterUnpack(object obj)
    {
        if (obj is CreatureSpawnerConfigPackage configPackage)
        {
            Log.LogDebug("Received and deserialized local spawner config package");

            ConfigurationManager.CreatureSpawnerConfig = configPackage.CreatureSpawnerConfig;

            Log.LogDebug($"Unpacked local spawner configurations: {ConfigurationManager.CreatureSpawnerConfig?.Subsections?.Count ?? 0}");

            Log.LogInfo("Successfully unpacked local spawner configs.");
        }
        else
        {
            Log.LogWarning("Received bad config package. Unable to load.");
        }
    }
}
