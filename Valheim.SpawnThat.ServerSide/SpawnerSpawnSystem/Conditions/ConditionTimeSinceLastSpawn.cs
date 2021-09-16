using System;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionTimeSinceLastSpawn : ISpawnCondition
    {
        public TimeSpan SpawnInterval { get; }

        public ConditionTimeSinceLastSpawn(TimeSpan spawnInterval)
        {
            SpawnInterval = spawnInterval;
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (TimeSpan.Zero == SpawnInterval)
            {
                return true;
            }

            double interval = SpawnInterval.TotalSeconds;

            if (interval < 0)
            {
                return false;
            }

            return SpawnInterval.TotalSeconds < SecondsSinceLastSpawn(context.SpawnSystemZDO, template);
        }

        private double SecondsSinceLastSpawn(ZDO zdo, SpawnTemplate template)
        {
            double currentSecond = ZNet.instance.m_netTime;
            long lastSpawnSecond = zdo.GetLong(template.SpawnHash, 0L) * 10_000_000;

            return (currentSecond - lastSpawnSecond);
        }
    }
}
