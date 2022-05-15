using System;
using System.Collections.Generic;
using System.Linq;

namespace SpawnThat.Core.Toml;

internal abstract class TomlConfig
{
    private Dictionary<string, ITomlConfigEntry> EntryFields { get; } = new();

    /// <summary>
    /// Full path of config section.
    /// </summary>
    public string SectionPath { get; set; }

    /// <summary>
    /// Config section name.
    /// Last part of section path.
    /// </summary>
    public string SectionName { get; set; }

    public TomlConfig()
    {
        Type entryType = typeof(ITomlConfigEntry);

        var fields = this
                .GetType()
                .GetFields()
                .Where(x => entryType.IsAssignableFrom(x.FieldType))
                .ToList();

        foreach (var field in fields)
        {
            var configEntry = (ITomlConfigEntry)field.GetValue(this);

            EntryFields[configEntry.Name] = configEntry;
        }
    }

    public bool TryGet(string entryName, out ITomlConfigEntry entry) 
        => EntryFields.TryGetValue(entryName, out entry);
}
