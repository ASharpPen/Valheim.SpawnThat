using System;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Services
{
    public interface IEntityCounterService
    {
        public bool HasAnyInRange(int prefabHash);

        public int CountEntitiesInRange(int prefabHash, Func<ZDO, bool> condition = null);
    }
}
