using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Core.Cache;
using Valheim.SpawnThat.Debugging.Gizmos;
using Valheim.SpawnThat.Locations;
using Valheim.SpawnThat.ServerSide.Contexts;
using Valheim.SpawnThat.ServerSide.SpawnConditions;
using Valheim.SpawnThat.ServerSide.SpawnerCreatureSpawner.Data;
using Valheim.SpawnThat.ServerSide.SpawnerCreatureSpawner.Templates;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Services;
using Valheim.SpawnThat.ServerSide.SpawnModifiers;
using Valheim.SpawnThat.ServerSide.SpawnPositionConditions;
using Valheim.SpawnThat.ServerSide.Utilities;
using Valheim.SpawnThat.Utilities.Spatial;

namespace Valheim.SpawnThat.ServerSide.SpawnerCreatureSpawner.Simulators;

public class CreatureSpawnerSimulator
{
    private static KeyValuePair<int, int> SpawnedIdHash { get; }
    private static int AliveTimeHash = "alive_time".GetStableHashCode();

    private DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.UtcNow;

    static CreatureSpawnerSimulator()
    {
        SpawnedIdHash = new KeyValuePair<int, int>("spawn_id_u".GetStableHashCode(), "spawn_id_i".GetStableHashCode());
    }

    public virtual void Update()
    {
        if (LastUpdate + TimeSpan.FromSeconds(4) < DateTimeOffset.UtcNow)
        {
            LastUpdate = DateTimeOffset.UtcNow;

            var spawners = GetSimulatedSpawners();

#if DEBUG
            Log.LogTrace("Simulating " + spawners.Count + " creature spawners.");
#endif

            foreach (var spawner in spawners)
            {
                Simulate(spawner);
            }
        }
    }

    public void Simulate(ZDO spawner)
    {
        // Get spawner prefab / template
        var template = GetTemplate(spawner);
        var spawnerPos = spawner.GetPosition();

#if DEBUG
        var loc = LocationHelper.FindLocation(spawnerPos);
        string spawnName = loc != null
            ? $"{loc.LocationName}.{template.PrefabName}"
            : template.PrefabName;
#endif

        ZDOID spawnedId = spawner.GetZDOID(SpawnedIdHash);

        // Check if spawned previously
        if (!spawnedId.IsNone())
        {
            if (template.SpawnInterval.TotalSeconds <= 0)
            {
                // Respawn disabled. Ignore the spawner from this point.
#if DEBUG
                Log.LogTrace($"[{spawnName}] Respawn disabled.");
#endif
                return;
            }
            // Check if alive
            var spawnZdo = ZDOMan.instance.GetZDO(spawnedId);
            if (spawnZdo != null)
            {
#if DEBUG
                Log.LogTrace($"[{spawnName}] Is alive.");
#endif

                // Set timestamp in ticks.
                spawner.Set(AliveTimeHash, (long)(ZNet.instance.m_netTime * 10_000_000));
                return;
            }
        }

        // Check if respawn time is too early
        var currentTimeTicks = (long)(ZNet.instance.m_netTime * 10_000_000);

        if (template.SpawnInterval.TotalSeconds > 0)
        {
            var lastTimestampTicks = spawner.GetLong(AliveTimeHash, 0L);
            var diff = currentTimeTicks - lastTimestampTicks;

            if (diff < template.SpawnInterval.Ticks)
            {
#if DEBUG
                Log.LogTrace($"[{spawnName}] Too early.");
#endif
                return;
            }
        }

        // Check condition day
        // Check condition night
        // Check condition playerbase
        // Check condition noise
        // Check condition trigger distance
        var session = new SpawnSessionContext(spawner);

        if (!template.SpawnConditions.All(x =>
        {
            try
            {
#if DEBUG
                var valid = x.IsValid(session);
                if (!valid)
                {
                    Log.LogTrace($"[{spawnName}] condition '{x.GetType().Name}' is invalid.");
                }
                return valid;
#else
                return x.IsValid(session);
#endif
            }
            catch (Exception e)
            {
                Log.LogWarning($"[{spawnName}] Error during condition '{x.GetType().Name}', skipping spawn '{template.PrefabName}'", e);
                return false;
            }
        }))
        {
            return;
        }

        if (!template.PositionConditions.All(x =>
        {
#if DEBUG
            var valid = x.IsValid(session, spawnerPos);
            if (!valid)
            {
                Log.LogTrace($"[{spawnName}] position condition '{x.GetType().Name}' is invalid.");
            }
            return valid;
#else
            return x.IsValid(session, spawnerPos);
#endif
        }))
        {
            return;
        }

        // Do Spawn
        (var entity, var entityZdo) = Spawn(spawner, template);

        if (entity == null)
        {
            return;
        }

#if DEBUG
        SphereGizmo.Create(entity.transform.position, 1f, lifeTime: TimeSpan.FromSeconds(30));
        SphereGizmo.Create(entity.transform.position, 0.1f, lifeTime: TimeSpan.FromSeconds(30));
#endif

        // Set link from spawner to spawn
        spawner.Set(SpawnedIdHash, entityZdo.m_uid);
        // Set spawner spawn time
        spawner.Set(AliveTimeHash, currentTimeTicks);

        // Do SpawnEffect
        SpawnEffect(spawner, entity);
    }

