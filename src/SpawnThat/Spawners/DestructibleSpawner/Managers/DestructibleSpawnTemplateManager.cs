using System.Runtime.CompilerServices;
using static SpawnArea;

namespace SpawnThat.Spawners.DestructibleSpawner.Managers;

internal static class DestructibleSpawnTemplateManager
{
    private static ConditionalWeakTable<SpawnData, DestructibleSpawnTemplate> SpawnTemplates { get; } = new();

    public static void SetTemplate(SpawnData key, DestructibleSpawnTemplate template)
    {
        if (SpawnTemplates.TryGetValue(key, out _))
        {
            SpawnTemplates.Remove(key);
        }

        SpawnTemplates.Add(key, template);
    }

    public static DestructibleSpawnTemplate GetTemplate(SpawnData key)
    {
        if (SpawnTemplates.TryGetValue(key, out var cached))
        {
            return cached;
        }

        return null;
    }

    public static bool TryGetTemplate(SpawnData key, out DestructibleSpawnTemplate template)
    {
        return SpawnTemplates.TryGetValue(key, out template);
    }
}
