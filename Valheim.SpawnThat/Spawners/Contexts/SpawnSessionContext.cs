namespace Valheim.SpawnThat.Spawners.Contexts;

public class SpawnSessionContext
{
    public ZDO SpawnerZdo { get; }

    public SpawnSessionContext(ZDO spawnerZdo)
    {
        SpawnerZdo = spawnerZdo;
    }
}
