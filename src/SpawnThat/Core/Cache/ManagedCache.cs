﻿using System;
using System.Collections.Generic;
using System.Linq;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Core.Cache;

/// <summary>
/// Automatically cleaned cache for mapping UnityEngine Objects.
/// 
/// Intended to replace ConditionalWeakTable for types that
/// are not supported by WeakReference in Unity.
/// </summary>
public class ManagedCache<TValue> : IDisposable, IManagedCache
{
    private Dictionary<UnityEngine.Object, TValue> Storage { get; } = new();

    public ManagedCache()
    {
        CacheCleaner.Subscribe(this);
    }

    public void Set(UnityEngine.Object key, TValue value)
    {
        lock (Storage)
        {
            Storage[key] = value;
        }
    }

    public bool TryGet(UnityEngine.Object key, out TValue value)
    {
        lock (Storage)
        {
            return Storage.TryGetValue(key, out value);
        }
    }

    public TValue GetOrCreate(UnityEngine.Object key)
    {
        if (TryGet(key, out TValue cached))
        {
            return cached;
        }

        var newValue = Activator.CreateInstance<TValue>();
        Set(key, newValue);

        if (newValue is IManagedValue managed)
        {
            managed.OnCreate();
        }

        return newValue;
    }

    public void Clean()
    {
        lock (Storage)
        {
            int cleaned = 0;
            int total = Storage.Count;

            foreach (var pair in Storage.ToArray())
            {
                if (pair.Key.IsNull())
                {
                    Storage.Remove(pair.Key);
                    cleaned++;

                    if (pair.Value is IManagedValue managed)
                    {
                        managed.OnDestroy();
                    }
                }
            }

#if false && DEBUG
                Log.LogTrace($"Cleaned {cleaned} dead keys out of {total} from managed dictionary {Storage.GetHashCode()}");
#endif
        }
    }

    public void Dispose()
    {
        CacheCleaner.Unsubscribe(this);
    }
}
