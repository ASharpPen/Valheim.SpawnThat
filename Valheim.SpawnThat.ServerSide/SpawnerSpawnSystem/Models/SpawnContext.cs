using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models
{
    public class SpawnContext
    {
        public SpawnContext(SpawnSessionContext sessionContext)
        {
            SessionContext = sessionContext;
        }

        public SpawnSessionContext SessionContext { get; private set; }

        public SpawnTemplate Template { get; set; }

        public ZDO SpawnSystemZdo => SessionContext.SpawnSystemZDO;
    }
}
