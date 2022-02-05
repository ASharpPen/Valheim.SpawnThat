
namespace SpawnThat.Spawners.LocalSpawner.Models;

internal record SpawnerNameIdentifier
{
    public string SpawnerPrefabName { get; internal set; }

    internal SpawnerNameIdentifier()
    { }

    public SpawnerNameIdentifier(string spawnerPrefabName)
    {
        SpawnerPrefabName = spawnerPrefabName;
    }
}
