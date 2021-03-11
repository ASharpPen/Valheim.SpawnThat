using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Valheim.SpawnThat.ConfigurationTypes;

namespace Valheim.SpawnThat.Debugging
{
    public static class SpawnDataFileDumper
    {
        public static void WriteToFile(List<SpawnSystem.SpawnData> spawners, string fileName)
        {
            string filePath = Path.Combine(Paths.PluginPath, fileName);

            List<string> lines = new List<string>(spawners.Count * 30);

            for (int i = 0; i < spawners.Count; ++i)
            {
                var spawner = spawners[i];

                lines.AddRange(WriteSpawner(spawner, i));
            }

            File.WriteAllLines(filePath, lines);
        }

        private static void Scan(SpawnSystem.SpawnData spawner, List<string> results, int depth = 1)
        {
            var fields = spawner.GetType().GetFields();

            string indent = "";
            for (int i = 0; i < depth; ++i)
            {
                indent += "\t";
            }

            foreach (var field in fields)
            {
                //Special cases
                if(typeof(GameObject).IsAssignableFrom(field.FieldType))
                {
                    results.Add($"{indent}{field.Name}: {((GameObject)field.GetValue(spawner))?.name}");
                }
                else if(typeof(Heightmap.Biome) == field.FieldType)
                {
                    results.Add($"{indent}{field.Name}: {(int)field.GetValue(spawner)}");

                    var spawnerBiome = spawner.m_biome;

                    var indent2 = indent + "\t";

                    foreach (var b in Enum.GetValues(typeof(Heightmap.Biome)))
                    {
                        if(b is Heightmap.Biome biome && biome != Heightmap.Biome.BiomesMax)
                        {
                            biome = (biome & spawnerBiome);

                            if (biome > Heightmap.Biome.None)
                            {
                                results.Add($"{indent2}{biome}");
                            }
                        }
                    }
                }
                else if (typeof(List<string>).IsAssignableFrom(field.FieldType))
                {
                    results.Add($"{indent}{field.Name}:");

                    var indent2 = indent + "\t";
                    foreach (var str in field.GetValue(spawner) as List<string>)
                    {
                        results.Add($"{indent2}{str}");
                    }
                }
                else
                {
                    results.Add($"{indent}{field.Name}: {field.GetValue(spawner)}");
                }
            }
        }

        internal static List<string> WriteSpawner(SpawnSystem.SpawnData spawner, int index)
        {
            List<string> lines = new List<string>();

            //Write header
            lines.Add($"[WorldSpawner.{index}]");

            //Write lines
            lines.Add($"{nameof(SpawnConfiguration.Name)}={spawner.m_name}");
            lines.Add($"{nameof(SpawnConfiguration.Enabled)}={spawner.m_enabled}");
            lines.Add($"{nameof(SpawnConfiguration.Biomes)}={BiomeArray(spawner.m_biome)}");
            lines.Add($"{nameof(SpawnConfiguration.PrefabName)}={spawner.m_prefab.name}");
            lines.Add($"{nameof(SpawnConfiguration.HuntPlayer)}={spawner.m_huntPlayer}");
            lines.Add($"{nameof(SpawnConfiguration.MaxSpawned)}={spawner.m_maxSpawned}");
            lines.Add($"{nameof(SpawnConfiguration.SpawnInternval)}={spawner.m_spawnInterval}");
            lines.Add($"{nameof(SpawnConfiguration.SpawnChance)}={spawner.m_spawnChance}");
            lines.Add($"{nameof(SpawnConfiguration.LevelMin)}={spawner.m_minLevel}");
            lines.Add($"{nameof(SpawnConfiguration.LevelMax)}={spawner.m_maxLevel}");
            lines.Add($"{nameof(SpawnConfiguration.LevelUpMinCenterDistance)}={spawner.m_levelUpMinCenterDistance}");
            lines.Add($"{nameof(SpawnConfiguration.SpawnDistance)}={spawner.m_spawnDistance}");
            lines.Add($"{nameof(SpawnConfiguration.SpawnRadiusMin)}={spawner.m_spawnRadiusMin}");
            lines.Add($"{nameof(SpawnConfiguration.SpawnRadiusMax)}={spawner.m_spawnRadiusMax}");
            lines.Add($"{nameof(SpawnConfiguration.RequiredGlobalKey)}={spawner.m_requiredGlobalKey}");
            lines.Add($"{nameof(SpawnConfiguration.RequiredEnvironments)}={spawner.m_requiredEnvironments}");
            lines.Add($"{nameof(SpawnConfiguration.GroupSizeMin)}={spawner.m_groupSizeMin}");
            lines.Add($"{nameof(SpawnConfiguration.GroupSizeMax)}={spawner.m_groupSizeMax}");
            lines.Add($"{nameof(SpawnConfiguration.GroupRadius)}={spawner.m_groupRadius}");
            lines.Add($"{nameof(SpawnConfiguration.GroundOffset)}={spawner.m_groundOffset}");
            lines.Add($"{nameof(SpawnConfiguration.SpawnDuringDay)}={spawner.m_spawnAtDay}");
            lines.Add($"{nameof(SpawnConfiguration.SpawnDuringNight)}={spawner.m_spawnAtNight}");
            lines.Add($"{nameof(SpawnConfiguration.ConditionAltitudeMin)}={spawner.m_minAltitude}");
            lines.Add($"{nameof(SpawnConfiguration.ConditionAltitudeMax)}={spawner.m_maxAltitude}");
            lines.Add($"{nameof(SpawnConfiguration.ConditionTiltMin)}={spawner.m_minTilt}");
            lines.Add($"{nameof(SpawnConfiguration.ConditionTiltMax)}={spawner.m_maxTilt}");
            lines.Add($"{nameof(SpawnConfiguration.SpawnInForest)}={spawner.m_inForest}");
            lines.Add($"{nameof(SpawnConfiguration.SpawnOutsideForest)}={spawner.m_outsideForest}");
            lines.Add($"{nameof(SpawnConfiguration.OceanDepthMin)}={spawner.m_minOceanDepth}");
            lines.Add($"{nameof(SpawnConfiguration.OceanDepthMax)}={spawner.m_maxOceanDepth}");
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
}
