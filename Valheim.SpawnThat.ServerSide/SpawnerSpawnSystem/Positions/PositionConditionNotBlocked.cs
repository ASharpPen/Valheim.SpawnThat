using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    internal class PositionConditionNotBlocked
    {
        public PositionConditionNotBlocked()
        {
            ZNetScene.instance.m_prefabs.Select(x => x.gameObject.layer);


        }
    }
}
