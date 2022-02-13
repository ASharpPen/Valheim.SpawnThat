using System.Collections.Generic;
using System.Linq;
using SpawnThat.Lifecycle;

namespace SpawnThat.Spawners.DestructibleSpawner.Managers;

internal class DestructibleSpawnerTemplateManager
{
    internal static ICollection<DestructibleSpawnerTemplate> Templates { get; set; } = new List<DestructibleSpawnerTemplate>();

    static DestructibleSpawnerTemplateManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            Templates = new List<DestructibleSpawnerTemplate>();
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
}
