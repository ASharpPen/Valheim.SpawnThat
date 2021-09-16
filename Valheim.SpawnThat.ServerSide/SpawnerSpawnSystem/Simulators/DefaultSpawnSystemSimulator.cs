using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Simulators
{
    internal class DefaultSpawnSystemSimulator
    {
        public List<SpawnTemplate> Templates { get; set; } = new();

        private DateTimeOffset lastUpdate { get; set; } = DateTimeOffset.UtcNow;

        public void Update()
        {
            if (lastUpdate + TimeSpan.FromSeconds(4) < DateTimeOffset.UtcNow)
            {
                lastUpdate = DateTimeOffset.UtcNow;

                var zdos = GetSimulatedZones();

                foreach (var zone in zdos)
                {
                    SimulateSpawnSystem(zone, Templates);
                }
            }
        }

        public void SimulateSpawnSystem(ZDO spawnSystem, List<SpawnTemplate> templates)
        {
            SpawnSessionContext sessionContext = new SpawnSessionContext
            {
                SpawnSystemZDO = spawnSystem
            };

            foreach (var template in Templates)
            {
                // Check conditions
                if (!HasValidConditions(sessionContext, template))
                {
                    continue;
                }

                // Enforce density by identifying number of spawn rounds.
                var ticks = spawnSystem.GetLong(template.SpawnHash, 0L);
                var currentSeconds = ZNet.instance.m_netTime;

                var secondsSinceLastSpawn = currentSeconds / (ticks / 10_000_000);

                var spawnFrequency = template.SpawnConditions.FirstOrDefault(x => x is ConditionTimeSinceLastSpawn) as ConditionTimeSinceLastSpawn;

                int spawnRounds = 1;
                if (spawnFrequency is not null)
                {
                    spawnRounds = Math.Max(1, (int)(spawnFrequency.SpawnInterval.TotalSeconds / secondsSinceLastSpawn));
                }

                var existingSpawns = sessionContext.EntityAreaCounter.CountEntitiesInRange(template.PrefabHash);

                for (int i = 0; i < spawnRounds; ++i)
                {
                    // Find valid spawn point

                    // Spawn group

                    // Modify spawn

                    // Update last spawn time.
                }

                spawnSystem.Set(template.SpawnHash, (long)(currentSeconds * 10_000_000));
            }
        }

        private bool HasValidConditions(SpawnSessionContext context, SpawnTemplate template)
        {
            return template.SpawnConditions.All(x => x.IsValid(context, template));
        }

        public List<ZDO> GetSimulatedZones()
        {
            var players = Player.GetAllPlayers();

            var activeZones = players
                .Select(x => x.m_nview.GetZDO().GetPosition())
                .Select(x => ZoneSystem.instance.GetZone(x))
                .Distinct()
                .ToList();

            List<ZDO> spawnSystemZDOs = new List<ZDO>(activeZones.Count);

            foreach (var activeZone in activeZones)
            {
                List<ZDO> zoneZDOs = new();
                ZDOMan.instance.FindObjects(activeZone, zoneZDOs);

                var spawnSystemZdo = zoneZDOs.FirstOrDefault(x => x.m_prefab == GameConstants.SpawnSystemPrefabHash);

                if (spawnSystemZdo is not null)
                {
                    spawnSystemZDOs.Add(spawnSystemZdo);
                }
            }

            return spawnSystemZDOs;
        }

        private 
    }
}
