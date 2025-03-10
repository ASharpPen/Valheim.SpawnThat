using System;
using System.Collections.Generic;

namespace SpawnThat.Core.Network;

public abstract class CompressedPackage
{
    protected HashSet<Type> RegisteredTypes { get; } = new();

    /// <summary>
    /// Register types that need to store the type info when serializing.
    /// This becomes relevant when storing lists of interfaces for instance.
    /// </summary>
    protected void RegisterType<T>(IEnumerable<T> objs)
    {
        foreach (var obj in objs)
        {
            RegisteredTypes.Add(obj.GetType());
        }
    }

    protected virtual void BeforePack() { }

    protected virtual void AfterUnpack(object obj) { }

    public ZPackage Pack()
    {
        BeforePack();

        var compressedSerialized = Serializer
            .ConfigureSerializer(RegisteredTypes)
            .SerializeAndCompress(this);

        Log.LogDebug($"Serialized size: {compressedSerialized.Length} bytes");

        ZPackage package = new();

        package.Write(compressedSerialized);

        return package;
    }

    public static void Unpack<T>(ZPackage package)
    {
        var serialized = package.ReadByteArray();

        Log.LogDebug($"Deserializing package size: {serialized.Length} bytes");

        var responseObject = Serializer
            .ConfigureDeserializer()
            .DeserializeCompressed<T>(serialized);

        if (responseObject is CompressedPackage compressedPackage)
        {
            compressedPackage.AfterUnpack(responseObject);
        }
    }
}
