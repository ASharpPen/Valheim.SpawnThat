using System.Collections.Generic;
using SpawnThat.Spawners.Contexts;
using static SpawnArea;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Models;

internal class SpawnSession : SpawnSessionContext
{
    public SpawnSession(ZDO spawnerZdo) : base(spawnerZdo)
    {
    }

    public SpawnData CurrentSpawn { get; set; }

    public SpawnAreaSpawnTemplate CurrentTemplate { get; set; }

    public List<SpawnData> OriginalSpawnData { get; set; }
}
