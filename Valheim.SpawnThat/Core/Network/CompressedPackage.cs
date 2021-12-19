using System;
using System.IO.Compression;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Valheim.SpawnThat.Core.Network;

[Serializable]
internal abstract class CompressedPackage
{
    protected virtual void BeforePack() { }

    protected virtual void AfterUnpack(object obj) { }

    public ZPackage Pack()
    {
        BeforePack();

        ZPackage package = new();

        using (MemoryStream memStream = new MemoryStream())
        {
            using (var zipStream = new GZipStream(memStream, CompressionLevel.Optimal))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(zipStream, this);
            }

            byte[] serialized = memStream.GetBuffer();

            Log.LogDebug($"Serialized size: {serialized.Length} bytes");

            package.Write(serialized);
        }

        return package;
    }

    public static void Unpack(ZPackage package)
    {
        var serialized = package.ReadByteArray();

        Log.LogDebug($"Deserializing package size: {serialized.Length} bytes");

        using (MemoryStream memStream = new MemoryStream(serialized))
        {
            using (var zipStream = new GZipStream(memStream, CompressionMode.Decompress, true))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                var responseObject = binaryFormatter.Deserialize(zipStream);

                if (responseObject is CompressedPackage compressedPackage)
                {
                    compressedPackage.AfterUnpack(responseObject);
                }
            }
        }
    }
}
