using Valheim.SpawnThat.Spawners.LocalSpawner.Models;
using Valheim.SpawnThat.World.Dungeons;
using Valheim.SpawnThat.World.Locations;

namespace Valheim.SpawnThat.Spawners.LocalSpawner.Services;

internal static class TemplateMatcherService
{
    public static LocalSpawnTemplate MatchMostSpecificTemplate(CreatureSpawner spawner)
    {
        var position = spawner.transform.position;
        string creatureName = "";

        if (spawner.m_creaturePrefab is not null || spawner.m_creaturePrefab)
        {
            creatureName = spawner.m_creaturePrefab.name?.ToUpperInvariant()?.Trim();
        }

        // Check room match
        var room = RoomManager.GetContainingRoom(position);

        if (room is not null)
        {
            var roomTemplate = LocalSpawnTemplateManager.GetTemplate(new RoomIdentifier(
                room.Name.ToUpperInvariant().Trim(),
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
                location.LocationName.ToUpperInvariant().Trim(),
                creatureName));

            if (locationTemplate is not null)
            {
                return locationTemplate;
            }
        }

        // Check spawner name match
        if (string.IsNullOrWhiteSpace(spawner.name))
        {
            return null;
        }

        var spawnerName = spawner.name.ToUpperInvariant().Trim();

        return LocalSpawnTemplateManager.GetTemplate(new SpawnerNameIdentifier(spawnerName));
    }
}
