using SpawnThat.Spawners.DestructibleSpawner.Identifiers;
using static Heightmap;

namespace SpawnThat.Spawners.DestructibleSpawner;

public static class IDestructibleSpawnerBuilderIdentifierExtensions
{
    public static IDestructibleSpawnerBuilder SetIdentifierName(this IDestructibleSpawnerBuilder builder, params string[] spawnerPrefabNames)
    {
        builder.SetIdentifier(new IdentifierName(spawnerPrefabNames));
        return builder;
    }

    public static IDestructibleSpawnerBuilder SetIdentifierBiome(this IDestructibleSpawnerBuilder builder, params Biome[] biomes)
    {
        builder.SetIdentifier(new IdentifierBiome(biomes));
        return builder;
    }

    public static IDestructibleSpawnerBuilder SetIdentifierLocation(this IDestructibleSpawnerBuilder builder, params string[] locations)
    {
        builder.SetIdentifier(new IdentifierLocation(locations));
        return builder;
    }

    public static IDestructibleSpawnerBuilder SetIdentifierRoom(this IDestructibleSpawnerBuilder builder, params string[] roomNames)
    {
        builder.SetIdentifier(new IdentifierRoom(roomNames));
        return builder;
    }
}
