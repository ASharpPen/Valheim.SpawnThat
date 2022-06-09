using System.Collections.Generic;
using System.Linq;
using SpawnThat.Core;
using SpawnThat.Core.Cache;
using SpawnThat.Lifecycle;
using SpawnThat.Options.Identifiers;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Spawners.SpawnAreaSpawner.Services;
using SpawnThat.Utilities.Extensions;
using YamlDotNet.Serialization;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Managers;

internal static class SpawnAreaSpawnerManager
{
    private static ManagedCache<SpawnAreaSpawnerTemplate> TemplateBySpawner { get; } = new();
    private static ManagedCache<bool> SpawnerIsConfigured { get; } = new();

    internal static ICollection<SpawnAreaSpawnerTemplate> Templates { get; set; } = new List<SpawnAreaSpawnerTemplate>();

    internal static bool DelaySpawners { get; set; } = true;

    static SpawnAreaSpawnerManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            Templates = new List<SpawnAreaSpawnerTemplate>();
            DelaySpawners = true;
        });
    }

    public static void AddTemplate(SpawnAreaSpawnerTemplate template)
    {
        Templates.Add(template);
    }

    public static List<SpawnAreaSpawnerTemplate> GetTemplates()
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
            Log.LogTrace($"Attempting to configure SpawnArea spawner '{spawner.name}:{spawner.transform.position}'");
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

    public static SpawnAreaSpawnerTemplate GetTemplate(SpawnArea spawner)
    {
        if (TemplateBySpawner.TryGet(spawner, out var template))
        {
            return template;
        }

        return null;
    }

    private static SpawnAreaSpawnerTemplate FindTemplateMatch(SpawnArea spawner)
    {
        var context = new IdentificationContext();
        context.Target = spawner.gameObject;
        context.Zdo = spawner.m_nview.GetZDO();

        var templates = GetTemplates();

        int maxMatchLevel = 0;
        double maxMatchScore = 0;
        SpawnAreaSpawnerTemplate maxTemplate = null;

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
                Log.LogTrace($"[SpawnArea Spawner] Identifier '{identifier.GetType().Name}'");
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
        if (context.TryGetCached(identifier, out bool cached))
        {
            return cached;
        }

        var match = identifier.IsValid(context);
        context.CacheIdentifierResult(identifier, match);

        return match;
    }
}
