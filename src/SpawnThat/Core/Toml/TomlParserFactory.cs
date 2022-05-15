using System;
using System.Collections.Generic;
using System.Linq;
using SpawnThat.Core.Toml.Parsers;

namespace SpawnThat.Core.Toml;

internal static class TomlParserFactory
{
    private static Dictionary<Type, IValueParser> parsers = new();

    private static Type ListType = typeof(List<>);
    private static Type EnumListParserType = typeof(EnumListParser<>);
    private static Type EnumParserType = typeof(EnumParser<>);
    private static Type NullableEnumParserType = typeof(NullableEnumParser<>);

    static TomlParserFactory()
    {
        parsers = new Dictionary<Type, IValueParser>
        {
            { typeof(bool), new BoolParser() },
            { typeof(double), new DoubleParser() },
            { typeof(float), new FloatParser() },
            { typeof(int), new IntParser() },
            { typeof(bool?), new NullableBoolParser() },
            { typeof(double?), new NullableDoubleParser() },
            { typeof(float?), new NullableFloatParser() },
            { typeof(int?), new NullableIntParser() },
            { typeof(string), new StringParser() },
            { typeof(List<string>), new StringListParser() },
            { typeof(List<int>), new IntListParser() },
        };
    }

    public static void Read<T>(this T entry, TomlLine line)
        where T : ITomlConfigEntry
    {
        IValueParser parser;

        if (parsers.TryGetValue(entry.SettingType, out parser))
        {
            parser.Parse(entry, line);
        }
        // Handle enum lists
        else if (
            entry.SettingType.IsGenericType &&
            entry.SettingType.GetGenericTypeDefinition() == ListType)
        {
            // Check if generic type of the list is an enum
            if (entry.SettingType.GenericTypeArguments.First().IsEnum)
            {
                var genericType = entry.SettingType.GenericTypeArguments.First();

                var constructedType = EnumListParserType.MakeGenericType(genericType);

                parser = (IValueParser)Activator.CreateInstance(constructedType);
                parsers[entry.SettingType] = parser;

                parser.Parse(entry, line);
            }
            else
            {
                throw new NotSupportedException($"Unable to parse config entries of type '{entry.SettingType.Name}'.");
            }
        }
        // Handle enums
        else if (entry.SettingType.IsEnum)
        {
            // Try generate enum parser for type.
            var constructedType = EnumParserType.MakeGenericType(entry.SettingType);

            parser = (IValueParser)Activator.CreateInstance(constructedType);
            parsers[entry.SettingType] = parser;

            parser.Parse(entry, line);
        }
        // Handle nullable enums
        else if(Nullable.GetUnderlyingType(entry.SettingType)?.IsEnum == true)
        {
            // Try generate enum parser for type.
            var underlyingType = Nullable.GetUnderlyingType(entry.SettingType);
            var constructedType = NullableEnumParserType.MakeGenericType(underlyingType);

            parser = (IValueParser)Activator.CreateInstance(constructedType);
            parsers[entry.SettingType] = parser;

            parser.Parse(entry, line);
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}
