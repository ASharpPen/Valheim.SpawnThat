using System.Collections.Generic;
using System.Linq;
using Valheim.SpawnThat.Startup;

namespace Valheim.SpawnThat.Spawners.WorldSpawner;

internal static class WorldSpawnTemplateManager
{
    private static Dictionary<int, WorldSpawnTemplate> templatesById = new();

    static WorldSpawnTemplateManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            templatesById = new();
        });
    }

    public static void SetTemplate(int id, WorldSpawnTemplate template)
    {
        templatesById[id] = template;
    }

    public static List<(int id, WorldSpawnTemplate template)> GetTemplates()
    {
        return templatesById
            .Select(x => (x.Key, x.Value))
            .ToList();
    }

    public static WorldSpawnTemplate GetTemplate(int id)
    {
        if (templatesById.TryGetValue(id, out WorldSpawnTemplate template))
        {
            return template;
        }

        return null;
    }
}
