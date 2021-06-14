﻿using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Maps.Managers;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionSpawnChanceInArea : IConditionOnSpawn
    {
        private static ConditionSpawnChanceInArea _instance;

        public static ConditionSpawnChanceInArea Instance => _instance ??= new();

        public bool ShouldFilter(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration config)
        {
            if (!spawner || spawner is null || config is null)
            {
                return false;
            }

            if (IsValid(spawner.transform.position, config))
            {
                return false;
            }

            Log.LogTrace($"Ignoring world config {config.Name} due to area chance.");
            return true;
        }

        public bool IsValid(Vector3 position, SpawnConfiguration config)
        {
            if (config.ConditionSpawnChanceInArea.Value >= 100)
            {
                return true;
            }

            if (config.ConditionSpawnChanceInArea.Value <= 0)
            {
                return false;
            }

            var areaChance = MapManager.GetAreaChance(position, config.Index) * 100;

            if(areaChance > config.ConditionSpawnChanceInArea.Value)
            {
                return false;
            }

            return true;
        }
    }
}
