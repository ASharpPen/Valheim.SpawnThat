using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;

namespace Valheim.SpawnThat.Debugging;

internal static class SpawnDataFileDumper
{
    public static void WriteToFile(List<SpawnSystem.SpawnData> spawners, string fileName, bool postChange = false)
    {
        if (spawners is null)
        {
            return;
        }

        string filePath = Path.Combine(Paths.PluginPath, fileName);

        List<string> lines = new List<string>(spawners.Count * 30);

        for (int i = 0; i < spawners.Count; ++i)
        {
            var spawner = spawners[i];

            if (spawner is not null)
            {
                lines.AddRange(WriteSpawner(spawner, i, postChange));
            }
            else
            {
                //Empty spawner. Just add the index and continue.
                lines.Add($"[WorldSpawner.{i}]");
                lines.Add($"## Spawner is empty for unknown reasons.");
                lines.Add($"");
            }
        }

        DebugFileWriter.WriteFile(lines, fileName, "world spawner configurations");
    }

    internal static List<string> WriteSpawner(SpawnSystem.SpawnData spawner, int index, bool postChange)
    {
        List<string> lines = new List<string>();

        //Write header
        lines.Add($"[WorldSpawner.{index}]");

        string environmentArray = "";
        try
        {
            if ((spawner.m_requiredEnvironments?.Count ?? 0) > 0)
            {
                environmentArray = spawner.m_requiredEnvironments.Join();
            }
        }
        catch (Exception e)
        {
            Log.LogWarning($"Error while attempting to read required environments of spawner {spawner}");
        }

        //Write lines
        lines.Add($"{nameof(SpawnConfiguration.Name)}={spawner.m_name}");
        lines.Add($"{nameof(SpawnConfiguration.Enabled)}={spawner.m_enabled}");
        try
        {
            lines.Add($"{nameof(SpawnConfiguration.Biomes)}={BiomeArray(spawner.m_biome)}");
        }
        catch (Exception e)
        {
            Log.LogWarning($"Failed to read biome of {spawner}");
        }

        try
        {
            lines.Add($"{nameof(SpawnConfiguration.PrefabName)}={spawner.m_prefab.name}");
        }
        catch (Exception e)
        {
            Log.LogWarning($"Error while attempting to read name of prefab for spawner {spawner}");
        }

        lines.Add($"{nameof(SpawnConfiguration.HuntPlayer)}={spawner.m_huntPlayer}");
        lines.Add($"{nameof(SpawnConfiguration.MaxSpawned)}={spawner.m_maxSpawned}");
        lines.Add($"{nameof(SpawnConfiguration.SpawnInterval)}={spawner.m_spawnInterval.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"{nameof(SpawnConfiguration.SpawnChance)}={spawner.m_spawnChance.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"{nameof(SpawnConfiguration.LevelMin)}={spawner.m_minLevel}");
        lines.Add($"{nameof(SpawnConfiguration.LevelMax)}={spawner.m_maxLevel}");
        lines.Add($"{nameof(SpawnConfiguration.LevelUpMinCenterDistance)}={spawner.m_levelUpMinCenterDistance.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"{nameof(SpawnConfiguration.SpawnDistance)}={spawner.m_spawnDistance.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"{nameof(SpawnConfiguration.SpawnRadiusMin)}={spawner.m_spawnRadiusMin.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"{nameof(SpawnConfiguration.SpawnRadiusMax)}={spawner.m_spawnRadiusMax.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"{nameof(SpawnConfiguration.RequiredGlobalKey)}={spawner.m_requiredGlobalKey}");
        lines.Add($"{nameof(SpawnConfiguration.RequiredEnvironments)}={environmentArray}");
        lines.Add($"{nameof(SpawnConfiguration.GroupSizeMin)}={spawner.m_groupSizeMin}");
        lines.Add($"{nameof(SpawnConfiguration.GroupSizeMax)}={spawner.m_groupSizeMax}");
        lines.Add($"{nameof(SpawnConfiguration.GroupRadius)}={spawner.m_groupRadius.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"{nameof(SpawnConfiguration.GroundOffset)}={spawner.m_groundOffset.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"{nameof(SpawnConfiguration.SpawnDuringDay)}={spawner.m_spawnAtDay}");
        lines.Add($"{nameof(SpawnConfiguration.SpawnDuringNight)}={spawner.m_spawnAtNight}");
        lines.Add($"{nameof(SpawnConfiguration.ConditionAltitudeMin)}={spawner.m_minAltitude.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"{nameof(SpawnConfiguration.ConditionAltitudeMax)}={spawner.m_maxAltitude.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"{nameof(SpawnConfiguration.ConditionTiltMin)}={spawner.m_minTilt.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"{nameof(SpawnConfiguration.ConditionTiltMax)}={spawner.m_maxTilt.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"{nameof(SpawnConfiguration.SpawnInForest)}={spawner.m_inForest}");
        lines.Add($"{nameof(SpawnConfiguration.SpawnOutsideForest)}={spawner.m_outsideForest}");
        lines.Add($"{nameof(SpawnConfiguration.OceanDepthMin)}={spawner.m_minOceanDepth.ToString(CultureInfo.InvariantCulture)}");
        lines.Add($"{nameof(SpawnConfiguration.OceanDepthMax)}={spawner.m_maxOceanDepth.ToString(CultureInfo.InvariantCulture)}");

        try
        {
            if (!postChange)
            {

                var character = spawner.m_prefab?.GetComponent<Character>();
                string factionName = "";

                if (character && character is not null)
                {
                    factionName = character.m_faction.ToString();
                }

                lines.Add($"{nameof(SpawnConfiguration.SetFaction)}={factionName}");
            }
        }
        catch (Exception e)
        {
            Log.LogWarning($"Error while attempting to write faction of spawner {spawner}");
        }

        lines.Add("");

        return lines;
    }

    private static string BiomeArray(Heightmap.Biome spawnerBiome)
    {
        string biomeArray = "";

        foreach (var b in Enum.GetValues(typeof(Heightmap.Biome)))
        {
            if (b is Heightmap.Biome biome && biome != Heightmap.Biome.BiomesMax)
            {
                biome = (biome & spawnerBiome);

                if (biome > Heightmap.Biome.None)
                {
                    biomeArray += biome + ",";
                }
            }
        }

        return biomeArray;
    }
}
