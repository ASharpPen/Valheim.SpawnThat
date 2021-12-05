using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.Utilities;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Simulators;

internal class DefaultRaidSimulator : DefaultSpawnSystemSimulator
{
    public Dictionary<string, List<DefaultSpawnSystemTemplate>> Raids { get; set; } = new();

    public override void Update()
    {
        var activeEvent = RandEventSystem.instance.m_activeEvent;

        if (activeEvent is not null)
        {
            if (Raids.TryGetValue(activeEvent.m_name, out var raidSpawns))
            {
                var zdos = GetSimulatedZones();

                Log.LogTrace("Simulating " + zdos.Count + " raid spawners.");

                foreach (var zone in zdos)
                {
                    SimulateSpawnSystem(zone, raidSpawns);
                }
            }
#if DEBUG
            else
            {
                Log.LogTrace("Unable to find event " + activeEvent.m_name);
            }
#endif
        }
        else
        {
            if (RandEventSystem.instance.m_randomEvent != null)
            {
                Log.LogTrace("Random event " + RandEventSystem.instance.m_randomEvent.m_name + " is ongoing, but not set as active.");
            }
        }
    }

    public List<ZDO> GetActiveRaidZones(RandomEvent randomEvent)
    {
        var players = PlayerUtils.GetPlayerZdosInRadius(randomEvent.m_pos, RandEventSystem.instance.m_randomEventRange);

        var activeZones = players
            .Select(x => ZoneSystem.instance.GetZone(x.GetPosition()))
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

    protected override int GetNearbyEntityCount(SpawnSessionContext sessionContext, DefaultSpawnSystemTemplate template)
    {
        return sessionContext.EntityAreaCounter.CountEntitiesInRange(template.PrefabHash, x => x.GetEventCreature());
    }
}
