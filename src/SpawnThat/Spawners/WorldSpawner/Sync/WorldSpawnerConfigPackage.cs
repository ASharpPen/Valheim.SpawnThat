using System.Collections.Generic;
using System.Linq;
using SpawnThat.Core;
using SpawnThat.Core.Network;
using SpawnThat.Spawners.WorldSpawner.Managers;

namespace SpawnThat.Spawners.WorldSpawner.Sync;

internal class WorldSpawnerConfigPackage : CompressedPackage
{
    public Dictionary<int, WorldSpawnTemplate> TemplatesById;
    public Dictionary<string, SimpleSpawnTemplate> SimpleConfigsByPrefab;

    protected override void BeforePack()
    {
        TemplatesById = new(WorldSpawnTemplateManager.TemplatesById);
        SimpleConfigsByPrefab = new(WorldSpawnTemplateManager.SimpleTemplatesByPrefab);

        Log.LogDebug($"Packaged world spawner configurations: {TemplatesById?.Count ?? 0}");
        Log.LogDebug($"Packaged simple world spawner configurations: {SimpleConfigsByPrefab?.Count ?? 0}");

        RegisterType(TemplatesById.Values.SelectMany(x => x.SpawnConditions));
        RegisterType(TemplatesById.Values.SelectMany(x => x.SpawnPositionConditions));
        RegisterType(TemplatesById.Values.SelectMany(x => x.SpawnModifiers));
    }

    protected override void AfterUnpack(object obj)
    {
        if (obj is WorldSpawnerConfigPackage configPackage)
        {
            Log.LogDebug("Received and deserialized world spawner config package");

            WorldSpawnTemplateManager.TemplatesById = configPackage.TemplatesById ?? new();
            WorldSpawnTemplateManager.SimpleTemplatesByPrefab = configPackage.SimpleConfigsByPrefab ?? new();

            Log.LogDebug($"Unpacked world spawner configurations: {WorldSpawnTemplateManager.TemplatesById?.Count ?? 0}");
            Log.LogDebug($"Unpacked simple world spawner configurations: {WorldSpawnTemplateManager.SimpleTemplatesByPrefab?.Count ?? 0}");

            Log.LogInfo("Successfully unpacked world spawner configs.");
        }
        else
        {
            Log.LogWarning("Received bad config package. Unable to load.");
        }
    }
}
