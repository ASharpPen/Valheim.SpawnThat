using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner
{
    [HarmonyPatch(typeof(DungeonGenerator))]
    public static class DungeonGeneratorPatch
    {
        private static MethodInfo Anchor = AccessTools.Method(typeof(GameObject), "GetComponent", generics: new[] { typeof(Room) });
        private static MethodInfo Detour = AccessTools.Method(typeof(DungeonGeneratorPatch), nameof(CacheRoom), new[] { typeof(Room) });

        [HarmonyPatch("PlaceRoom", new[] { typeof(DungeonDB.RoomData), typeof(Vector3), typeof(Quaternion), typeof(RoomConnection), typeof(ZoneSystem.SpawnMode) })]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> HookRoomObject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Callvirt, Anchor))
                .Advance(2)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_1))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, Detour))
                .InstructionEnumeration();
        }

        private static void CacheRoom(Room component)
        {
#if !DEBUG
            if(ConfigurationManager.CreatureSpawnerConfig.Count == 0)
            {
                //Skip if we have no spawner configs to actually use the rooms for.
                return;
            }
#endif

#if DEBUG
            Log.LogDebug($"Registering room at {component.transform.position} with name {component.name}");
#endif

            RoomCache.AddRoom(component);
        }
    }
}
