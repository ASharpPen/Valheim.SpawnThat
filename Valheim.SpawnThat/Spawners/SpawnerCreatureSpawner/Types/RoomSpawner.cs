using System.Linq;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.Types
{
    internal static class RoomSpawner
    {
        public static bool TryGetRoom(CreatureSpawner spawner, out RoomData room)
        {
            room = RoomCache.GetContainingRoom(spawner.transform.position);

            if(room is null)
            {
                return false;
            }

            return true;
        }

        public static bool ApplyConfig(CreatureSpawner spawner, string roomName)
        {
            var creatureName = spawner.m_creaturePrefab?.name;

            if (string.IsNullOrWhiteSpace(creatureName))
            {
                Log.LogTrace($"Empty creature in local spawner {spawner.name} at {spawner.transform.position}");
                return false;
            }

            var cleanedRoomName = roomName.Split(new[] {'(' }).FirstOrDefault();

            var config = CreatureSpawnerConfigCache.SearchConfig(cleanedRoomName, creatureName);

            if(config is null)
            {
                return false;
            }

            SpawnerModifier.ApplyConfiguration(spawner, config);

            return true;
        }
    }
}
