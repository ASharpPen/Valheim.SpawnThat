using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using SpawnThat.Utilities;

namespace SpawnThat.Core.Toml;

internal static class TomlLoader
{
    private static readonly Regex SectionHeader = new Regex(@"^\s*[[](.+)[]]", RegexOptions.Compiled);
    private static readonly Regex SectionSanitizer = new Regex(@"[^\p{L}\d.\[\]_]|\s");

    private static readonly Regex SettingIdentifier = new Regex(@"^\s*(\p{L}+)\s*=(.*)");
    private static readonly Regex CommentIdentifier = new Regex(@"^\s*(//|#|--)", RegexOptions.Compiled);

    public static TConfig LoadFile<TConfig>(string path)
        where TConfig : TomlConfig, ITomlConfigFile
    {
        var lines = File.ReadAllLines(path);

        return Load<TConfig>(lines);
    }

    public static TConfig Load<TConfig>(string[] lines)
        where TConfig : TomlConfig, ITomlConfigFile
    {
        var mainConfig = Activator.CreateInstance<TConfig>();

        int currentLine = 0;
        string currentSection = null;
        List<string> currentSectionParts;
        TomlConfig currentSectionConfig = mainConfig;

        for (; currentLine < lines.Length; ++currentLine)
        {
            var line = lines[currentLine];

            // Skip empty lines.
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            // Check for comment
            if (CommentIdentifier.IsMatch(line))
            {
                continue;
            }

            // Check for header
            var sectionHeaderMatch = SectionHeader.Match(line);
            if (sectionHeaderMatch.Success)
            {
                var section = sectionHeaderMatch.Groups[1].Value;

                // Run cleanup and sanitizing
                var sanitized = SectionSanitizer.Replace(section, "");

                // Warn about potential issues in header
                if (string.IsNullOrWhiteSpace(sanitized))
                {
                    Log.LogWarning($"Line {currentLine}]: Section name '{section}' empty after sanitizing. Unable to load section.");
                    currentSection = null;
                    continue;
                }

                // Grab section and parts
                currentSection = sanitized;
                currentSectionParts = section.SplitBy(Separator.Dot);
                currentSectionConfig = FindSection(mainConfig, currentSectionParts);

                if (currentSectionConfig is null)
                {
                    // Warn about unknown section
                    Log.LogWarning($"Line {currentLine}]: Unable to find valid config for section '{sanitized}'.");
                }

                continue;
            }

            // Check for setting
            var settingMatch = SettingIdentifier.Match(line);
            if (settingMatch.Success)
            {
                var settingName = settingMatch.Groups[1].Value;
                var settingValue = settingMatch.Groups[2].Value;

                if (string.IsNullOrWhiteSpace(currentSection))
                {
                    // Log warning about settings outside section scope.
                    Log.LogWarning($"Line {currentLine}]: Setting '{settingName}' was not inside any section. Ignoring setting.");

                    continue;
                }
                else if (currentSectionConfig is null)
                {
                    Log.LogWarning($"Line {currentLine}]: Skipping setting '{settingName}' due to not finding valid config for section.");
                }

                // Look up config using section path.
                // Find matching entry in config
                if (currentSectionConfig.TryGet(settingName, out var configEntry))
                {
                    configEntry.Read(settingValue);
                }
                else
                {
                    // Log warning about unknown entry if no entry matches.
                    Log.LogWarning($"Line {currentLine}]: Setting '{settingName}' did not match any known setting for section '{currentSectionConfig.SectionPath}'. Ignoring setting.");
                }
            }

            // Log warning about unknown text line if we reached this far.
            Log.LogWarning($"Line {currentLine}]: Unknown text '{line}' did not match any known format. Ignoring setting.");
        }

        return mainConfig;
    }

    private static TomlConfig FindSection<T>(T configFile, List<string> sectionParts)
        where T : TomlConfig
    {
        TomlConfig config = configFile;

        foreach (var sectionName in sectionParts)
        {
            if (config is IHaveSubsections configWithSubsections)
            {
                config = configWithSubsections.GetSubsection(sectionName);
            }
            else
            {
                return null;
            }
        }

        return config;
    }
}
