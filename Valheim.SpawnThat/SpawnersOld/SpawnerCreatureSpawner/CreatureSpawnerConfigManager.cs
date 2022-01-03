using System;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.Caches;
using Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.Types;
using Valheim.SpawnThat.Spawners.WorldSpawner.Caches;
using Valheim.SpawnThat.Startup;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner
{
    public static class CreatureSpawnerConfigManager
    {
        internal static bool Wait = true;

        static CreatureSpawnerConfigManager()
        {
            StateResetter.Subscribe(() =>
            {
                Wait = true;
            });
        }

        public static void ApplyConfigsIfMissing(CreatureSpawner __instance)
        {
            if (Wait)
            {
                return;
            }

            if (__instance.IsInitialized())
            {
                return;
            }

            try
            {
                ApplyConfigs(__instance);
            }
            catch (Exception e)
            {
                Log.LogError($"Error while applying configs to local spawner {__instance}.", e);
                __instance.SetFailedInitialization();
            }

            if (__instance.IsFailedInitialization() && __instance.GetFailedInitCount() >= 2)
            {
                Log.LogTrace($"Too many failed initialization attempts for spawner {__instance}, will stop retrying.");
                __instance.SetInitialized(true);
                __instance.SetShouldWait(false);
                return;
            }
        }

        public static void ApplyConfigs(CreatureSpawner __instance)
        {
            if (!ConfigurationManager.GeneralConfig.EnableLocalSpawner.Value)
            {
                __instance.SetSuccessfulInit();
                return;
            }

            if (ConfigurationManager.CreatureSpawnerConfig is not null && ConfigurationManager.CreatureSpawnerConfig.Subsections.Count == 0)
            {
                __instance.SetSuccessfulInit();
                return;
            }

            //Identify if spawner is a location based or room based spawner.
            if (RoomSpawner.TryGetRoom(__instance, out RoomData room))
            {
#if DEBUG
                Log.LogDebug($"Starting modification of room creature spawner {__instance.name} in room {room.Name} at {__instance.transform.position}");
#endif
                //Spawner is an a room
                RoomSpawner.ApplyConfig(__instance, room.Name);
            }

#if DEBUG
            Log.LogDebug($"Starting modification of location creature spawner {__instance.name} at {__instance.transform.position}");
#endif

            if (!__instance.IsInitialized())
            {
                //Use location lookup to target.
                LocationSpawner.ApplyConfig(__instance);
            }

            if(!__instance.IsInitialized())
            {
                __instance.SetFailedInitialization();
            }
        }
    }
}
