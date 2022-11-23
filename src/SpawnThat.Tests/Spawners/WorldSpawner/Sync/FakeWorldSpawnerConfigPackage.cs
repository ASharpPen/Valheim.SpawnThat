using SpawnThat.Core.Network;

namespace SpawnThat.Spawners.WorldSpawner.Sync;

/// <summary>
/// Test class for testing serialization and deserialization of <see cref="WorldSpawnerConfigPackage"/>,
/// without having to deal with types that might be stripped during testing, like <see cref="ZPackage"/>.
/// </summary>
internal class FakeWorldSpawnerConfigPackage : WorldSpawnerConfigPackage
{
    public FakeWorldSpawnerConfigPackage()
    {
    }

    public byte[] FakePack()
    {
        BeforePack();

        var compressedSerialized = Serializer
            .ConfigureSerializer(RegisteredTypes)
            .SerializeAndCompress(this);

        return compressedSerialized;
    }

    public void FakeUnpack(byte[] compressedSerialized)
    {
        var responseObject = Serializer
            .ConfigureDeserializer()
            .DeserializeCompressed<FakeWorldSpawnerConfigPackage>(compressedSerialized);

        if (responseObject is FakeWorldSpawnerConfigPackage compressedPackage)
        {
            compressedPackage.AfterUnpack(responseObject);
        }
    }
}
