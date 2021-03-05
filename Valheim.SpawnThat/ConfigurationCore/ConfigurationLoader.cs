using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Valheim.SpawnThat.ConfigurationCore
{
    public static class ConfigurationLoader
    {
        private static readonly Regex SectionHeader = new Regex(@"(?<=[[]).+(?=[]])", RegexOptions.Compiled);

        public static Dictionary<string, TGroup> LoadConfigurationGroup<TGroup, TSection>(ConfigFile configFile)
            where TGroup : ConfigurationGroup<TSection>
            where TSection : ConfigurationSection
        {
            if (string.IsNullOrEmpty(configFile.ConfigFilePath))
            {
                Log.LogError("Unable to load unloaded ConfigFile.");
                return new Dictionary<string, TGroup>();
            }

            //Scan for headers
            var sectionHeaders = ScanSectionHeads(configFile.ConfigFilePath);

            Dictionary<string, TGroup> configurations = new Dictionary<string, TGroup>();

            foreach(var sectionHead in sectionHeaders)
            {
                Log.LogTrace($"Loading config entries for {sectionHead}.");

                //Identify header type
                var components = sectionHead.Split('.');

                string groupName = components[0];
                string elementKey = components.Length > 1
                    ? components[1]
                    : null;

                TGroup group = GetOrInitializeGroup<TGroup, TSection>(configurations, groupName);
                
                if (string.IsNullOrEmpty(elementKey))
                {
                    //Assume that this is a group definition. Load in group entries.
                    BindObjectEntries(configFile, group, groupName);
                }
                else
                {
                    TSection section = GetOrInitializeSection(group.Sections, elementKey);

                    //Start loading in the configuration key:values of the section
                    BindObjectEntries(configFile, section, sectionHead);
                }
            }

            return configurations;
        }

        private static void BindObjectEntries(ConfigFile configFile, IHaveEntries configuration, string sectionHeader)
        {
            Type entryType = typeof(IConfigurationEntry);

            var fields = configuration
                    .GetType()
                    .GetFields()
                    .Where(x => entryType.IsAssignableFrom(x.FieldType))
                    .ToList();

            foreach (var field in fields)
            {
                Log.LogTrace($"Creating and binding entry for '{sectionHeader}:{field.Name}'");

                var entry = (IConfigurationEntry)field.GetValue(configuration);

                if (entry is null)
                {
                    entry = (IConfigurationEntry)Activator.CreateInstance(field.FieldType);
                }

                entry.Bind(configFile, sectionHeader, field.Name);

                configuration.Entries[field.Name] = entry;
                field.SetValue(configuration, entry);

                Log.LogTrace($"[{sectionHeader}]: Loaded [{field.Name}:{entry}]");
            }
        }

        private static List<string> ScanSectionHeads(string configFile)
        {
            if (!File.Exists(configFile))
            {
                return new List<string>();
            }

            Log.LogTrace($"Scanning config sections in {configFile}");

            //Scan for headers
            var lines = File.ReadAllLines(configFile);

            var sectionHeaders = new List<string>(lines.Count() / 5); //Just some random guess at lines pr header.

            foreach (var line in lines)
            {
                var sectionMatch = SectionHeader.Match(line);

                if (sectionMatch.Success)
                {
                    var sectionHeader = sectionMatch.Value;

                    Log.LogTrace($"Found section '{sectionHeader}'");

                    sectionHeaders.Add(sectionHeader);
                }
            }

            return sectionHeaders;
        }

        private static TGroup GetOrInitializeGroup<TGroup, TSection>(Dictionary<string, TGroup> groups, string groupName)
            where TGroup : ConfigurationGroup<TSection>
            where TSection : ConfigurationSection
        {
            TGroup group;

            if (!groups.ContainsKey(groupName))
            {
                groups.Add(groupName, group = Activator.CreateInstance<TGroup>());
                group.GroupName = groupName;
                group.Sections = new Dictionary<string, TSection>();
                group.Entries = new Dictionary<string, IConfigurationEntry>();
            }
            else
            {
                group = groups[groupName];
            }

            return group;
        }

        private static TSection GetOrInitializeSection<TSection>(Dictionary<string, TSection> sections, string sectionKey)
            where TSection : ConfigurationSection
        {
            TSection section;

            if (!sections.ContainsKey(sectionKey))
            {
                sections.Add(sectionKey, section = Activator.CreateInstance<TSection>());
                section.SectionName = sectionKey;
                section.Entries = new Dictionary<string, IConfigurationEntry>();
            }
            else
            {
                section = sections[sectionKey];
            }

            return section;
        }
    }
}
