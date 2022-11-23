using System.Collections.Generic;
using System.Linq;
using SpawnThat.Core;
using SpawnThat.Core.Network;
using SpawnThat.Spawners.SpawnAreaSpawner.Managers;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Sync;

internal class SpawnAreaSpawnerConfigPackage : CompressedPackage
{
    public List<SpawnAreaSpawnerTemplate> Templates;

    protected override void BeforePack()
    {
        Templates = SpawnAreaSpawnerManager.GetTemplates();

        Log.LogDebug($"Packaged SpawnArea spawner configurations: {Templates?.Count ?? 0}");

        RegisterType(Templates.SelectMany(x => x.Identifiers));

        var spawns = Templates.SelectMany(x => x.Spawns.Values);

        RegisterType(spawns.SelectMany(x => x.Conditions));
        RegisterType(spawns.SelectMany(x => x.PositionConditions));
        RegisterType(spawns.SelectMany(x => x.Modifiers));
    }

    protected override void AfterUnpack(object obj)
    {
        if (obj is SpawnAreaSpawnerConfigPackage configPackage)
        {
            Log.LogDebug("Received and deserialized SpawnArea spawner config package");

            SpawnAreaSpawnerManager.Templates = configPackage.Templates ?? new();

            Log.LogInfo($"Successfully unpacked SpawnArea spawner configurations: {SpawnAreaSpawnerManager.Templates?.Count ?? 0}");
        }
        else
        {
            Log.LogWarning("Received bad config package. Unable to load.");
        }
    }
}