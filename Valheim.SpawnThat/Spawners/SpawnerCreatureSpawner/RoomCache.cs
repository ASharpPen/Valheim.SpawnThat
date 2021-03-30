using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Valheim.SpawnThat.ConfigurationCore;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner
{
    internal static class RoomCache
    {
        private static ConditionalWeakTable<Room, RoomData> RoomTable = new ConditionalWeakTable<Room, RoomData>();

        private static HashSet<Vector3> HasAdded = new HashSet<Vector3>();
        private static List<RoomData> RoomList = new List<RoomData>();

        static RoomCache()
        {
            StateResetter.Subscribe(() =>
            {
                HasAdded = new HashSet<Vector3>();
                RoomList = new List<RoomData>();
            });
        }

        public static void AddRoom(Room room)
        {
            var data = RoomTable.GetOrCreateValue(room);

            var roomPos = room.transform.position;

            if (HasAdded.Add(roomPos))
            {
                data.Name = room.name;
                data.Pos = roomPos;

                //Jesus, fix your shit IronGate!
                int sizeX = room.m_size.x == 0
                    ? 10
                    : room.m_size.x;
                int sizeY = room.m_size.y == 0
                    ? 10
                    : room.m_size.y;
                int sizeZ = room.m_size.z == 0
                    ? 10
                    : room.m_size.z;

                data.Size = new Vector3Int(sizeX*2, sizeY*2, sizeZ*2);

                RoomList.Insert(data);
            }
        }

        public static RoomData GetContainingRoom(CreatureSpawner spawner)
        {
            var spawnerPos = spawner.transform.position;
            var closestRoom = RoomList.FindClosest(spawnerPos);

            if (closestRoom is null)
            {
                return null;
            }

            var roomBounds = new Bounds(closestRoom.Pos, closestRoom.Size);

#if DEBUG
            Log.LogDebug($"Testing bounds: {roomBounds} for spawner {closestRoom.Name} at {spawnerPos}");
#endif 

            if(roomBounds.Contains(spawnerPos))
            {
                return closestRoom;
            }

            return null;
        }

        private static RoomData FindClosestRoom(Vector3 pos)
        {
            if((RoomList?.Count ?? 0) == 0)
            {
                return null;
            }

            RoomData closest = null;
            float closestDistance = float.MaxValue;

            //Get range + a bit to the left.
            int rangeFromX = RoomList.IndexLeft(pos.x - 20);
            
            //Get range + a bit to the right.
            int rangeToX = RoomList.IndexRight(pos.x + 20);

#if DEBUG

            //Log.LogDebug($"Indexes: {rangeFromX}, {rangeToX}");

            //Log.LogDebug("Checking rooms: " + RoomList.GetRange(rangeFromX, Math.Max(0, rangeToX - rangeFromX)).Select(x => x.Pos).Join());
#endif


            for (int i = rangeFromX; i < rangeToX; ++i)
            {
                var room = RoomList[i];

                //Skip if way above/below room.
                if(Math.Abs(room.Pos.y - pos.y) > 100)
                {
                    continue;
                }

                var distX = pos.x - room.Pos.x;
                var distZ = pos.z - room.Pos.z;

                var distance = Math.Abs(distX) + Math.Abs(distZ);

                if (distance < closestDistance)
                {
                    closest = room;
                    closestDistance = distance;
                }
            }

            return closest;
        }
    }

    internal class RoomData : IPoint, IHaveVector3
    {
        public string Name;

        public Vector3Int Size;

        private Vector3 _pos;

        public Vector3 Pos 
        { 
            get
            {
                return _pos;
            }

            set
            {
                _pos = value;
                X = value.x;
                Y = value.z;
            }
        }

        public float X { get; set; }
        public float Y { get; set; }
    }
}
