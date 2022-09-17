using System.Runtime.CompilerServices;
using static SpawnArea;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Managers;

internal static class SpawnAreaSpawnTemplateManager
{
    private static ConditionalWeakTable<SpawnData, SpawnAreaSpawnTemplate> SpawnTemplates { get; } = new();

    public static void SetTemplate(SpawnData key, SpawnAreaSpawnTemplate template)
    {
        if (SpawnTemplates.TryGetValue(key, out _))
        {
            SpawnTemplates.Remove(key);
        }

        SpawnTemplates.Add(key, template);
    }

    public static SpawnAreaSpawnTemplate GetTemplate(SpawnData key)
    {
        if (SpawnTemplates.TryGetValue(key, out var cached))
        {
            return cached;
        }

        return null;
    }

    public static bool TryGetTemplate(SpawnData key, out SpawnAreaSpawnTemplate template)
    {
        return SpawnTemplates.TryGetValue(key, out template);
    }
}
