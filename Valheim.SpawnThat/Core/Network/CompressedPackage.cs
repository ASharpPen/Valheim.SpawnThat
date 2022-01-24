#define VERBOSE

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Valheim.SpawnThat.Spawn.Conditions;
using Valheim.SpawnThat.Spawn.Modifiers;
using Valheim.SpawnThat.Spawn.PositionConditions;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Valheim.SpawnThat.Core.Network;

public abstract class CompressedPackage
{
    private HashSet<Type> RegisteredTypes { get; } = new();

    protected void RegisterType<T>(SerializerBuilder builder, IEnumerable<T> objs)
    {
        foreach(var obj in objs)
        {
            RegisteredTypes.Add(obj.GetType());
        }
    }

    protected virtual void BeforePack(SerializerBuilder builder) { }

    protected virtual void AfterUnpack(object obj) { }

    public ZPackage Pack()
    {
        var serializerBuilder = new SerializerBuilder();

        BeforePack(serializerBuilder);

        foreach (var type in RegisteredTypes)
{
            serializerBuilder.WithTagMapping("!" + type.AssemblyQualifiedName, type);
        }

        ZPackage package = new();

        var serialized = serializerBuilder
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults | DefaultValuesHandling.OmitEmptyCollections)
            .Build()
            .Serialize(this);

#if VERBOSE && DEBUG
        Log.LogDebug(serialized);
#endif
        var encoded = Encoding.Unicode.GetBytes(serialized);

        using var decompressedStream = new MemoryStream(encoded);
        using var compressedStream = new MemoryStream();

        using (var zipStream = new GZipStream(compressedStream, CompressionLevel.Optimal))
        {
            decompressedStream.CopyTo(zipStream);
        }

        var compressedSerialized = compressedStream.ToArray();

        Log.LogDebug($"Serialized size: {compressedSerialized.Length} bytes");

        package.Write(compressedSerialized);

        return package;
    }

    public static void Unpack<T>(ZPackage package)
    {
        var serialized = package.ReadByteArray();

        Log.LogDebug($"Deserializing package size: {serialized.Length} bytes");

        using var compressedStream = new MemoryStream(serialized);
        using var decompressedStream = new MemoryStream();

        using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress, true))
        {
            zipStream.CopyTo(decompressedStream);
        }

        var decompressedBytes = decompressedStream.ToArray();
        string serializedString = Encoding.Unicode.GetString(decompressedBytes);

#if VERBOSE && DEBUG
        Log.LogDebug(serializedString);
#endif
        var deserializer = new DeserializerBuilder()
            .WithNodeTypeResolver(new TypeResolver())
            .Build();

        T responseObject = deserializer.Deserialize<T>(serializedString);

        if (responseObject is CompressedPackage compressedPackage)
        {
            compressedPackage.AfterUnpack(responseObject);
        }
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
            typeof(ISpawnModifier)
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
