using System;
using System.Collections.Generic;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Managers
{
    public class SpawnSessionManager
    {
        private static SpawnSessionManager _instance;

        public static SpawnSessionManager Instance = _instance ??= new();

        public static void ResetSession()
        {
            _instance = null;
        }

        private Dictionary<Type, object> Services { get; } = new();

        public T GetService<T>() where T : class
        {
            if (Services.TryGetValue(typeof(T), out object result))
            {
                return (T)result;
            }

            return null;
        }

        public void SetService<T>(T service) where T : class
        {
            Services[typeof(T)] = service;
        }
    }
}
