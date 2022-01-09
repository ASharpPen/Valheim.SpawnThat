using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Core.Configuration;
using Valheim.SpawnThat.Core.Network;
using Valheim.SpawnThat.Spawners.LocalSpawner;
using Valheim.SpawnThat.Spawners.LocalSpawner.Configuration;

namespace Valheim.SpawnThat.Configuration.Multiplayer;

[Serializable]
internal class CreatureSpawnerConfigPackage : CompressedPackage
{
    public Dictionary<SpawnerNameIdentifier, LocalSpawnTemplate> TemplatesBySpawnerName;
    public Dictionary<LocationIdentifier, LocalSpawnTemplate> TemplatesByLocation;
    public Dictionary<RoomIdentifier, LocalSpawnTemplate> TemplatesByRoom;


    protected override void BeforePack()
    {
        TemplatesBySpawnerName = new(LocalSpawnTemplateManager.TemplatesBySpawnerName);
        TemplatesByLocation = new(LocalSpawnTemplateManager.TemplatesByLocation);
        TemplatesByRoom = new(LocalSpawnTemplateManager.TemplatesByRoom);

        int count = 
            (TemplatesBySpawnerName?.Count ?? 0) +
            (TemplatesByLocation?.Count ?? 0) + 
            (TemplatesByRoom?.Count ?? 0);
        Log.LogDebug($"Packaged local spawner configurations: {count}");
    }

    protected override void AfterUnpack(object obj)
    {
        if (obj is CreatureSpawnerConfigPackage configPackage)
        {
            Log.LogDebug("Received and deserialized local spawner config package");

            LocalSpawnTemplateManager.TemplatesBySpawnerName = configPackage.TemplatesBySpawnerName;
            LocalSpawnTemplateManager.TemplatesByLocation = configPackage.TemplatesByLocation;
            LocalSpawnTemplateManager.TemplatesByRoom = configPackage.TemplatesByRoom;

            int count =
                (configPackage.TemplatesBySpawnerName?.Count ?? 0) +
                (configPackage.TemplatesByLocation?.Count ?? 0) +
                (configPackage.TemplatesByRoom?.Count ?? 0);

            Log.LogDebug($"Unpacked local spawner configurations: {count}");

            Log.LogInfo("Successfully unpacked local spawner configs.");
        }
        else
        {
            Log.LogWarning("Received bad config package. Unable to load.");
        }
    }
}
