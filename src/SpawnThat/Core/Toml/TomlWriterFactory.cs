using System;
using System.Collections.Generic;
using System.Linq;
using SpawnThat.Core.Toml.Writers;

namespace SpawnThat.Core.Toml;

internal static class TomlWriterFactory
{
    private static Dictionary<Type, IValueWriter> writers = new();

    private static Type ListType = typeof(List<>);
    private static Type EnumListWriterType = typeof(EnumListWriter<>);
    private static Type EnumWriterType = typeof(EnumWriter<>);
    private static Type NullableEnumWriterType = typeof(NullableEnumWriter<>);


    static TomlWriterFactory()
    {
        writers = new Dictionary<Type, IValueWriter>()
        {
            { typeof(bool), new BoolWriter() },
            { typeof(bool?), new NullableBoolWriter() },
            { typeof(double), new DoubleWriter() },
            { typeof(double?), new NullableDoubleWriter() },
            { typeof(float), new FloatWriter() },
            { typeof(float?), new NullableFloatWriter() },
            { typeof(int), new IntWriter() },
            { typeof(int?), new NullableIntWriter() },
            { typeof(string), new StringWriter() },
            { typeof(List<int>), new IntListWriter() },
            { typeof(List<string>), new StringListWriter() },
        };
    }

    public static string Write<T>(this T entry)
        where T : ITomlConfigEntry
    {
        IValueWriter writer;

        if (writers.TryGetValue(entry.SettingType, out writer))
        {
            return writer.Write(entry);
        }
        // Handle enum lists
        else if(
            entry.SettingType.IsGenericType &&
            entry.SettingType.GetGenericTypeDefinition() == ListType)
        {
            // Check if generic type of the list is an enum
            if (entry.SettingType.GenericTypeArguments.First().IsEnum)
            {
                var genericType = entry.SettingType.GenericTypeArguments.First();

                var constructedType = EnumListWriterType.MakeGenericType(genericType);

                writer = (IValueWriter)Activator.CreateInstance(constructedType);
                writers[entry.SettingType] = writer;

                return writer.Write(entry);
            }
            else
            {
                throw new NotSupportedException($"Unable to write config entries of type '{entry.SettingType.Name}'.");
            }
        }
        // Handle enums
        else if (entry.SettingType.IsEnum)
        {
            // Try generate enum writer for type.
            var constructedType = EnumWriterType.MakeGenericType(entry.SettingType);

            writer = (IValueWriter)Activator.CreateInstance(constructedType);
            writers[entry.SettingType] = writer;

            return writer.Write(entry);
        }
        // Handle nullable enums
        else if (Nullable.GetUnderlyingType(entry.SettingType)?.IsEnum == true)
        {
            // Try generate enum parser for type.
            var underlyingType = Nullable.GetUnderlyingType(entry.SettingType);
            var constructedType = NullableEnumWriterType.MakeGenericType(underlyingType);

            writer = (IValueWriter)Activator.CreateInstance(constructedType);
            writers[entry.SettingType] = writer;

            return writer.Write(entry);
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}
