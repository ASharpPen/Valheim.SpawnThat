using SpawnThat.Core.Network;

namespace SpawnThat.Spawners.LocalSpawner.Sync;

/// <summary>
/// Test class for testing serialization and deserialization of <see cref="LocalSpawnerConfigPackage"/>,
/// without having to deal with types that might be stripped during testing, like <see cref="ZPackage"/>.
/// </summary>
internal class FakeLocalSpawnerConfigPackage : LocalSpawnerConfigPackage
{
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
            .DeserializeCompressed<FakeLocalSpawnerConfigPackage>(compressedSerialized);

        if (responseObject is FakeLocalSpawnerConfigPackage compressedPackage)
        {
            compressedPackage.AfterUnpack(responseObject);
        }
    }
}
