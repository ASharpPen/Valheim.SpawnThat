﻿using System.Collections.Generic;
using System.Linq;
using SpawnThat.Core;
using SpawnThat.Core.Cache;
using SpawnThat.Lifecycle;
using SpawnThat.Options.Identifiers;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Spawners.DestructibleSpawner.Services;
using SpawnThat.Utilities.Extensions;
using YamlDotNet.Serialization;

namespace SpawnThat.Spawners.DestructibleSpawner.Managers;

internal static class DestructibleSpawnerManager
{
    private static ManagedCache<DestructibleSpawnerTemplate> TemplateBySpawner { get; } = new();
    private static ManagedCache<bool> SpawnerIsConfigured { get; } = new();

    internal static ICollection<DestructibleSpawnerTemplate> Templates { get; set; } = new List<DestructibleSpawnerTemplate>();

    internal static bool DelaySpawners { get; set; } = true;

    static DestructibleSpawnerManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            Templates = new List<DestructibleSpawnerTemplate>();
            DelaySpawners = true;
        });
    }

    public static void AddTemplate(DestructibleSpawnerTemplate template)
    {
        Templates.Add(template);
    }

    public static List<DestructibleSpawnerTemplate> GetTemplates()
    {
        return Templates.ToList();
    }

    public static void ConfigureSpawner(SpawnArea spawner)
    {
        bool configureSpawner = true;

        if (SpawnerIsConfigured.TryGet(spawner, out bool isConfigured))
        {
            configureSpawner = !isConfigured;
        }

        if (configureSpawner)
        {
#if DEBUG
            Log.LogTrace($"Attempting to configure destructible spawner '{spawner.name}:{spawner.transform.position}'");
#endif

            var spawnerTemplate = FindTemplateMatch(spawner);

            if (spawnerTemplate is not null)
            {
#if DEBUG
                Log.LogTrace($"Found template '{spawnerTemplate.TemplateName}'");
#endif

                TemplateBySpawner.Set(spawner, spawnerTemplate);

                ConfigApplicationService.ConfigureSpawner(spawner, spawnerTemplate);
            }

            SpawnerIsConfigured.Set(spawner, true);
        }
    }

    public static DestructibleSpawnerTemplate GetTemplate(SpawnArea spawner)
    {
        if (TemplateBySpawner.TryGet(spawner, out var template))
        {
            return template;
        }

        return null;
    }

    private static DestructibleSpawnerTemplate FindTemplateMatch(SpawnArea spawner)
    {
        var context = new IdentificationContext();
        context.Target = spawner.gameObject;
        context.Zdo = spawner.m_nview.GetZDO();

        var templates = GetTemplates();

        int maxMatchLevel = 0;
        double maxMatchScore = 0;
        DestructibleSpawnerTemplate maxTemplate = null;

        foreach (var template in templates)
        {
#if DEBUG
            Log.LogTrace($"Checking template '{template.TemplateName}:{spawner.GetName()}:{spawner.transform.position}'");

#endif
            int matchLevel = 0;
            double matchScore = 0;

            foreach (var identifier in template.Identifiers)
            {
#if DEBUG
                Log.LogTrace($"[Destructible Spawner] Identifier '{identifier.GetType().Name}'");
#endif
                if (IsMatch(context, identifier))
                {
                    ++matchLevel;
                    matchScore += identifier.GetMatchWeight();
                }
                else
                {
                    matchLevel = 0;
                    break;
                }
            }

            if (matchLevel == 0)
            {
                continue;
            }

            if (matchLevel > maxMatchLevel)
            {
                maxMatchLevel = matchLevel;
                maxMatchScore = matchScore;
                maxTemplate = template;
            }
            else if (matchLevel == maxMatchLevel && matchScore > maxMatchScore)
            {
                maxMatchScore = matchScore;
                maxTemplate = template;
            }
        }

        return maxTemplate;
    }

    private static bool IsMatch(IdentificationContext context, ISpawnerIdentifier identifier)
    {
        if (identifier is ICacheableIdentifier cacheable)
        {
            if (context.TryGetCached(cacheable, out bool cached))
            {
                return cached;
            }
            else
            {
                var match = identifier.IsValid(context);
                context.CacheIdentifierResult(cacheable, match);

                return match;
            }
        }

        return identifier.IsValid(context);
    }
}
