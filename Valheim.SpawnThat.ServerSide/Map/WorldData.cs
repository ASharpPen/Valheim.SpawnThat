using System.Collections.Generic;
using UnityEngine;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.ServerSide.Map
{
    public static class WorldData
    {
        private static Dictionary<Vector2i, Zone> ZoneByZoneId { get; set; } = new();

        static WorldData()
        {
            StateResetter.Subscribe(() =>
            {
                ZoneByZoneId = new();
            });
        }

        public static Zone GetZone(Vector3 point)
        {
            return GetZone(point.GetZoneId());
        }

        public static Zone GetZone(Vector2i zoneId)
        {
            if (ZoneByZoneId.TryGetValue(zoneId, out var cached))
            {
                return cached;
            }

            return ZoneByZoneId[zoneId] = new Zone(zoneId);
        }
    }
}