    private void SpawnEffect(ZDO spawner, GameObject spawnedObject)
    {
        var spawnerPrefab = ZNetScene.instance.GetPrefab(spawner.GetPrefab());
        if (!spawnerPrefab || spawnerPrefab is null)
        {
            return;
        }

        var creatureSpawner = ComponentCache.GetComponent<CreatureSpawner>(spawnerPrefab);
        if (creatureSpawner is null)
        {
            return;
        }

        Character component = ComponentCache.GetComponent<Character>(spawnedObject);
        if (component is null)
        {
            return;
        }

        Vector3 basePos = component ? component.GetCenterPoint() : (spawner.GetPosition() + Vector3.up * 0.75f);

        creatureSpawner.m_spawnEffects.Create(basePos, Quaternion.identity, null, 1f, -1);
    }

    private (GameObject entity, ZDO entityZdo) Spawn(ZDO spawner, DefaultLocalTemplate template)
    {
        // Find position.
        var spawnerPos = spawner.GetPosition();

        var pos = new Vector3(spawnerPos.x, spawnerPos.y + template.GroundOffset, spawnerPos.z);
        var rot = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);

        // Instantiate prefab
        var prefab = ZNetScene.instance.GetPrefab(template.PrefabHash);

        if (!prefab || prefab is null)
        {
            Log.LogWarning($"Unable to find prefab '{template.PrefabName}'. Entity is not loaded, verify installation or name.");
            return (null, null);
        }

        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, pos, rot);

        Log.LogDebug($"Spawned {template.PrefabName} x 1 ({nameof(CreatureSpawnerSimulator)})");

        // Set patrol point
        // Roll level
        var znetView = ComponentCache.GetComponent<ZNetView>(gameObject);
        var zdo = (znetView && znetView != null)
            ? znetView.GetZDO()
            : null;

        template.Modifiers.ForEach(x => x.Apply(gameObject, zdo));

        // Set PGW_Version to match spawner.
        zdo?.SetPGWVersion(spawner.GetPGWVersion());
                
        return (gameObject, zdo);
    }

    // TODO: Hook into something that can figure out which template to ACTUALLY return, and not just a default converter.
    public virtual DefaultLocalTemplate GetTemplate(ZDO spawnerZdo)
    {
        var prefab = ZNetScene.instance.GetPrefab(spawnerZdo.m_prefab);
        var spawner = ComponentCache.GetComponent<CreatureSpawner>(prefab);

        if (!prefab || prefab is null || spawner is null)
        {
#if DEBUG
            Log.LogWarning($"Unable to find a creature spawner with hash '{spawnerZdo.m_prefab}'.");
#endif
            return null;
        }

        if (spawner.m_creaturePrefab is null || !spawner.m_creaturePrefab)
        {
            return null;
        }

        var template = new DefaultLocalTemplate();

        template.PrefabHash = spawner.m_creaturePrefab.name.GetStableHashCode();
        template.PrefabName = spawner.m_creaturePrefab.name;

#if DEBUG
        template.SpawnInterval = TimeSpan.FromSeconds(5);
#else
        template.SpawnInterval = TimeSpan.FromMinutes(spawner.m_respawnTimeMinuts);
#endif

        // Conditions
        if (!spawner.m_spawnAtDay)
        {
            template.SpawnConditions.Add(new ConditionSpawnDuringDay(spawner.m_spawnAtDay));
        }

        if (!spawner.m_spawnAtNight)
        {
            template.SpawnConditions.Add(new ConditionSpawnDuringNight(spawner.m_spawnAtNight));
        }

        if ((int)spawner.m_triggerNoise > 0)
        {
            template.SpawnConditions.Add(new ConditionNoise((int)spawner.m_triggerNoise, (int)spawner.m_triggerNoise));
        }

        template.SpawnConditions.Add(new ConditionCloseToPlayer(spawner.m_triggerDistance));

        // Position Conditions
        if (!spawner.m_spawnInPlayerBase)
        {
            template.PositionConditions.Add(new PositionConditionPlayerBase());
        }

        // Modifiers
        if (spawner.m_setPatrolSpawnPoint)
        {
            template.Modifiers.Add(new SpawnModifierSetPatrol());
        }
        template.Modifiers.Add(new SpawnModifierDefaultRollLevel(spawner.m_minLevel, spawner.m_maxLevel, 0, 10));

        return template;
    }

    // TODO: A lot of potential optimizations possible here, with regards to caching zones
    public virtual List<ZDO> GetSimulatedSpawners()
    {
        var players = ZNet.instance.GetAllCharacterZDOS();

        // TODO: Find all loaded zones, and not just zones with players in them
        var activeZones = players
            .Select(x => ZoneSystem.instance.GetZone(x.GetPosition()))
            .Distinct()
            .ToList();

        List<ZDO> spawners = new List<ZDO>(activeZones.Count);

        var hashes = CreatureSpawnerPrefabData.CreatureSpawnerHashes;

        foreach (var activeZone in activeZones)
        {
            List<ZDO> zoneZDOs = new();
            ZDOMan.instance.FindObjects(activeZone, zoneZDOs);

            var creatureSpawnerZdos = zoneZDOs.Where(x => hashes.Contains(x.m_prefab));

            spawners.AddRange(creatureSpawnerZdos);
        }

        return spawners;
    }

    private class FindFloorQuery : ZdoQuery
    {
        public FindFloorQuery(Vector3 center) : base(center, 2)
        {
        }

        public ZDO FirstSolid()
        {
            foreach (var zdo in Zdos)
            {
                // Filter for nearby zdos.
                if (!IsWithinRange(zdo))
                {
                    continue;
                }

                var isSolid = PrefabData.IsSolid(zdo.GetPrefab());

                if (isSolid)
                {
                    return zdo;
                }
            }

            return null;
        }
    }
}
