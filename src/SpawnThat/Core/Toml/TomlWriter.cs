using System;
using System.Text;
using SpawnThat.Debugging;
using SpawnThat.Utilities;

namespace SpawnThat.Core.Toml;

internal static class TomlWriter
{
    public static void WriteToDisk(TomlConfig config, TomlWriterSettings settings)
    {
        var stringBuilder = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(settings.Header))
        {
            var headerLines = settings.Header.SplitBy(Separator.Newline);

            foreach (var headerLine in headerLines)
            {
                stringBuilder.AppendLine(headerLine);
            }

            stringBuilder.AppendLine();
        }

        // Add initial free-floating options, if any.
        // May need to reconsider this. Should probably always be inside a subsection.
        WriteOptions(stringBuilder, config, settings);

        if (config is IHaveSubsections configWithSubsections)
        {
            WriteSubsectionsRecursively(stringBuilder, configWithSubsections, settings);
        }

        DebugFileWriter.WriteFile(stringBuilder.ToString(), settings.FileName, settings.FileDescription);
    }

    private static void WriteSubsectionsRecursively(StringBuilder builder, IHaveSubsections config, TomlWriterSettings settings)
    {
        foreach (var subsection in config.GetSubsections())
        {
            builder.AppendLine($"[{subsection.Value.SectionPath}]");
            WriteOptions(builder, subsection.Value, settings);
            builder.AppendLine();

            if (subsection.Value is IHaveSubsections configWithSubsections)
            {
                WriteSubsectionsRecursively(builder, configWithSubsections, settings);
            }
        }
    }

    private static void WriteOptions(StringBuilder builder, TomlConfig config, TomlWriterSettings settings)
    {
        var entries = config.GetEntries();

        foreach (var entry in entries)
        {
            var entryName = entry.Key;
            var entryValue = entry.Value;

            // Skip entries that were never used.
            if (!entryValue.IsSet)
            {
                continue;
            }

            if (settings?.AddComments == true)
            {
                var descriptionLines = entryValue.Description.SplitBy(Separator.Newline);

                foreach (var descriptionLine in descriptionLines)
                {
                    builder.AppendLine("# " + descriptionLine);
                }
            }

            try
            {
                var value = TomlWriterFactory.Write(entryValue);

                builder.AppendLine($"{entryName} = {value}");
            }
            catch (Exception e)
            {
                Log.LogWarning($"Error while attempting to write setting '{entryName}' to file '{settings.FileName}'. Skipping setting.", e);
            }
        }
    }
}
