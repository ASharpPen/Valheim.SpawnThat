using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using SpawnThat.Options.Conditions;
using SpawnThat.Options.Modifiers;
using SpawnThat.Options.PositionConditions;
using YamlDotNet.Core.Events;
using SpawnThat.Options.Identifiers;

namespace SpawnThat.Core.Network;

internal static class Serializer
{
    public static ISerializer ConfigureSerializer(HashSet<Type> registeredTypes = null)
    {
        var builder = new SerializerBuilder();

        if (registeredTypes is not null)
        {
            foreach (var type in registeredTypes)
            {
                builder.WithTagMapping("!" + type.AssemblyQualifiedName, type);
            }
        }

        return builder
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults | DefaultValuesHandling.OmitEmptyCollections)
            .WithTypeConverter(new YamlEnumWriter())
            .Build();
    }

    public static IDeserializer ConfigureDeserializer()
    {
        var deserializer = new DeserializerBuilder()
            .WithNodeTypeResolver(new TypeResolver())
            .Build();

        return deserializer;
    }

    public static byte[] SerializeAndCompress(this ISerializer serializer, object obj)
    {
        var serialized = serializer.Serialize(obj);

        var encoded = Encoding.Unicode.GetBytes(serialized);

        using var decompressedStream = new MemoryStream(encoded);
        using var compressedStream = new MemoryStream();

        using (var zipStream = new GZipStream(compressedStream, CompressionLevel.Optimal))
        {
            decompressedStream.CopyTo(zipStream);
        }

        var compressedSerialized = compressedStream.ToArray();

        return compressedSerialized;
    }

    public static T DeserializeCompressed<T>(this IDeserializer deserializer, byte[] serialized)
    {
        using var compressedStream = new MemoryStream(serialized);
        using var decompressedStream = new MemoryStream();

        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress, true))
        {
            zipStream.CopyTo(decompressedStream);
        }

        var decompressedBytes = decompressedStream.ToArray();
        string serializedString = Encoding.Unicode.GetString(decompressedBytes);

        T responseObject = deserializer.Deserialize<T>(serializedString);

        return responseObject;
    }

    /// <summary>
    /// Resolver for interface based-types, that would otherwise have trouble deserializing.
    /// </summary>
    private class TypeResolver : INodeTypeResolver
    {
        private List<Type> WhitelistedTypes { get; } = new()
        {
            typeof(ISpawnCondition),
            typeof(ISpawnPositionCondition),
            typeof(ISpawnModifier),
            typeof(ISpawnerIdentifier)
        };

        public bool Resolve(NodeEvent nodeEvent, ref Type currentType)
        {
            if (nodeEvent.Tag.IsEmpty || string.IsNullOrWhiteSpace(nodeEvent.Tag.Value))
            {
                return false;
            }

            // Retrieve type based on tag.
            var type = Type.GetType(nodeEvent.Tag.Value.Substring(1));

            if (type is null)
            {
                return false;
            }

            // Verify type is one of the whitelisted types.
            if (WhitelistedTypes.Any(x => x.IsAssignableFrom(type)))
            {
                // Set resolved type to tags type.
                currentType = type;
                return true;
            }

            return false;
        }
    }
}
