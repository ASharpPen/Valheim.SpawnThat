using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Spawners.LocalSpawner.Configuration.BepInEx;

namespace SpawnThat.Debugging;

internal static class CreatureSpawnerFileDumper
{
    private const string FileName = "local_spawners_pre_changes.txt";

    public static void WriteToFile(List<ZoneSystem.ZoneLocation> zoneLocations)
    {
        List<string> spawnersSerialized = new List<string>();

        var orderedLocations = zoneLocations
            .OrderBy(x => x.m_biome)
            .ThenBy(x => x.m_prefabName);

        foreach (var location in orderedLocations)
        {
            var locPrefab = location.m_prefab;

            if (locPrefab is null)
            {
                continue;
            }

            //Get location spawners
            var spawners = locPrefab.GetComponentsInChildren<CreatureSpawner>().ToList();

            //Get location dungeon generators, so we can scan for their spawners too
            var dungeons = locPrefab.GetComponentsInChildren<DungeonGenerator>();

            if (dungeons is not null && dungeons.Length > 0)
            {
                foreach (var dungeon in dungeons)
                {
                    //Find rooms and extract spawners
                    var rooms = DungeonDB.GetRooms().Where(x => (x.m_room.m_theme & dungeon.m_themes) == x.m_room.m_theme).ToList();

                    if (rooms.Count == 0)
                    {
                        Log.LogDebug($"No rooms for {locPrefab.name}:{dungeon.name}");
                    }


                    var roomSpawners = rooms
                        .SelectMany(x => x.m_room.GetComponentsInChildren<CreatureSpawner>())
                        .Where(x => x is not null)
                        .ToList();

                    if (roomSpawners.Count > 0)
                    {
                        if (spawners is null)
                        {
                            spawners = roomSpawners;
                        }
                        else
                        {
                            spawners.AddRange(roomSpawners);
                        }
                    }
                    else
                    {
                        Log.LogDebug($"No room spawners for {locPrefab.name}:{dungeon.name}");
                    }
                }
            }

            if (spawners is not null && spawners.Count > 0)
            {
                spawnersSerialized.Add(Serialize(location, spawners));
            }
        }

        DebugFileWriter.WriteFile(spawnersSerialized, FileName, "local spawner configurations");
    }

    private static string Serialize(ZoneSystem.ZoneLocation location, IList<CreatureSpawner> spawners)
    {
        if (!ConfigurationManager.GeneralConfig.DontCollapseFile.Value)
        {
            spawners = spawners.GroupBy(x => x.m_creaturePrefab.name).Select(x => x.First()).ToList();
        }

        var biome = location.m_biome;
        var locationName = location.m_prefabName;

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < spawners.Count; ++i)
        {
            var spawner = spawners[i];

            stringBuilder.AppendLine($"## Biome: {biome}, Location: {locationName}, Spawner: {i} ");

            try
            {
                stringBuilder.AppendLine($"[{locationName}.{spawner.m_creaturePrefab.name}]");
            }
            catch (Exception e)
            {
                Log.LogWarning($"Error while attempting to read name of prefab for spawner {spawner}", e);
            }

            stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.PrefabName)}={spawner.m_creaturePrefab.name}");
            stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.Enabled)}={spawner.enabled}");
            stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.SpawnAtDay)}={spawner.m_spawnAtDay}");
            stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.SpawnAtNight)}={spawner.m_spawnAtNight}");
            stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.LevelMin)}={spawner.m_minLevel}");
            stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.LevelMax)}={spawner.m_maxLevel}");
            stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.LevelUpChance)}={10}");
            stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.RespawnTime)}={spawner.m_respawnTimeMinuts.ToString(CultureInfo.InvariantCulture)}");
            stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.TriggerDistance)}={spawner.m_triggerDistance.ToString(CultureInfo.InvariantCulture)}");
            stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.TriggerNoise)}={spawner.m_triggerNoise.ToString(CultureInfo.InvariantCulture)}");
            stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.SpawnInPlayerBase)}={spawner.m_spawnInPlayerBase}");
            stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.SetPatrolPoint)}={spawner.m_setPatrolSpawnPoint}");

            var character = spawner.m_creaturePrefab.GetComponent<Character>();
            string factionName = "";

            if (character is not null)
            {
                factionName = character.m_faction.ToString();
            }

            stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.SetFaction)}={factionName}");
            //stringBuilder.AppendLine($"{nameof(CreatureSpawnerConfig.RequireSpawnArea)}={spawner.m_requireSpawnArea}"); //Disabled for now, due to not seemingly being used.
            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }
}
