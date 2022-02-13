
using SpawnThat.Core;

namespace SpawnThat.Spawners.LocalSpawner.Models;

internal record SpawnerNameIdentifier
{
    public string SpawnerPrefabName { get; internal set; }

    public SpawnerNameIdentifier()
    { }

    public SpawnerNameIdentifier(string spawnerPrefabName)
    {
#if DEBUG
        if (string.IsNullOrWhiteSpace(spawnerPrefabName))
        {
            Log.LogWarning("LocalSpawner builder with empty SpawnerNameIdentifier detected.");
        }
#endif
        SpawnerPrefabName = spawnerPrefabName.Trim();
    }
}
