using System.Collections.Generic;
using Valheim.SpawnThat.Spawners.LocalSpawner.Configuration;
using Valheim.SpawnThat.Startup;

namespace Valheim.SpawnThat.Spawners.LocalSpawner;

internal static class LocalSpawnTemplateManager
{
    private static Dictionary<SpawnerNameIdentifier, LocalSpawnTemplate> templatesBySpawnerName = new();
    private static Dictionary<LocationIdentifier, LocalSpawnTemplate> templatesByLocation = new();
    private static Dictionary<RoomIdentifier, LocalSpawnTemplate> templatesByRoom = new();

    static LocalSpawnTemplateManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            templatesBySpawnerName = new();
            templatesByLocation = new();
            templatesByRoom = new();
        });
    }

    public static void SetTemplate(SpawnerNameIdentifier identifier, LocalSpawnTemplate template)
    {
        templatesBySpawnerName[identifier] = template;
    }

    public static void SetTemplate(LocationIdentifier identifier, LocalSpawnTemplate template)
    {
        templatesByLocation[identifier] = template;
    }

    public static void SetTemplate(RoomIdentifier identifier, LocalSpawnTemplate template)
    {
        templatesByRoom[identifier] = template;
    }

    public static LocalSpawnTemplate GetTemplate(SpawnerNameIdentifier identifier)
    {
        if (templatesBySpawnerName.TryGetValue(identifier, out LocalSpawnTemplate template))
        {
            return template;
        }
        return null;
    }

    public static LocalSpawnTemplate GetTemplate(LocationIdentifier identifier)
    {
        if (templatesByLocation.TryGetValue(identifier, out LocalSpawnTemplate template))
        {
            return template;
        }
        return null;
    }

    public static LocalSpawnTemplate GetTemplate(RoomIdentifier identifier)
    {
        if (templatesByRoom.TryGetValue(identifier, out LocalSpawnTemplate template))
        {
            return template;
        }
        return null;
    }
}