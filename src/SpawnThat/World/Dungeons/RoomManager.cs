using System;
using System.Collections.Generic;
using UnityEngine;
using SpawnThat.Core.Cache;
using HarmonyLib;
using SpawnThat.Lifecycle;
using SpawnThat.Utilities.Spatial;
using SpawnThat.Utilities.Extensions;
using SpawnThat.Core;

namespace SpawnThat.World.Dungeons;

public static class RoomManager
{
    private static ManagedCache<RoomData> RoomTable = new();

    private static HashSet<Vector3> HasAdded = new HashSet<Vector3>();
    private static List<RoomData> RoomList = new List<RoomData>();

    static RoomManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            HasAdded = new HashSet<Vector3>();
            RoomList = new List<RoomData>();
        });
    }

    public static RoomData GetContainingRoom(Vector3 pos)
    {
        var closestRoom = FindClosestRoom(pos);

#if DEBUG && FALSE
        if (closestRoom is not null)
        {
            Log.LogTrace($"Testing bounds: {new Bounds(closestRoom.Pos, closestRoom.Size)} for room {closestRoom.Name} at {pos}");
        }
#endif

        return closestRoom;
    }

    private static RoomData FindClosestRoom(Vector3 pos)
    {
        if ((RoomList?.Count ?? 0) == 0)
        {
            return null;
        }

        RoomData closest = null;
        float closestDistance = float.MaxValue;

        //Get range + a bit to the left.
        int rangeFromX = RoomList.IndexLeft(pos.x - 20);

        //Get range + a bit to the right.
        int rangeToX = RoomList.IndexRight(pos.x + 20);

        for (int i = rangeFromX; i < rangeToX; ++i)
        {
            var room = RoomList[i];

            //Skip if way above/below room.
            if (Math.Abs(room.Pos.y - pos.y) > 100)
            {
                continue;
            }

            if (!room.Contains(pos))
            {
                continue;
            }

            var distX = pos.x - room.Pos.x;
            var distZ = pos.z - room.Pos.z;
            var distY = pos.y - room.Pos.y;

            var distance = Math.Abs(distX) + Math.Abs(distZ) + Math.Abs(distY);

            if (distance < closestDistance)
            {
                closest = room;
                closestDistance = distance;
            }
        }

        return closest;
    }

    internal static void AddRoom(Room room)
    {
        var data = RoomTable.GetOrCreate(room);

        var roomPos = room.transform.position;

        if (HasAdded.Add(roomPos))
        {
            data.Name = room.GetCleanedName();
            data.Pos = roomPos;

            // Fallbacks in case no sizes are registered.
            int sizeX = room.m_size.x == 0
                ? 10
                : room.m_size.x;
            int sizeY = room.m_size.y == 0
                ? 10
                : room.m_size.y;
            int sizeZ = room.m_size.z == 0
                ? 10
                : room.m_size.z;

            data.Size = new Vector3Int(sizeX * 2, sizeY * 2, sizeZ * 2);
            data.Rotation = room.transform.rotation;

            RoomList.Insert(data);
        }
    }

    internal static void RemoveRoom(RoomData room)
    {
        RoomList.Remove(room);
        HasAdded.Remove(room.Pos);
    }

    [HarmonyPatch(typeof(DungeonGenerator))]
    private static class DungeonGeneratorPatch
    {
        [HarmonyPatch(nameof(DungeonGenerator.PlaceRoom), new[] { typeof(DungeonDB.RoomData), typeof(Vector3), typeof(Quaternion), typeof(RoomConnection), typeof(ZoneSystem.SpawnMode) })]
        [HarmonyPostfix]
        private static void GetRoomObject(Room __result)
        {
            CacheRoom(__result);
        }

        private static void CacheRoom(Room component)
        {
#if DEBUG
            Log.LogDebug($"Registering room at {component.transform.position} with name {component.GetCleanedName()} - {new Bounds(component.transform.position, component.m_size)} - {component.transform.rotation}");
#endif

            RoomManager.AddRoom(component);
        }
    }
}
