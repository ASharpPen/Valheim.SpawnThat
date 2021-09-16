using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int SpawnRounds { get; set; }

        public ZDO SpawnSystemZdo { get; set; }
    }
}
