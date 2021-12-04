using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Services;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models
{
    public class SpawnSessionContext
    {
        private IEntityCounterService entityAreaCounter;

        public ZDO SpawnSystemZDO { get; set; }

        public IEntityCounterService EntityAreaCounter => entityAreaCounter ??= new EntityCounterService(SpawnSystemZDO.GetPosition(), 128);
    }
}
