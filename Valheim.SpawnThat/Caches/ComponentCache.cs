using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Valheim.SpawnThat.Caches
{
    [Obsolete("Use Core.Cache.ComponentCache instead. This one still uses the potentially unreliable ConditionalWeakTable.")]
    public class ComponentCache
    {
        private static ConditionalWeakTable<GameObject, ComponentCache> Cache { get; } = new();

        public static T GetComponent<T>(GameObject obj) where T : Component
        {
            var cache = Cache.GetOrCreateValue(obj);

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
