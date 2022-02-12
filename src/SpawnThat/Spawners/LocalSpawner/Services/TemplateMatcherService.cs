using SpawnThat.Spawners.LocalSpawner.Managers;
using SpawnThat.Spawners.LocalSpawner.Models;
using SpawnThat.Utilities.Extensions;
using SpawnThat.World.Dungeons;
using SpawnThat.World.Locations;

namespace SpawnThat.Spawners.LocalSpawner.Services;

internal static class TemplateMatcherService
{
    public static LocalSpawnTemplate MatchMostSpecificTemplate(CreatureSpawner spawner)
    {
        var position = spawner.transform.position;
        string creatureName = "";

        if (spawner.m_creaturePrefab.IsNotNull())
        {
            creatureName = spawner.m_creaturePrefab.GetCleanedName();
        }

        // Check room match
        var room = RoomManager.GetContainingRoom(position);

        if (room is not null)
        {
            var roomTemplate = LocalSpawnTemplateManager.GetTemplate(new RoomIdentifier(
                room.Name,
                creatureName));

            if (roomTemplate is not null)
            {
                return roomTemplate;
            }
        }

        // Check location match.
        var location = LocationManager.GetLocation(position);
        
        if (location is not null)
        {
            var locationTemplate = LocalSpawnTemplateManager.GetTemplate(new LocationIdentifier(
                location.LocationName,
                creatureName));

            if (locationTemplate is not null)
            {
                return locationTemplate;
            }
        }

        // Check spawner name match
        return LocalSpawnTemplateManager.GetTemplate(new SpawnerNameIdentifier(spawner.GetCleanedName()));
    }
}
