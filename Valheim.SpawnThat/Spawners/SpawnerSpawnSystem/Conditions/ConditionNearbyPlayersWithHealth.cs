using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionNearbyPlayersWithHealth : IConditionOnSpawn
    {
        private static ConditionNearbyPlayersWithHealth _instance;

        public static ConditionNearbyPlayersWithHealth Instance => _instance ??= new();

        public bool ShouldFilter(SpawnConditionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
