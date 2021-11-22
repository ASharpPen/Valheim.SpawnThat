using UnityEngine;
using Valheim.SpawnThat.Utilities.World;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    /// <summary>
    /// Basic estimation of terrain angle, based on looking at nearby world height
    /// and taking the average tilt from center point.
    /// </summary>
    /// <remarks>Does not currently take into account terrain changes!</remarks>
    public class PositionConditionSurfaceTilt : ISpawnPositionCondition
    {
        private int MinTilt { get; }
        private int MaxTilt { get; }

        private static Vector2i North { get; } = new Vector2i(0, 1);
        private static Vector2i South { get; } = new Vector2i(0, -1);
        private static Vector2i West { get; } = new Vector2i(-1, 0);
        private static Vector2i East { get; } = new Vector2i(1, 0);

        public PositionConditionSurfaceTilt(int minTilt, int maxTilt)
        {
            MinTilt = minTilt;
            MaxTilt = maxTilt;
        }

        public bool IsValid(PositionContext context)
        {
            var point = context.Point;
            Zone zone = WorldData.GetZone(point);

            var center = zone.WorldToZoneCoordinate(point);
            var north = center + North;
            var south = center + South;
            var west = center + West;
            var east = center + East;

            var northHeight = zone.Height(north);
            var southHeight = zone.Height(south);
            var westHeight = zone.Height(west);
            var eastHeight = zone.Height(east);

            // These coordinates will be slightly off, since height was based off local grid coords
            // but whatever, hopefully it shouldn't matter too much *sweats profusely*.
            var toNorth = (new Vector3(point.x, northHeight, point.z + 1) - zone.ZonePos).normalized;
            var toSouth = (new Vector3(point.x, southHeight, point.z - 1) - zone.ZonePos).normalized;
            var toWest = (new Vector3(point.x - 1, westHeight, point.z) - zone.ZonePos).normalized;
            var toEast = (new Vector3(point.x + 1, eastHeight, point.z) - zone.ZonePos).normalized;

            var angleNorth = Vector3.Angle(Vector3.up, toNorth);
            var angleSouth = Vector3.Angle(Vector3.up, toSouth);
            var angleWest = Vector3.Angle(Vector3.up, toWest);
            var angleEast = Vector3.Angle(Vector3.up, toEast);

            var avgAngle = (angleNorth + angleSouth + angleWest + angleEast) / 4;

            if (avgAngle < MinTilt)
            {
                return false;
            }

            if (avgAngle > MaxTilt)
            {
                return false;
            }

            return true;
        }
    }
}
