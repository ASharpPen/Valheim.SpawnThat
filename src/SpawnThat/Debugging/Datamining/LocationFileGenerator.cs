﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Debugging.Datamining;

[HarmonyPatch(typeof(ZoneSystem))]
internal static class LocationFileGenerator
{
    private const string FileName = "spawn_that.locations.txt";

    [HarmonyPatch(nameof(ZoneSystem.Start))]
    [HarmonyPostfix]
    private static void PrintLocations(ZoneSystem __instance)
    {
        if (__instance.IsNull() ||
            __instance.m_locations is null)
        {
            return;
        }

        if (ConfigurationManager.GeneralConfig?.WriteLocationsToFile.Value == true)
        {
            try
            {
                WriteToList(__instance.m_locations);
            }
            catch (Exception e)
            {
                Log.LogWarning("Error while attempting to datamine locations and write them to file. Skipping.", e);
            }
        }
    }

    private static void WriteToList(List<ZoneSystem.ZoneLocation> zoneLocations)
    {
        HashSet<string> printedLocations = new();
        StringBuilder stringBuilder = new();

        // Add header
        stringBuilder.AppendLine($"# This file was auto-generated by Spawn That {SpawnThatPlugin.Version} at {DateTimeOffset.UtcNow.ToString("u")}, with Valheim '{Version.CurrentVersion.m_major}.{Version.CurrentVersion.m_minor}.{Version.CurrentVersion.m_patch}'.");
        stringBuilder.AppendLine($"# This file lists all prefab names of locations loaded, by the biome in which they can appear.");
        stringBuilder.AppendLine();

        Dictionary<Heightmap.Biome, List<string>> locationsByBiome = new();

        foreach (var location in zoneLocations)
        {
            var biomes = location.m_biome.Split();

            foreach (var biome in biomes)
            {
                List<string> biomeLocations;

                if (!locationsByBiome.TryGetValue(biome, out biomeLocations))
                {
                    locationsByBiome[biome] = biomeLocations = new();
                }

                biomeLocations.Add(location.m_prefabName);
            }
        }

        foreach (var biome in locationsByBiome.OrderBy(x => x.Key))
        {
            var currentBiome = biome.Key.ToString();

            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"[{currentBiome}]");

            foreach (var location in biome.Value)
            {
                var locationKey = location + "." + currentBiome;

                if (!printedLocations.Contains(locationKey))
                {
                    stringBuilder.AppendLine(location);
                    printedLocations.Add(locationKey);
                }
            }
        }

        DebugFileWriter.WriteFile(stringBuilder.ToString(), FileName, "datamined locations by biome");
    }
}