using System;
using UnityEngine;

namespace Valheim.SpawnThat.Locations
{
    public class SimpleLocation
    {
        public Vector3 Position;

        public Vector2i ZonePosition;

        public string LocationName;
    }

    [Serializable]
    public struct SimpleLocationDTO
    {
        public int ZonePositionX;
        public int ZonePositionY;

        public float PositionX;
        public float PositionY;
        public float PositionZ;

        public string LocationName;

        public SimpleLocationDTO(Vector2i zonePos, Vector3 pos, string location)
        {
            ZonePositionX = zonePos.x;
            ZonePositionY = zonePos.y;

            PositionX = pos.x;
            PositionY = pos.y;
            PositionZ = pos.z;

            LocationName = location;
        }

        public SimpleLocation ToSimpleLocation()
        {
            return new SimpleLocation
            {
                Position = new Vector3(PositionX, PositionY, PositionZ),
                ZonePosition = new Vector2i(ZonePositionX, ZonePositionY),
                LocationName = LocationName
            };
        }
    }
}
