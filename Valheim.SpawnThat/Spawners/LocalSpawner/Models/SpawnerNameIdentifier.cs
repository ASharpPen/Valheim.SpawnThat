
namespace Valheim.SpawnThat.Spawners.LocalSpawner.Models;

public record SpawnerNameIdentifier
{
    public string SpawnerPrefabName { get; set; }

    public SpawnerNameIdentifier()
    { }

    public SpawnerNameIdentifier(string spawnerPrefabName)
    {
        SpawnerPrefabName = spawnerPrefabName;
    }
}
