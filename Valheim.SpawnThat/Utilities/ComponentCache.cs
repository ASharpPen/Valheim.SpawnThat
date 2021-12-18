using System;
using System.Collections.Generic;
using UnityEngine;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.Utilities;

public class ComponentCache
{
    private static ManagedCache<ComponentCache> CacheTable { get; } = new();
    private Dictionary<Type, Component> ComponentTable { get; } = new();

    public static ZDO GetZdo(GameObject obj)
    {
        if (!obj || obj is null)
        {
            return null;
        }

        var znetView = Get<ZNetView>(obj);

        if (znetView == null)
        {
            return null;
        }

        return znetView.GetZDO();
    }

    public static ZDO GetZdo(Component obj)
    {
        if (!obj || obj is null)
        {
            return null;
        }

        return GetZdo(obj.gameObject);
    }

    public static T Get<T>(GameObject obj) where T : Component
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

    public static T GetInChildren<T>(GameObject obj, bool includeInactive = false)
        where T : Component
    {
        ComponentCache cache = CacheTable.GetOrCreate(obj);

        Type componentType = typeof(T);

        if (cache.ComponentTable.TryGetValue(componentType, out Component cached))
        {
            return (T)cached;
        }

        T component = obj.GetComponentInChildren<T>(includeInactive);

        if (component is not null)
        {
            cache.ComponentTable.Add(componentType, component);
            return component;
        }
        else
        {
            cache.ComponentTable.Add(componentType, null);
            return null;
        }
    }
}
