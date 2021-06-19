using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Maps.Managers;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    public class ConditionAreaIds : IConditionOnSpawn
    {
        private static ConditionAreaIds _instance;

        public static ConditionAreaIds Instance => _instance ??= new();

        public bool ShouldFilter(SpawnConditionContext context)
        {
            if (IsValid(context.Position, context.Config))
            {
                return false;
            }

            Log.LogTrace($"Ignoring world config {context.Config.Name} due to not being in area with required id.");
            return true;
        }

        public bool IsValid(int areaId, SpawnConfiguration config)
        {
            if (config is null)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(config.ConditionAreaIds.Value))
            {
                return true;
            }

            var areaIds = config.ConditionAreaIds.Value.SplitByComma(true);

            if (areaIds.Count == 0)
            {
                return true;
            }

            var id = areaId.ToString();
            if (areaIds.Any(x => x == id))
            {
#if DEBUG
                Log.LogTrace($"Found required area with id: {areaId}");
#endif
                return true;
            }

            return false;
        }

        public bool IsValid(Vector3 position, SpawnConfiguration config)
        {
            if(config is null)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(config.ConditionAreaIds.Value))
            {
                return true;
            }

            var areaIds = config.ConditionAreaIds.Value.SplitByComma(true);

            if (areaIds.Count == 0)
            {
                return true;
            }

            var areaId = MapManager.GetAreaId(position).ToString().ToUpperInvariant();

            if (areaIds.Any(x => x == areaId))
            {
#if DEBUG
                Log.LogTrace($"Found required area with id: {areaId}");
#endif
                return true;
            }

            return false;
        }
    }
}
