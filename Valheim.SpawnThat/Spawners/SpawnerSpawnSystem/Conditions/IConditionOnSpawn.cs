using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.Configuration.ConfigTypes;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public interface IConditionOnSpawn
    {
        bool ShouldFilter(SpawnSystem.SpawnData spawner, SpawnConfiguration config);
    }
}
