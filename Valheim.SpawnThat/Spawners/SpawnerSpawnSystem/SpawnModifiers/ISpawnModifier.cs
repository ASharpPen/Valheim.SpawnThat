using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers
{
    public interface ISpawnModifier
    {
        void Modify(GameObject spawn, SpawnSystem.SpawnData spawner, SpawnConfiguration config);
    }
}
