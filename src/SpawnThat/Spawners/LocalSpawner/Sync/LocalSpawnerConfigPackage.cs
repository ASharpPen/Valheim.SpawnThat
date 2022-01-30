using System.Collections.Generic;
using System.Linq;
using SpawnThat.Core.Network;
using SpawnThat.Core;
using SpawnThat.Spawners.LocalSpawner.Models;
using SpawnThat.Spawners.LocalSpawner.Managers;

namespace SpawnThat.Spawners.LocalSpawner.Sync;

internal class LocalSpawnerConfigPackage : CompressedPackage
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

        RegisterType(TemplatesBySpawnerName.Values.SelectMany(x => x.SpawnConditions));
        RegisterType(TemplatesBySpawnerName.Values.SelectMany(x => x.Modifiers));
        RegisterType(TemplatesByLocation.Values.SelectMany(x => x.SpawnConditions));
        RegisterType(TemplatesByLocation.Values.SelectMany(x => x.Modifiers));
        RegisterType(TemplatesByRoom.Values.SelectMany(x => x.SpawnConditions));
        RegisterType(TemplatesByRoom.Values.SelectMany(x => x.Modifiers));
    }

    protected override void AfterUnpack(object obj)
    {
        if (obj is LocalSpawnerConfigPackage configPackage)
        {
            Log.LogDebug("Received and deserialized local spawner config package");

            LocalSpawnTemplateManager.TemplatesBySpawnerName = configPackage.TemplatesBySpawnerName ?? new();
            LocalSpawnTemplateManager.TemplatesByLocation = configPackage.TemplatesByLocation ?? new();
            LocalSpawnTemplateManager.TemplatesByRoom = configPackage.TemplatesByRoom ?? new();

            int count =
                (LocalSpawnTemplateManager.TemplatesBySpawnerName.Count) +
                (LocalSpawnTemplateManager.TemplatesByLocation.Count) +
                (LocalSpawnTemplateManager.TemplatesByRoom.Count);

            Log.LogDebug($"Unpacked local spawner configurations: {count}");

            Log.LogInfo("Successfully unpacked local spawner configs.");
        }
        else
        {
            Log.LogWarning("Received bad config package. Unable to load.");
        }
    }
}
