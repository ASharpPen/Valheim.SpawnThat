using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Position
{
    public interface ISpawnPositionCondition
    {
        bool ShouldFilter(SpawnSystem.SpawnData spawn, SpawnConfiguration config, Vector3 position);
    }
}
