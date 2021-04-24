using System.Runtime.CompilerServices;

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
        private static ConditionalWeakTable<object, SpawnerInitStatus> Table = new ConditionalWeakTable<object, SpawnerInitStatus>();

        public static void SetInitStatus(this SpawnSystem spawner, SpawnerInitStatus cache) => InternalSetInitCache(spawner, cache);
        public static void SetInitStatus(this CreatureSpawner spawner, SpawnerInitStatus cache) => InternalSetInitCache(spawner, cache);

        private static void InternalSetInitCache(object obj, SpawnerInitStatus cache)
        {
            Table.Add(obj, cache);
        }

        public static void SetSuccessfulInit(this SpawnSystem spawner) => InternalSetSuccessfulInit(spawner);
        public static void SetSuccessfulInit(this CreatureSpawner spawner) => InternalSetSuccessfulInit(spawner);

        private static void InternalSetSuccessfulInit(object obj)
        {
            var cache = Table.GetOrCreateValue(obj);

            cache.Initialized = true;
            cache.FailedInit = false;
            cache.ShouldWait = false;
        }

        public static void SetInitialized(this SpawnSystem spawner, bool initialized = true) => InternalSetInitialized(spawner, initialized);
        public static void SetInitialized(this CreatureSpawner spawner, bool initialized = true) => InternalSetInitialized(spawner, initialized);

        private static void InternalSetInitialized(object obj, bool status)
        {
            Table.GetOrCreateValue(obj).Initialized = status;
        }

        public static bool IsInitialized(this SpawnSystem spawner) => InternalIsInitialized(spawner);
        public static bool IsInitialized(this CreatureSpawner spawner) => InternalIsInitialized(spawner);

        private static bool InternalIsInitialized(object obj)
        {
            if (Table.TryGetValue(obj, out SpawnerInitStatus cache))
            {
                return cache.Initialized;
            }

            return false;
        }

        public static void SetFailedInitialization(this SpawnSystem spawner, bool failed = true) => InternalSetFailedInitialization(spawner, failed);
        public static void SetFailedInitialization(this CreatureSpawner spawner, bool failed = true) => InternalSetFailedInitialization(spawner, failed);

        private static void InternalSetFailedInitialization(object obj, bool failed)
        {
            var cache = Table.GetOrCreateValue(obj);
            cache.FailedInit = failed;

            if (failed)
            {
                cache.FailedInitCount++;
            }
        }

        public static bool IsFailedInitialization(this SpawnSystem spawner) => InternalIsFailedInitialization(spawner);
        public static bool IsFailedInitialization(this CreatureSpawner spawner) => InternalIsFailedInitialization(spawner);

        private static bool InternalIsFailedInitialization(object obj)
        {
            if (Table.TryGetValue(obj, out SpawnerInitStatus cache))
            {
                return cache.FailedInit;
            }

            return false;
        }
        public static int GetFailedInitCount(this SpawnSystem spawner) => InternalGetFailedInitCount(spawner);
        public static int GetFailedInitCount(this CreatureSpawner spawner) => InternalGetFailedInitCount(spawner);

        private static int InternalGetFailedInitCount(object obj)
        {
            if (Table.TryGetValue(obj, out SpawnerInitStatus cache))
            {
                return cache.FailedInitCount;
            }

            return 0;
        }

        public static void SetShouldWait(this SpawnSystem spawner, bool shouldWait = true) => InternalSetShouldWait(spawner, shouldWait);
        public static void SetShouldWait(this CreatureSpawner spawner, bool shouldWait = true) => InternalSetShouldWait(spawner, shouldWait);

        private static void InternalSetShouldWait(object obj, bool shouldWait)
        {
            Table.GetOrCreateValue(obj).ShouldWait = shouldWait;
        }

        public static bool ShouldWait(this SpawnSystem spawner) => InternalShouldWait(spawner);
        public static bool ShouldWait(this CreatureSpawner spawner) => InternalShouldWait(spawner);

        private static bool InternalShouldWait(object obj)
        {
            if(Table.TryGetValue(obj, out SpawnerInitStatus cache))
            {
                return cache.ShouldWait;
            }

            return true;
        }
    }
}
