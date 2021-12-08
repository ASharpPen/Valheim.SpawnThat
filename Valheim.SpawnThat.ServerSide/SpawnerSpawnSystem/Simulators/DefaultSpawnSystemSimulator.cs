using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Core.Cache;
using Valheim.SpawnThat.Debugging.Gizmos;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;
using Valheim.SpawnThat.Utilities.World;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Simulators
{
    internal class DefaultSpawnSystemSimulator
    {
        public List<DefaultSpawnTemplate> Templates { get; set; } = new();

        private DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.UtcNow;

        public ISuggestPosition PositionSuggester { get; set; } = new PositionSuggesterDefault();

        public DefaultSpawnSystemSimulator()
        {
        }

        public virtual void Update()
        {
            if (LastUpdate + TimeSpan.FromSeconds(4) < DateTimeOffset.UtcNow)
            {
                LastUpdate = DateTimeOffset.UtcNow;

                var zdos = GetSimulatedZones();

#if false && DEBUG
                Log.LogTrace("Simulating " + zdos.Count + " spawners.");
#endif

                foreach (var zone in zdos)
                {
                    SimulateSpawnSystem(zone, Templates);
                }
            }
        }

        public void SimulateSpawnSystem(ZDO spawnSystem, List<DefaultSpawnTemplate> templates)
        {
            SpawnSessionContext sessionContext = new SpawnSessionContext
            {
                SpawnSystemZDO = spawnSystem
            };

#if false && DEBUG
            Log.LogTrace("Running spawn simulation for " + Templates.Count + " spawn templates.");
#endif

            foreach (var template in templates)
            {
                // Check conditions
                if (!template.Enabled)
                {
#if false && DEBUG
                    Log.LogTrace("Template " + template.Index + " disabled.");
#endif

                    continue;
                }

                if (!HasValidConditions(sessionContext, template))
                {
                    continue;
                }

                // Enforce density by identifying number of spawn rounds.
                var ticks = spawnSystem.GetLong(template.SpawnHash, 0L);
                var currentSeconds = ZNet.instance.m_netTime;

                var secondsSinceLastSpawn = currentSeconds - (ticks / 10_000_000);

                int spawnRounds = (int)(secondsSinceLastSpawn / template.SpawnInterval.TotalSeconds);

                if (spawnRounds < 1)
                {
#if false && DEBUG
                    Log.LogTrace($"Template " + template.Index + $" too early. {secondsSinceLastSpawn} / {template.SpawnInterval.TotalSeconds}");
#endif
                    continue;
                }

                // Update last spawn time.
                long currentTicks = (long)(currentSeconds * 10_000_000L);
                spawnSystem.Set(template.SpawnHash, currentTicks);

                var existingSpawns = GetNearbyEntityCount(sessionContext, template);
                var maxSpawns = template.MaxSpawned;

                for (int i = 0; i < spawnRounds; ++i)
                {
                    // Roll chance
#if false && DEBUG
                    var roll = UnityEngine.Random.Range(0f, 100f);
                    if (roll > template.SpawnChance)
                    {
                        if (template.Index <= 1)
                        {
                            Log.LogTrace($"[{template.Index}] SpawnChance {template.SpawnChance}, roll {roll} too low.");
                        }
                        continue;
                    }
#else 
                    if (UnityEngine.Random.Range(0f, 100f) > template.SpawnChance)
                    {
                        continue;
                    }
#endif

                    SpawnContext spawnContext = new SpawnContext(sessionContext)
                    {
                        Template = template,
                    };

                    // Find valid spawn point
                    var point = PositionSuggester.SuggestPosition(spawnContext);

                    if (point is null)
                    {
#if false && DEBUG
                        Log.LogTrace("Template " + template.Index + ":" + i + " no valid spawn point.");
#endif
                        continue;
                    }

                    // Spawn group
                    int randomGroupSize = UnityEngine.Random.Range(template.MinPackSize, template.MaxPackSize + 1);
                    int toSpawn = Math.Min(randomGroupSize, maxSpawns - existingSpawns);

#if DEBUG
                    if (toSpawn == 0 && this is DefaultRaidSimulator)
                    {
                        Log.LogTrace($"[{template.PrefabName}:{template.Index}] Max spawns already reached. {existingSpawns} out of  {maxSpawns}");
                    }
#endif

                    for (int j = 0; j < toSpawn; ++j)
                    {
                        // Calculate new position for each group spawn
                        float circleRadius = Math.Min(template.PackRadius, 0);
                        Vector2 circle = UnityEngine.Random.insideUnitCircle * circleRadius;

                        Vector3 spawnPoint = point.Value + new Vector3(circle.x, 0, circle.y);

                        var zone = WorldData.GetZone(spawnPoint);
                        var y = zone.Height(spawnPoint) + template.GroundOffset;

                        // Spawn entity
                        var entity = Spawn(template, new Vector3(spawnPoint.x, y, spawnPoint.z));

                        if (!entity || entity is null)
                        {
#if false && DEBUG
                                Log.LogTrace("Template " + template.Index + ":" + i + ":" + j + " spawned empty.");
#endif

                            continue;
                        }

                        // Modify spawn
                        var znetView = ComponentCache.GetComponent<ZNetView>(entity);
                        var zdo = (znetView && znetView != null)
                            ? znetView.GetZDO()
                            : null;
#if DEBUG
                        SphereGizmo.Create(entity.transform.position, 1f, lifeTime: TimeSpan.FromSeconds(30));
                        SphereGizmo.Create(entity.transform.position, 0.1f, lifeTime: TimeSpan.FromSeconds(30));
#endif

                        foreach (var modifier in template.Modifiers ?? new List<Modifiers.ISpawnModifier>(0))
                        {
                            try
                            {
                                modifier?.Modify(spawnContext, entity, zdo);
                            }
                            catch (Exception e)
                            {
                                Log.LogError($"Error while attempting to apply modifier '{modifier.GetType().Name}'.", e);
                            }
                        }

                        existingSpawns += 1;

                        if (existingSpawns >= maxSpawns)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public GameObject Spawn(DefaultSpawnTemplate template, Vector3 point)
        {
            var prefab = ZNetScene.instance.GetPrefab(template.PrefabHash);

            if (!prefab || prefab is null)
            {
                Log.LogWarning($"Unable to find prefab to spawn for '{template.PrefabName}'. Disabling spawn template '{template.Index}'.");

                template.Enabled = false;
                return null;
            }

            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, point, Quaternion.identity);

            Log.LogInfo($"Spawned {template.PrefabName}({template.Index}) x 1 ({GetType().Name})");

            return gameObject;
        }

        private bool HasValidConditions(SpawnSessionContext context, SpawnTemplate template)
        {
            return template.SpawnConditions.All(x =>
            {
#if true && DEBUG
                var valid = x.IsValid(context, template);

                if (!valid && this is DefaultRaidSimulator)
                {
                    Log.LogTrace($"[{template.Index}] Condition {x.GetType().Name} is invalid.");
                }

                return valid;
#else 
                return x?.IsValid(context, template) ?? true;
#endif
            });
        }

        protected virtual int GetNearbyEntityCount(SpawnSessionContext sessionContext, DefaultSpawnTemplate template)
        {
            return sessionContext.EntityAreaCounter.CountEntitiesInRange(template.PrefabHash);
        }

        public virtual List<ZDO> GetSimulatedZones()
        {
            var players = ZNet.instance.GetAllCharacterZDOS();

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
    }
}
