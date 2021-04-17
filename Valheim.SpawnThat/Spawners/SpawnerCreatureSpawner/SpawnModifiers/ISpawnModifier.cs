using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers
{
    public interface ISpawnModifier
    {
        void Modify(GameObject spawn, CreatureSpawnerConfig config);
    }
}
