using System.Collections.Generic;

namespace SpawnThat.Core.Toml;

internal abstract class TomlConfigWithSubsections<T> : TomlConfig, IHaveSubsections
    where T : TomlConfig
{
    public Dictionary<string, T> Subsections { get; set; } = new Dictionary<string, T>();

    public TomlConfig GetSubsection(string subsectionName)
    {
        var cleanedName = subsectionName.Trim().ToUpperInvariant();

        if (Subsections.TryGetValue(cleanedName, out T existingItem))
        {
            return existingItem;
        }
        else
        {
            var newItem = InstantiateSubsection(cleanedName);
            Subsections[cleanedName] = newItem;

            return newItem;
        }
    }

    public bool TryGet(string key, out T value)
    {
        return Subsections.TryGetValue(key.Trim().ToUpperInvariant(), out value);
    }

    protected abstract T InstantiateSubsection(string subsectionName);
}
