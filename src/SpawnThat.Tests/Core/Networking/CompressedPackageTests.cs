using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpawnThat.Core.Network;
using YamlDotNet.Serialization;

namespace SpawnThat.Core.Networking;

[TestClass]
public class CompressedPackageTests
{
    [TestMethod]
    public void EnumsShouldSerializeAsValue()
    {
        var package = new FakePackage();

        (var serialized, var bytes) = Serialize();

        (var deserialized, var unpacked) = Deserialize(bytes);

        Assert.AreEqual(package.ModelWithEnum.Prop, unpacked.ModelWithEnum.Prop);
        Assert.AreEqual(package.ModelWithEnum.Prop2, unpacked.ModelWithEnum.Prop2);

        (string, byte[]) Serialize()
        {

            var serializerBuilder = new SerializerBuilder();

            var serialized = serializerBuilder
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults | DefaultValuesHandling.OmitEmptyCollections)
                .WithTypeConverter(new YamlEnumWriter())
                .Build()
                .Serialize(package);

            var encoded = Encoding.Unicode.GetBytes(serialized);

            using var decompressedStream = new MemoryStream(encoded);
            using var compressedStream = new MemoryStream();

            using (var zipStream = new GZipStream(compressedStream, CompressionLevel.Optimal))
            {
                decompressedStream.CopyTo(zipStream);
            }

            var compressedSerialized = compressedStream.ToArray();

            return (serialized, compressedSerialized);
        }

        (string, FakePackage) Deserialize(byte[] serialized)
        {
            using var compressedStream = new MemoryStream(serialized);
            using var decompressedStream = new MemoryStream();

            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress, true))
            {
                zipStream.CopyTo(decompressedStream);
            }

            var decompressedBytes = decompressedStream.ToArray();
            string serializedString = Encoding.Unicode.GetString(decompressedBytes);

            var deserializer = new DeserializerBuilder()
                .Build();

            var package = deserializer.Deserialize<FakePackage>(serializedString);

            return (serializedString, package);
        }
    }

    public enum FakeEnum
    {
        Invalid = 0,
        SomeValue = 1,
        SomeOtherValue = 4,
    }

    public class FakeModelWithEnum
    {
        public FakeEnum Prop { get; set; } = FakeEnum.SomeValue | FakeEnum.SomeOtherValue;

        public FakeEnum Prop2 { get; set; } = FakeEnum.SomeValue;
    }

    public class FakePackage : CompressedPackage
    {
        public FakeModelWithEnum ModelWithEnum = new();
    }
}

