using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem
{
    public class SpawnConditionContext
    {
        public Vector3 Position { get; set; }

        public SpawnSystem.SpawnData SpawnData { get; set; }

        public SpawnConfiguration Config { get; set; }
    }
}
