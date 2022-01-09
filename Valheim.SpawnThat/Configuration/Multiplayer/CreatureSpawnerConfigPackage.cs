using System;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Network;
using Valheim.SpawnThat.Spawners.LocalSpawner.Configuration.BepInEx;

namespace Valheim.SpawnThat.Configuration.Multiplayer;

[Serializable]
internal class CreatureSpawnerConfigPackage : CompressedPackage
{
    public CreatureSpawnerConfigurationFile CreatureSpawnerConfig;

    protected override void BeforePack()
    {
        CreatureSpawnerConfig = CreatureSpawnerConfigurationManager.CreatureSpawnerConfig;

        Log.LogDebug($"Packaged local spawner configurations: {CreatureSpawnerConfig?.Subsections?.Count ?? 0}");
    }

    protected override void AfterUnpack(object obj)
    {
        if (obj is CreatureSpawnerConfigPackage configPackage)
        {
            Log.LogDebug("Received and deserialized local spawner config package");

            CreatureSpawnerConfigurationManager.CreatureSpawnerConfig = configPackage.CreatureSpawnerConfig;

            Log.LogDebug($"Unpacked local spawner configurations: {CreatureSpawnerConfigurationManager.CreatureSpawnerConfig?.Subsections?.Count ?? 0}");

            Log.LogInfo("Successfully unpacked local spawner configs.");
        }
        else
        {
            Log.LogWarning("Received bad config package. Unable to load.");
        }
    }
}
