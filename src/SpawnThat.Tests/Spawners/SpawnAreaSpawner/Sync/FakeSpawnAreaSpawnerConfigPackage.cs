using SpawnThat.Core.Network;

namespace SpawnThat.Spawners.SpawnAreaSpawner.Sync;

/// <summary>
/// Test class for testing serialization and deserialization of <see cref="SpawnAreaSpawnerConfigPackage"/>,
/// without having to deal with types that might be stripped during testing, like <see cref="ZPackage"/>.
/// </summary>
internal class FakeSpawnAreaSpawnerConfigPackage : SpawnAreaSpawnerConfigPackage
{
    public FakeSpawnAreaSpawnerConfigPackage()
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
            .DeserializeCompressed<FakeSpawnAreaSpawnerConfigPackage>(compressedSerialized);

        if (responseObject is FakeSpawnAreaSpawnerConfigPackage compressedPackage)
        {
            compressedPackage.AfterUnpack(responseObject);
        }
    }
}
