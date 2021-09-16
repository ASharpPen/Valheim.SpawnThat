using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using static Heightmap;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    public class PositionContext
    {
        public PositionContext(SpawnContext context)
        {
            SpawnContext = context;
            SessionContext = context.SessionContext;
        }

        public SpawnSessionContext SessionContext { get; private set; }

        public SpawnContext SpawnContext { get; private set; }

        public Vector3 Point { get; set; }
    }
}
