using BepInEx;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Debugging
{
    public static class RoomCreatureSpawnerFileDumper
    {
        private const string FileName = "local_spawners_dungeons_pre_changes.txt";

        public static void WriteToFile(List<DungeonDB.RoomData> rooms)
        {
            List<string> spawnersSerialized = new List<string>();

            if(rooms is null)
            {
                Log.LogWarning("Rooms are null");
                return;
            }

            if(rooms.Count  == 0)
            {
                Log.LogWarning("No rooms to write.");

                return;
            }

            try
            {
                foreach (var room in rooms
                    .Select(x => x.m_room)
                    .OrderBy(x => x.m_theme)
                    .ThenBy(x => x.name))
                {
                    if(room.gameObject == null)
                    {
                        Log.LogTrace($"Room {room.name} gameobject is null");
                    }
                }
            }
            catch(Exception e)
            {
                Log.LogError("Wtf?", e);
            }

            var orderedRooms = rooms
                .Select(x => x.m_room)
                .OrderBy(x => x.m_theme)
                .ThenBy(x => x.name);

            foreach (var room in orderedRooms)
            {
                var roomPrefab = room;

                if (roomPrefab is null)
                {
#if DEBUG
                    Log.LogDebug($"No gameobject for room {room.name}");
#endif
                    continue;
                }

                var spawners = roomPrefab.GetComponentsInChildren<CreatureSpawner>();

                if (spawners is null)
                {
#if DEBUG
                    Log.LogDebug($"No spawners for room {room.name}");
#endif
                    continue;
                }

                if (spawners != null && spawners.Length > 0)
                {
                    spawnersSerialized.Add(Serialize(room, spawners));
                }
            }

            string filePath = Path.Combine(Paths.PluginPath, FileName);

            Log.LogInfo($"Writing defalt local spawn dungeon configurations to {filePath}");

            File.WriteAllLines(filePath, spawnersSerialized);
        }

        private static string Serialize(Room room, IList<CreatureSpawner> spawners)
        {
            if (!ConfigurationManager.GeneralConfig.DontCollapseFile.Value)
            {
                spawners = spawners.GroupBy(x => x.m_creaturePrefab.name).Select(x => x.First()).ToList();
            }

            var theme = room.m_theme;
            var roomName = room.name;

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < spawners.Count; ++i)
            {
                var spawner = spawners[i];

                stringBuilder.AppendLine($"## Room Theme: {theme}, Room: {roomName}, Spawner: {i} ");
                stringBuilder.AppendLine($"[{roomName}.{spawner.m_creaturePrefab.name}]");

                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.PrefabName)}={spawner.m_creaturePrefab.name}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.Enabled)}={spawner.enabled}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.SpawnAtDay)}={spawner.m_spawnAtDay}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.SpawnAtNight)}={spawner.m_spawnAtNight}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.LevelMin)}={spawner.m_minLevel}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.LevelMax)}={spawner.m_maxLevel}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.LevelUpChance)}={spawner.m_levelupChance.ToString(CultureInfo.InvariantCulture)}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.RespawnTime)}={spawner.m_respawnTimeMinuts.ToString(CultureInfo.InvariantCulture)}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.TriggerDistance)}={spawner.m_triggerDistance.ToString(CultureInfo.InvariantCulture)}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.TriggerNoise)}={spawner.m_triggerNoise.ToString(CultureInfo.InvariantCulture)}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.SpawnInPlayerBase)}={spawner.m_spawnInPlayerBase}");
                stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.SetPatrolPoint)}={spawner.m_setPatrolSpawnPoint}");
                //stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.RequireSpawnArea)}={spawner.m_requireSpawnArea}"); //Disabled for now, due to not seemingly being used.
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}
