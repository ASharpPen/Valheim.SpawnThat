using System.Collections.Generic;
using System.Linq;

namespace SpawnThat.Core.Toml;

internal abstract class TomlConfigWithSubsections<T> : TomlConfig, IHaveSubsections
    where T : TomlConfig
{
    public Dictionary<string, T> Subsections { get; set; } = new Dictionary<string, T>();

    public TomlConfig GetSubsection(string subsectionName) 
        => GetGenericSubsection(subsectionName);

    public bool TryGet(string key, out T value)
    {
        return Subsections.TryGetValue(key.Trim().ToUpperInvariant(), out value);
    }

    protected abstract T InstantiateSubsection(string subsectionName);

    public T GetGenericSubsection(string subsectionName)
    {
        var cleanedName = subsectionName.Trim().ToUpperInvariant();

        if (Subsections.TryGetValue(cleanedName, out T existingItem))
        {
            return existingItem;
        }
        else
        {
            var newItem = InstantiateSubsection(cleanedName);
            newItem.SectionName = cleanedName;

            newItem.SectionPath = string.IsNullOrEmpty(SectionPath)
                ? newItem.SectionName
                : SectionPath + "." + newItem.SectionName;

            Subsections[cleanedName] = newItem;

            return newItem;
        }
    }

    public List<KeyValuePair<string, TomlConfig>> GetSubsections()
        => Subsections
            .Select(x => new KeyValuePair<string, TomlConfig>(x.Key, x.Value as TomlConfig))
            .ToList();
}
