//#define VERBOSE

using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpawnThat.Core.Configuration
{
    public static class ConfigurationLoader
    {
        private static readonly Regex SectionHeader = new Regex(@"(?<=[[]).+(?=[]])", RegexOptions.Compiled);
        private static readonly Regex SectionSanitizer = new Regex(@"[^\p{L}\d.\[\]_\s]");
        private static readonly Regex SectionWhiteSpaceTrim = new Regex(@"\s+(?=\])");

        /// <summary>
        /// Attempts to clean up section headers in file.
        /// </summary>
        public static void SanitizeSectionHeaders(string filePath)
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);

                for (int i = 0; i < lines.Length; ++i)
                {
                    string line = lines[i];

                    if (SectionHeader.IsMatch(line))
                    {
                        var sanitized = SectionSanitizer.Replace(line, "");
                        sanitized = SectionWhiteSpaceTrim.Replace(sanitized, "");
                        lines[i] = sanitized;
                    }
                }

                File.WriteAllLines(filePath, lines);
            }
        }

        public static TConfig LoadConfiguration<TConfig>(ConfigFile configFile) where TConfig : Config, IConfigFile
        {
            var mainConfig = Activator.CreateInstance<TConfig>();

            if (string.IsNullOrEmpty(configFile.ConfigFilePath))
            {
                Log.LogError("Unable to load unloaded ConfigFile.");
                return mainConfig;
            }

            //Scan for headers
            var sectionHeaders = ScanSectionHeads(configFile.ConfigFilePath);

            foreach (var sectionHead in sectionHeaders)
            {
#if VERBOSE
                Log.LogTrace($"Loading config entries for '{sectionHead}'.");
#endif

                //Identify header components
                var components = sectionHead.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                if (components.Length < 1 || string.IsNullOrEmpty(components[0]))
                {
#if DEBUG && VERBOSE
                    Log.LogTrace("Early stop. 0 components found.");
#endif

                    continue;
                }

                LoadRecursively(configFile, mainConfig, components, components[0], 0);
            }

            return mainConfig;
        }

        private static void LoadRecursively<T>(ConfigFile configFile, T currentConfig, IList<string> sectionParts, string sectionKey, int depth) where T : Config
        {
            currentConfig.SectionName = sectionParts[depth];
            currentConfig.SectionKey = sectionKey;

            //Check if we have reached the end
            if(depth == sectionParts.Count - 1 && currentConfig is not IConfigFile)
            {
#if DEBUG && VERBOSE
                Log.LogTrace($"Binding entries for {sectionKey}:{currentConfig.GetType().Name}");
#endif
                BindObjectEntries(configFile, currentConfig, sectionKey);
                return;
            }

            //Check if we are at the top level. If so, don't increment subsections.
            if (currentConfig is IConfigFile topConfig)
            {
                var subsection = topConfig.GetSubsection(sectionKey);

                LoadRecursively(configFile, subsection, sectionParts, sectionKey, depth);
            }
            //Go deeper into tree.
            else if (currentConfig is IHaveSubsections parent)
            {
                var subsection = parent.GetSubsection(sectionParts[depth + 1]);
                var subsectionKey = sectionKey + "." + sectionParts[depth + 1];

                LoadRecursively(configFile, subsection, sectionParts, subsectionKey, depth + 1);
            }
        }

        private static void BindObjectEntries<T>(ConfigFile configFile, T config, string sectionHeader) where T : Config
        {
            Type entryType = typeof(IConfigurationEntry);

            var fields = config
                    .GetType()
                    .GetFields()
                    .Where(x => entryType.IsAssignableFrom(x.FieldType))
                    .ToList();

            foreach (var field in fields)
            {
#if VERBOSE
                Log.LogTrace($"Creating and binding entry for '{sectionHeader}:{field.Name}'");
#endif

                var entry = field.GetValue(config) as IConfigurationEntry;

                if (entry is null)
                {
                    entry = (IConfigurationEntry)Activator.CreateInstance(field.FieldType);
                    field.SetValue(config, entry);
                }

                entry.Bind(configFile, sectionHeader, field.Name);
#if VERBOSE
                Log.LogTrace($"[{sectionHeader}]: Loaded {field.Name}:{entry}");
#endif
            }
        }

        private static List<string> ScanSectionHeads(string configFile)
        {
            if (!File.Exists(configFile))
            {
                return new List<string>();
            }
#if VERBOSE
            Log.LogTrace($"Scanning config sections in {configFile}");
#endif
            //Scan for headers
            var lines = File.ReadAllLines(configFile);

            var sectionHeaders = new List<string>(lines.Count() / 5); //Just some random guess at lines pr header.

            foreach (var line in lines)
            {
                var sectionMatch = SectionHeader.Match(line);

                if (sectionMatch.Success)
                {
                    var sectionHeader = sectionMatch.Value;
#if VERBOSE
                    Log.LogTrace($"Found section '{sectionHeader}'");
#endif
                    sectionHeaders.Add(sectionHeader);
                }
            }

            return sectionHeaders;
        }
    }
}
