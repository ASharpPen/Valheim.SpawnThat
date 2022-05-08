using System;
using System.Collections.Generic;
using SpawnThat.Core.Toml.Parsers;
using System.Linq;

namespace SpawnThat.Core.Toml;

internal static class TomlParserFactory
{
    private static Dictionary<Type, IValueParser> parsers = new();

    static TomlParserFactory()
    {
        parsers = new Dictionary<Type, IValueParser>
        {
            { typeof(bool?), new BoolParser() },
            { typeof(double?), new DoubleParser() },
            { typeof(float?), new FloatParser() },
            { typeof(int?), new IntParser() },
            { typeof(string), new StringParser() },
            { typeof(List<string>), new StringListParser() },
        };
    }

    public static void Read<T>(this T entry, string value)
        where T : ITomlConfigEntry
    {
        if (parsers.TryGetValue(entry.SettingType, out var parser))
        {
            parser.Parse(value, entry);
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}
