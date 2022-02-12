using System.Collections.Generic;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners.LocalSpawner.Models;

namespace SpawnThat.Spawners.LocalSpawner.Managers;

internal static class LocalSpawnTemplateManager
{
    public static Dictionary<SpawnerNameIdentifier, LocalSpawnTemplate> TemplatesBySpawnerName { get; internal set; } = new();
    public static Dictionary<LocationIdentifier, LocalSpawnTemplate> TemplatesByLocation { get; internal set; } = new();
    public static Dictionary<RoomIdentifier, LocalSpawnTemplate> TemplatesByRoom { get; internal set; } = new();

    static LocalSpawnTemplateManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            TemplatesBySpawnerName = new();
            TemplatesByLocation = new();
            TemplatesByRoom = new();
        });
    }

    public static void SetTemplate(SpawnerNameIdentifier identifier, LocalSpawnTemplate template)
    {
        if (template.TemplateEnabled)
        {
            TemplatesBySpawnerName[identifier] = template;
        }
    }

    public static void SetTemplate(LocationIdentifier identifier, LocalSpawnTemplate template)
    {
        if (template.TemplateEnabled)
        {
            TemplatesByLocation[identifier] = template;
        }
    }

    public static void SetTemplate(RoomIdentifier identifier, LocalSpawnTemplate template)
    {
        if (template.TemplateEnabled)
        {
            TemplatesByRoom[identifier] = template;
        }
    }

    public static LocalSpawnTemplate GetTemplate(SpawnerNameIdentifier identifier)
    {
        if (TemplatesBySpawnerName.TryGetValue(identifier, out LocalSpawnTemplate template))
        {
            return template;
        }
        return null;
    }

    public static LocalSpawnTemplate GetTemplate(LocationIdentifier identifier)
    {
        if (TemplatesByLocation.TryGetValue(identifier, out LocalSpawnTemplate template))
        {
            return template;
        }
        return null;
    }

    public static LocalSpawnTemplate GetTemplate(RoomIdentifier identifier)
    {
        if (TemplatesByRoom.TryGetValue(identifier, out LocalSpawnTemplate template))
        {
            return template;
        }
        return null;
    }
}