using System;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

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

#if DEBUG
            var seconds = SecondsSinceLastSpawn(context.SpawnSystemZDO, template);
            Log.LogTrace("LastSpawn: " + seconds + ", CurrentTime: " + ZNet.instance.m_netTime + ", Diff: " + (ZNet.instance.m_netTime - seconds) + ", MaxDiff: " + SpawnInterval.TotalSeconds);

            return SpawnInterval.TotalSeconds < seconds;
#else
            return SpawnInterval.TotalSeconds < SecondsSinceLastSpawn(context.SpawnSystemZDO, template);
#endif
        }

        private double SecondsSinceLastSpawn(ZDO zdo, SpawnTemplate template)
        {
            double currentSecond = ZNet.instance.m_netTime;
            long lastSpawnSecond = zdo.GetLong(template.SpawnHash, 0L) / 10_000_000;

            return (currentSecond - lastSpawnSecond);
        }
    }
}
