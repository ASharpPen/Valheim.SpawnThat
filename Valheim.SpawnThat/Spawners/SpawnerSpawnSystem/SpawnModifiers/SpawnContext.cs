using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers
{
    public class SpawnContext
    {
        public SpawnSystem SpawnSystem { get; set; }
        public GameObject Spawn { get; set; }
        public SpawnSystem.SpawnData Spawner { get; set; }
        public SpawnConfiguration Config { get; set; }
    }
}
