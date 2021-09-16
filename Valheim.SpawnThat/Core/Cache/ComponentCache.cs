using System;
using System.Collections.Generic;
using UnityEngine;

namespace Valheim.SpawnThat.Core.Cache
{
    public class ComponentCache
    {
        private static ManagedCache<ComponentCache> CacheTable { get; } = new();

        public static T GetComponent<T>(GameObject obj) where T : Component
        {
            ComponentCache cache = CacheTable.GetOrCreate(obj);

            Type componentType = typeof(T);

            if (cache.ComponentTable.TryGetValue(componentType, out Component cached))
            {
                return (T)cached;
            }

            if (obj.TryGetComponent(componentType, out Component component))
            {
                cache.ComponentTable.Add(componentType, component);
                return (T)component;
            }
            else
            {
                cache.ComponentTable.Add(componentType, null);
                return null;
            }
        }

        private Dictionary<Type, Component> ComponentTable { get; } = new();
    }
}
