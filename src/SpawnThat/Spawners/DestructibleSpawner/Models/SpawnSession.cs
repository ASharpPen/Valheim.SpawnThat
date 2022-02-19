using System.Collections.Generic;
using SpawnThat.Spawners.Contexts;
using static SpawnArea;

namespace SpawnThat.Spawners.DestructibleSpawner.Models;

internal class SpawnSession : SpawnSessionContext
{
    public SpawnSession(ZDO spawnerZdo) : base(spawnerZdo)
    {
    }

    public SpawnData CurrentSpawn { get; set; }

    public DestructibleSpawnTemplate CurrentTemplate { get; set; }

    public List<SpawnData> OriginalSpawnData { get; set; }
}
