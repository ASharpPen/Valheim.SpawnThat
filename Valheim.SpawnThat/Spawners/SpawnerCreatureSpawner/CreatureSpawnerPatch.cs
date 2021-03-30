using HarmonyLib;
using Valheim.SpawnThat.ConfigurationCore;
using Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.Types;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner
{
    [HarmonyPatch(typeof(CreatureSpawner), "Awake")]
    public static class CreatureSpawnerPatch
    {
        private static void Postfix(CreatureSpawner __instance)
        {
            //Identify if spawner is a location based or room based spawner.
            if (RoomSpawner.TryGetRoom(__instance, out RoomData room))
            {
#if DEBUG
                Log.LogDebug($"Starting modification of room creature spawner {__instance.name} in room {room.Name} at {__instance.transform.position}");
#endif
                //Spawner is an a room
                bool appliedConfig = RoomSpawner.ApplyConfig(__instance, room.Name);

                if(appliedConfig)
                {
                    return;
                }
            }

#if DEBUG
            Log.LogDebug($"Starting modification of location creature spawner {__instance.name} at {__instance.transform.position}");
#endif

            //Use location lookup to target.
            LocationSpawner.ApplyConfig(__instance);
            return;
        }
    }
}
