using UnityEngine;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.Spawners.Caches
{
    public class SpawnerInitStatus
    {
        public bool Initialized { get; set; }
        public bool FailedInit { get; set; }
        public int FailedInitCount { get; set; }
        public bool ShouldWait { get; set; } = true;
    }

    public static class SpawnerInitCache
    {
        private static ManagedCache<SpawnerInitStatus> Cache = new();

        public static void SetSuccessfulInit(this SpawnSystem spawner) => InternalSetSuccessfulInit(spawner);
        public static void SetSuccessfulInit(this CreatureSpawner spawner) => InternalSetSuccessfulInit(spawner);

        private static void InternalSetSuccessfulInit(Component obj)
        {
            var cache = Cache.GetOrCreate(obj);

            cache.Initialized = true;
            cache.FailedInit = false;
            cache.ShouldWait = false;
        }

        public static void SetInitialized(this SpawnSystem spawner, bool initialized = true) => InternalSetInitialized(spawner, initialized);
        public static void SetInitialized(this CreatureSpawner spawner, bool initialized = true) => InternalSetInitialized(spawner, initialized);

        private static void InternalSetInitialized(Component obj, bool status)
        {
            Cache.GetOrCreate(obj).Initialized = status;
        }

        public static bool IsInitialized(this SpawnSystem spawner) => InternalIsInitialized(spawner);
        public static bool IsInitialized(this CreatureSpawner spawner) => InternalIsInitialized(spawner);

        private static bool InternalIsInitialized(Component obj)
        {
            if (Cache.TryGet(obj, out SpawnerInitStatus cache))
            {
                return cache.Initialized;
            }

            return false;
        }

        public static void SetFailedInitialization(this SpawnSystem spawner, bool failed = true) => InternalSetFailedInitialization(spawner, failed);
        public static void SetFailedInitialization(this CreatureSpawner spawner, bool failed = true) => InternalSetFailedInitialization(spawner, failed);

        private static void InternalSetFailedInitialization(Component obj, bool failed)
        {
            var cache = Cache.GetOrCreate(obj);
            cache.FailedInit = failed;

            if (failed)
            {
                cache.FailedInitCount++;
            }
        }

        public static bool IsFailedInitialization(this SpawnSystem spawner) => InternalIsFailedInitialization(spawner);
        public static bool IsFailedInitialization(this CreatureSpawner spawner) => InternalIsFailedInitialization(spawner);

        private static bool InternalIsFailedInitialization(Component obj)
        {
            if (Cache.TryGet(obj, out SpawnerInitStatus cache))
            {
                return cache.FailedInit;
            }

            return false;
        }
        public static int GetFailedInitCount(this SpawnSystem spawner) => InternalGetFailedInitCount(spawner);
        public static int GetFailedInitCount(this CreatureSpawner spawner) => InternalGetFailedInitCount(spawner);

        private static int InternalGetFailedInitCount(Component obj)
        {
            if (Cache.TryGet(obj, out SpawnerInitStatus cache))
            {
                return cache.FailedInitCount;
            }

            return 0;
        }

        public static void SetShouldWait(this SpawnSystem spawner, bool shouldWait = true) => InternalSetShouldWait(spawner, shouldWait);
        public static void SetShouldWait(this CreatureSpawner spawner, bool shouldWait = true) => InternalSetShouldWait(spawner, shouldWait);

        private static void InternalSetShouldWait(Component obj, bool shouldWait)
        {
            Cache.GetOrCreate(obj).ShouldWait = shouldWait;
        }

        public static bool ShouldWait(this SpawnSystem spawner) => InternalShouldWait(spawner);
        public static bool ShouldWait(this CreatureSpawner spawner) => InternalShouldWait(spawner);

        private static bool InternalShouldWait(Component obj)
        {
            if(Cache.TryGet(obj, out SpawnerInitStatus cache))
            {
                return cache.ShouldWait;
            }

            return true;
        }
    }
}
