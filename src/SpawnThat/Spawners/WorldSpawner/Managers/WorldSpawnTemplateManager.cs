﻿using System.Collections.Generic;
using System.Linq;
using SpawnThat.Core;
using SpawnThat.Lifecycle;

namespace SpawnThat.Spawners.WorldSpawner.Managers;

internal static class WorldSpawnTemplateManager
{
    public static Dictionary<int, WorldSpawnTemplate> TemplatesById { get; internal set; } = new();

    public static Dictionary<string, SimpleSpawnTemplate> SimpleTemplatesByPrefab { get; internal set; } = new();

    static WorldSpawnTemplateManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            TemplatesById = new();
            SimpleTemplatesByPrefab = new();
        });
    }

    public static void SetTemplate(int id, WorldSpawnTemplate template)
    {
        if (template.TemplateEnabled)
        {
            Log.LogTrace($"Assigned template [{template.Index}:{template.PrefabName}]");

            TemplatesById[id] = template;
        }
    }

    public static void SetTemplate(SimpleSpawnTemplate template)
    {
        SimpleTemplatesByPrefab[template.PrefabName.Trim().ToUpperInvariant()] = template;
    }

    public static List<(int id, WorldSpawnTemplate template)> GetTemplates()
    {
        return TemplatesById
            .Select(x => (x.Key, x.Value))
            .ToList();
    }

    public static WorldSpawnTemplate GetTemplate(int id)
    {
        if (TemplatesById.TryGetValue(id, out WorldSpawnTemplate template))
        {
            return template;
        }

        return null;
    }
}
