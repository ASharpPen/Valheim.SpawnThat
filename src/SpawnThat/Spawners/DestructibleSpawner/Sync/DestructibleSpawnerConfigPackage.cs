using System.Collections.Generic;
using System.Linq;
using SpawnThat.Core;
using SpawnThat.Core.Network;
using SpawnThat.Spawners.DestructibleSpawner.Managers;

namespace SpawnThat.Spawners.DestructibleSpawner.Sync;

internal class DestructibleSpawnerConfigPackage : CompressedPackage
{
    public List<DestructibleSpawnerTemplate> Templates;

    protected override void BeforePack()
    {
        Templates = DestructibleSpawnerManager.GetTemplates();

        Log.LogDebug($"Packaged world spawner configurations: {Templates?.Count ?? 0}");

        var spawns = Templates.SelectMany(x => x.Spawns.Values);

        RegisterType(spawns.SelectMany(x => x.Conditions));
        RegisterType(spawns.SelectMany(x => x.PositionConditions));
        RegisterType(spawns.SelectMany(x => x.Modifiers));
    }

    protected override void AfterUnpack(object obj)
    {
        if (obj is DestructibleSpawnerConfigPackage configPackage)
        {
            Log.LogDebug("Received and deserialized destructible spawner config package");

            DestructibleSpawnerManager.Templates = configPackage.Templates ?? new();

            Log.LogInfo($"Successfully unpacked destructible spawner configurations: {DestructibleSpawnerManager.Templates?.Count ?? 0}");
        }
        else
        {
            Log.LogWarning("Received bad config package. Unable to load.");
        }
    }
}