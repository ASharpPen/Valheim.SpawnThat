using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpawnThat.Spawners.DestructibleSpawner.Configuration;

internal class DestructibleSpawnBuilder : IDestructibleSpawnBuilder
{
    public uint Id { get; }

    public DestructibleSpawnBuilder(uint id)
    {
        Id = id;
    }
}
