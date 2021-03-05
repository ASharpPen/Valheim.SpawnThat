using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.ConfigurationCore;

namespace Valheim.CustomRaids
{

    //[HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.Load))]
    [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.GenerateLocations), new Type[] { })]
    public static class InvestigateZoneSpawnersPatch
    {
        private static void Postfix(ref ZoneSystem __instance)
        {
            Log.LogInfo("Did I get it?");

            var locations = __instance.m_locations;

            WriteToFile(locations, true, @".\zonesystem_zonelocations.txt");
        }

        public static void WriteToFile(List<ZoneSystem.ZoneLocation> locations, bool debug, string fileName = "default_random_events.txt")
        {
            string filePath = Path.Combine(Paths.PluginPath, fileName);
            if (debug) Debug.Log($"Writing default zones to '{filePath}'.");

            List<string> lines = new List<string>(locations.Count * 30);

            foreach (var item in locations)
            {
                lines.Add("");
                lines.Add("[ZoneLocation]");

                Scan(item, lines);

                lines.Add("");
            }
            File.WriteAllLines(filePath, lines);
        }

        private static void Scan(object obj, List<string> results, int depth = 1)
        {
            var fields = obj.GetType().GetFields();

            string indent = "";
            for (int i = 0; i < depth; ++i)
            {
                indent += "\t";
            }

            foreach (var field in fields)
            {
                if (typeof(List<string>).IsAssignableFrom(field.FieldType))
                {
                    results.Add($"{indent}{field.Name}:");

                    var indent2 = indent + "\t";
                    foreach (var str in field.GetValue(obj) as List<string>)
                    {
                        results.Add($"{indent2}{str}");
                    }
                }
                else if (typeof(List<RandomSpawn>).IsAssignableFrom(field.FieldType))
                {
                    var list = ((List<RandomSpawn>)field.GetValue(obj));
                    results.Add($"{indent}{field.Name}:{list?.Count.ToString() ?? "null"}");
                    foreach (var spawn in list)
                    {
                        Scan(spawn, results, depth + 1);
                    }
                }
                else
                {
                    results.Add($"{indent}{field.Name}: {field.GetValue(obj)}");
                }
            }
        }
    }

    [HarmonyPatch(typeof(ZoneSystem), "Start")]
    public static class ZoneSystemAwake
    {
        private static void Postfix(ref ZoneSystem __instance)
        {
            Log.LogInfo("Wait, I can just do this can't I?");

            InvestigateZoneSpawnersPatch.WriteToFile(__instance.m_locations, true, @".\zonesystem_start_zonelocations.txt");
        }
    }

    [HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
    public static class StartupPatch
    {
        private static void Postfix(ref ZNet __instance)
        {
            Log.LogInfo("World initiated.");

            var locations = ZoneSystem.instance.m_locations;

            InvestigateZoneSpawnersPatch.WriteToFile(locations, true, @".\rpc_peerinfo_zonesystem_zonelocations.txt");
        }
    }

    //[HarmonyPatch(typeof(SpawnSystem), "UpdateSpawning")]
    public static class SpawnSystemPatch
    {
        private static void Postfix(ref SpawnSystem __instance)
        {
            Log.LogDebug($"And whats going on here mister {__instance.name}");

            WriteToFile(__instance.m_spawners, true, @".\spawnsystem_spawners.txt");
        }

        public static void WriteToFile(List<SpawnSystem.SpawnData> spawners, bool debug, string fileName = "default_random_events.txt")
        {
            string filePath = Path.Combine(Paths.PluginPath, fileName);
            if (debug) Debug.Log($"Writing default random events to '{filePath}'.");

            List<string> lines = new List<string>(spawners.Count * 30);

            foreach (var item in spawners)
            {
                lines.Add("");
                lines.Add("[Spawner]");

                Scan(item, lines);

                lines.Add("");
            }
            File.WriteAllLines(filePath, lines);
        }

        private static void Scan(object obj, List<string> results, int depth = 1)
        {
            var fields = obj.GetType().GetFields();

            string indent = "";
            for (int i = 0; i < depth; ++i)
            {
                indent += "\t";
            }

            foreach (var field in fields)
            {
                if (typeof(List<string>).IsAssignableFrom(field.FieldType))
                {
                    results.Add($"{indent}{field.Name}:");

                    var indent2 = indent + "\t";
                    foreach (var str in field.GetValue(obj) as List<string>)
                    {
                        results.Add($"{indent2}{str}");
                    }
                }
                else
                {
                    results.Add($"{indent}{field.Name}: {field.GetValue(obj)}");
                }
            }
        }
    }
}


