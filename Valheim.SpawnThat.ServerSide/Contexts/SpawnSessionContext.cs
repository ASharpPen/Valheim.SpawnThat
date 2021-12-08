using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Services;

namespace Valheim.SpawnThat.ServerSide.Contexts;

public class SpawnSessionContext
{
    private IEntityCounterService entityAreaCounter;

    public IEntityCounterService EntityAreaCounter => entityAreaCounter ??= new EntityCounterService(SpawnerZdo.GetPosition(), 128);

    public ZDO SpawnerZdo { get; }

    public SpawnSessionContext(ZDO spawnerZdo)
    {
        SpawnerZdo = spawnerZdo;
    }
}
