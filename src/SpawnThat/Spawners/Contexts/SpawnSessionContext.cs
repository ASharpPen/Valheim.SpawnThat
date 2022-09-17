namespace SpawnThat.Spawners.Contexts;

public class SpawnSessionContext
{
    public SpawnSessionContext(ZDO spawnerZdo)
    {
        SpawnerZdo = spawnerZdo;
    }

    public ZDO SpawnerZdo { get; }
}
