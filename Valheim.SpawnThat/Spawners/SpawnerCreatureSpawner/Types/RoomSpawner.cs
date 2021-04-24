using System;
using System.Linq;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.Types
{
    internal static class RoomSpawner
    {
        public static bool TryGetRoom(CreatureSpawner spawner, out RoomData room)
        {
            try
            {
                room = RoomCache.GetContainingRoom(spawner.transform.position);

                if (room is null)
                {
                    return false;
                }

                return true;
            }
            catch(Exception e)
            {
                Log.LogWarning($"Error while attempting to check dungeon room of spawner {spawner}.: " + e.Message);
                room = null;
                return false;
            }
        }

        public static bool ApplyConfig(CreatureSpawner spawner, string roomName)
        {
            try
            {
                var creatureName = spawner.m_creaturePrefab?.name;

                if (string.IsNullOrWhiteSpace(creatureName))
                {
                    Log.LogTrace($"Empty creature in local spawner {spawner.name} at {spawner.transform.position}");
                    return false;
                }

                var cleanedRoomName = roomName.Split(new[] { '(' }).FirstOrDefault();

                var config = ConfigFinder.SearchConfig(cleanedRoomName, creatureName);

                if (config is null)
                {
                    return false;
                }

                SpawnerConfigApplier.ApplyConfiguration(spawner, config);

                return true;
            }
            catch(Exception e)
            {
                Log.LogWarning($"Error while attempting to apply config to local spawner {spawner}: " + e.Message);
                return false;
            }
        }
    }
}
