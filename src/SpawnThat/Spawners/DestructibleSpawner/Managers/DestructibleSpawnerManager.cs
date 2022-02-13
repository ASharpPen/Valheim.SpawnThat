using SpawnThat.Core.Cache;
using SpawnThat.Spawners.DestructibleSpawner.Identifiers;

namespace SpawnThat.Spawners.DestructibleSpawner.Managers;

internal static class DestructibleSpawnerManager
{
    private static ManagedCache<DestructibleSpawnerTemplate> TemplateBySpawner { get; } = new();
    private static ManagedCache<bool> SpawnerIsConfigured { get; } = new();

    public static void ConfigureSpawner(SpawnArea spawner)
    {
        bool configureSpawner = true;

        if (SpawnerIsConfigured.TryGet(spawner, out bool isConfigured))
        {
            configureSpawner = !isConfigured;
        }

        if (configureSpawner)
        {
            var spawnerTemplate = FindTemplateMatch(spawner);

            if (spawnerTemplate is not null)
            {
                TemplateBySpawner.Set(spawner, spawnerTemplate);
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

        var templates = DestructibleSpawnerTemplateManager.GetTemplates();

        int maxMatchLevel = 0;
        double maxMatchScore = 0;
        DestructibleSpawnerTemplate maxTemplate = null;

        foreach (var template in templates)
        {
            int matchLevel = 0;
            double matchScore = 0;

            foreach (var identifier in template.Identifiers)
            {
                if (IsMatch(context, identifier))
                {
                    ++matchLevel;
                    matchScore += identifier.GetMatchWeight();
                }
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
