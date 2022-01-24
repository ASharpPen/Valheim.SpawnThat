using System;
using System.Collections.Generic;
using UnityEngine;
using Valheim.SpawnThat.Core.Cache;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Utilities;
using HarmonyLib;
using System.Reflection.Emit;
using System.Reflection;
using Valheim.SpawnThat.Lifecycle;

namespace Valheim.SpawnThat.World.Dungeons;

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
        var closestRoom = RoomList.FindClosest(pos);

        if (closestRoom is null)
        {
            return null;
        }

        var roomBounds = new Bounds(closestRoom.Pos, closestRoom.Size);

#if DEBUG
        Log.LogTrace($"Testing bounds: {roomBounds} for room {closestRoom.Name} at {pos}");
#endif

        if (roomBounds.Contains(pos))
        {
            return closestRoom;
        }

        return null;
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

    internal static void AddRoom(Room room)
    {
        var data = RoomTable.GetOrCreate(room);

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

            data.Size = new Vector3Int(sizeX * 2, sizeY * 2, sizeZ * 2);

            RoomList.Insert(data);
        }
    }

    [HarmonyPatch(typeof(DungeonGenerator))]
    private static class DungeonGeneratorPatch
    {
        private static MethodInfo Anchor = AccessTools.Method(typeof(GameObject), nameof(GameObject.GetComponent), generics: new[] { typeof(Room) });

        [HarmonyPatch(nameof(DungeonGenerator.PlaceRoom), new[] { typeof(DungeonDB.RoomData), typeof(Vector3), typeof(Quaternion), typeof(RoomConnection), typeof(ZoneSystem.SpawnMode) })]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> GetRoomObject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Callvirt, Anchor))
                .Advance(2)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_1))
                .InsertAndAdvance(Transpilers.EmitDelegate(CacheRoom))
                .InstructionEnumeration();
        }

        private static void CacheRoom(Room component)
        {
#if !DEBUG
            if((ConfigurationManager.CreatureSpawnerConfig?.Subsections?.Count ?? 0) == 0)
            {
                //Skip if we have no spawner configs to actually use the rooms for.
                return;
            }
#endif

#if DEBUG
            Log.LogDebug($"Registering room at {component.transform.position} with name {component.name}");
#endif

            RoomManager.AddRoom(component);
        }
    }


}
